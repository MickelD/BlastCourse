using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CustomMethods;
using Unity.Mathematics;

public class PlayerMovement : MonoBehaviour
{
    #region Fields
    [Header("Horizontal Movement"), Space(3)]
    [SerializeField] float _groundSpeed;
    [SerializeField] float _groundFriction;
    [Space(2), SerializeField] float _airSpeed;
    [SerializeField] float _airDrag;

    [Space(5), Header("Sliding"), Space(3)]
    [SerializeField] float _slideSpeed;
    [SerializeField] float _slideDrag;
    [SerializeField] float _maxSlideTime;
    [SerializeField] float _minSpeedToSlide;
    [SerializeField] float _slideCooldown;
    [SerializeField] float _slideScale;
    [SerializeField] float _slideScalingSpeed;

    [Space(5), Header("Vertical Movement"), Space(3)]
    [SerializeField] float _jumpForce;
    [SerializeField] float _rocketJumpDrag;
    [SerializeField] float _yCancelMultiplier;

    [Space(5), Header("Input"), Space(3)]
    [SerializeField] string _horAxisName;
    [SerializeField] string _verAxisName;
    [SerializeField] string _jumpButtonName;
    [SerializeField] string _slideButtonName;
    [SerializeField] string _rjCancelButtonName;

    [Space(5), Header("Surface Interaction"), Space(3)]
    [SerializeField] float _surfaceCheckDistance;
    [SerializeField] float _maxSlopeAngle;
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] LayerMask _wallLayerMask;

    [Space(5), Header("Components"), Space(3)]
    [SerializeField] private Rigidbody c_rb;
    [SerializeField] private CapsuleCollider c_hitbox;
    [SerializeField] private Transform g_orientation;
    [SerializeField] private Transform g_centerOfGravity;

    [Space(5), Header("Inputs"), Space(3)]
    [SerializeField] float _coyoteTime;
    [SerializeField] float _inputBufferTime;
    [SerializeField] float _jumpCooldown;

    [Space(5), Header("Data"), Space(3)]
    [SerializeField] float _notifyVelocityInterval;

    #endregion

    #region Vars
    //CONDITIONS
    private bool _isGrounded;

    private bool _isRocketJumping;
    private bool _readyToJump = true;

    private bool _slideInput;
    private bool _isSliding;
    private bool _readyToSlide = true;
    private bool _expiredSlideInput;

    private bool _onGraviPad;

    //ACTIVE COROUTINES
    private Coroutine _currentlyActiveSlideTimer;

    //SPEED VALUES
    private float _playerAbsoluteSpeed;
    private float _playerHorizontalSpeed;
    private float _playerVerticalVelocity;

    //VECTORS
    private Vector2 _inputVec;
    private Vector3 _movementDirection;

    //SURACE DETECTION
    private float _walkAngle;
    private float _previousSurfaceAngle;

    //QoL
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private float _RJCancelBufferTimer;
    #endregion

    #region UnityFunctions

    private void Start()
    {
        c_rb.freezeRotation = true;

        StartCoroutine(NotifyUpdateSpeed());
    }

    private void Update()
    {
        GroundCheck();
        ReadMovementInput();
        JumpInput();
        SlideInput();
        RJCancelInput();
        UpdateDrag();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
    }

    #endregion

    #region Methods

    public void PushPull(Vector3 direction)
    {
        c_rb.AddForce(direction, ForceMode.Acceleration);
    }

    private void ReadMovementInput()
    {
        _inputVec = new Vector2(Input.GetAxisRaw(_horAxisName), Input.GetAxisRaw(_verAxisName));

        _inputVec.y = _isGrounded ? _inputVec.y : Mathf.Clamp01(_inputVec.y);
    }
    private void JumpInput()
    {
        _jumpBufferTimer = Mathf.Clamp(_jumpBufferTimer + Time.deltaTime, 0f, _inputBufferTime * 2);

        if (Input.GetButton(_jumpButtonName))
        {
            _jumpBufferTimer = 0;
        }

        if (_jumpBufferTimer <= _inputBufferTime && _coyoteTimer <= _coyoteTime && _readyToJump && !_onGraviPad)
        {
            Jump();
        }
    }

    private void SlideInput()
    {
        _slideInput = Input.GetButton(_slideButtonName);

        if (!_expiredSlideInput)
        {
            if (_slideInput && _isGrounded && _playerHorizontalSpeed >= _minSpeedToSlide && _inputVec.y == 1)
            {
                TryStartSlide();
            }
        }
        else
        {
            _expiredSlideInput = !Input.GetButtonDown(_slideButtonName);
        }

        if (_isSliding && (_playerHorizontalSpeed < _minSpeedToSlide || !_slideInput || !_isGrounded))
        {
            StopSlide();
        }
    }

    private void RJCancelInput()
    {
        _RJCancelBufferTimer = Mathf.Clamp(_RJCancelBufferTimer + Time.deltaTime, 0f, _inputBufferTime * 2);

        if (Input.GetButton(_rjCancelButtonName))
        {
            _RJCancelBufferTimer = 0;
        }
    }

    private void HorizontalMovement()
    {
        //if we are sliding, we cannot strafe
        _movementDirection = _isSliding? g_orientation.forward : (g_orientation.forward * _inputVec.y + g_orientation.right * _inputVec.x).normalized;

        if (_movementDirection != Vector3.zero)
        {
            if (Physics.Raycast(transform.position, _movementDirection, out RaycastHit hitWall, c_hitbox.radius + _surfaceCheckDistance, _wallLayerMask))
            {
                //WE HAVE DETECTED A WALL IN OUR DESIRED MOVEMENT DIRECTION, SO NULLIFY MOVEMENT
                if ((90f - Vector3.Angle(-_movementDirection, hitWall.normal)) >= _maxSlopeAngle)
                {
                    _isSliding = false;
                    return;
                }
            }

            float desiredSpeed = _isGrounded ? _isSliding? _slideSpeed : _groundSpeed : _airSpeed;          

            c_rb.AddForce(_movementDirection * desiredSpeed, ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        _readyToJump = false;

        c_rb.velocity = new Vector3(c_rb.velocity.x, 0, c_rb.velocity.z);

        c_rb.AddForce(transform.up * _jumpForce, ForceMode.VelocityChange);

        //ensure jump Inputs cannot be duplicated
        this.Invoke(() => _readyToJump = true, _jumpCooldown);
    }

    private void GroundCheck()
    {
        if (Physics.Raycast(g_centerOfGravity.position, Vector3.down, out RaycastHit hitGround, g_centerOfGravity.localPosition.y + _surfaceCheckDistance, _groundLayerMask))
        {
            _walkAngle = Vector3.Angle(Vector3.up, hitGround.normal);

            if (_walkAngle <= _maxSlopeAngle) //IN A VALID SURFACE
            {
                if (!_isGrounded) EventManager.OnPlayerLanded?.Invoke(c_rb.velocity.y);

                _isGrounded = true;
                c_rb.useGravity = _isSliding;

                if (_walkAngle != _previousSurfaceAngle) //ONLY REORIENT PLAYER IF SURFACE ANGLE HAS CHANGED
                {
                    _previousSurfaceAngle = _walkAngle;

                    //reorient 
                    ReorientUpVector(hitGround.normal);

                    //Stick To Ground
                    c_rb.velocity = new Vector3(c_rb.velocity.x, 0f, c_rb.velocity.z);
                }
            }
            else //SURFACE IS TO STEEP
            {
                SetAirBorne();
            }
        }
        else //NO SURFACE DETECTED
        {
            SetAirBorne();
        }

        _coyoteTimer = _isGrounded ? 0f : _coyoteTimer += Time.deltaTime;
    }

    private void SetAirBorne()
    {
        ReorientUpVector(Vector3.up);

        _previousSurfaceAngle = 0f;
        _isGrounded = false;
        c_rb.useGravity = true;
    }

    private void ReorientUpVector(Vector3 newUp)
    {
        float Yrot = g_orientation.localEulerAngles.y;
        g_orientation.up = newUp.normalized;
        g_orientation.Rotate(0f, Yrot, 0f, Space.Self);
    }

    private void UpdateDrag()
    {
        //we stop "rocketjumping" when we start falling(when our Yvel becomes negative)
        if (_isRocketJumping && _playerVerticalVelocity < 0f)
        {
            _isRocketJumping = _isRocketJumping && _playerVerticalVelocity < 0f;
        }

        //set drag: if we are on ground do ground drag, if we are on air check if we are rocket jumping and change drag accordingly
        c_rb.drag = _isGrounded ? 
                      _isSliding? _slideDrag : _groundFriction //ONGROUND
                    : _isRocketJumping ? _rocketJumpDrag : _airDrag; //ONAIR
    }

    public void SetRocketJump(Vector3 origin, Explosion exp, Vector3 normal)
    {
        _isRocketJumping = true;

        float mod = exp.DetermineModifierBySegment((c_rb.position - origin).magnitude);

        //DETERMINE Y FORCE MAGNITUDE
        float yForce =
              exp.BlastForce
            * math.select(_yCancelMultiplier, mod, (_RJCancelBufferTimer > _inputBufferTime));

        //DETERMINE XZ DIRECTION
        Vector3 xzDir = ExtendedMathUtility.HorizontalDirection(origin, c_rb.position);

        //DETERMINE XZ FORCE MAGNITUDE
        float xzForce =
              exp.BlastForce
            * mod;

        //Apply force
        //c_rb.AddForce(exp.ExplosionRules.PlayerDirectionDistribution.x                  //HORIZONTAL MOVEMENT
        //                * (exp.ExplosionRules.SurfaceNormalInfluence * yForce * normal      //Normal repulsion
        //                + xzDir * xzForce)                                                  //Horizontal explosion force
        //            + exp.ExplosionRules.PlayerDirectionDistribution.y                  //VERTICAL MOVEMENT
        //                * (1 - exp.ExplosionRules.SurfaceNormalInfluence)                   //
        //                * yForce * Vector3.up, ForceMode.Impulse);

        //Apply Force
        c_rb.AddForce(
            //SURFACE NORMAL REPULSION
            exp.BlastForce * exp.ExplosionRules.SurfaceNormalInfluence * normal 
            //NORMAL-INDEPENDENT IMPULSE
            + (1 - exp.ExplosionRules.SurfaceNormalInfluence) * (
                //Horizontal Impulse
                exp.ExplosionRules.PlayerDirectionDistribution.x * xzForce * xzDir
                //vertical Impulse
              + exp.ExplosionRules.PlayerDirectionDistribution.y * yForce * Vector3.up
            ),     
            ForceMode.Impulse
        );
    }

    private void TryStartSlide()
    {
        if (!_isSliding && _readyToSlide)
        {
            _isSliding = true;
            _readyToSlide = false;

            //just in case, this code is technically not needed
            if(_currentlyActiveSlideTimer != null) StopCoroutine(_currentlyActiveSlideTimer);

            _currentlyActiveSlideTimer = StartCoroutine(SlideTimer());

            transform.DOScaleY(_slideScale, _slideScalingSpeed).SetEase(Ease.OutSine);
        }
    }

    private void StopSlide()
    {
        _isSliding = false;
        _expiredSlideInput = true;
        this.Invoke(() => _readyToSlide = true, _slideCooldown);

        if (_currentlyActiveSlideTimer != null) StopCoroutine(_currentlyActiveSlideTimer);

        transform.DOScaleY(1f, _slideScalingSpeed).SetEase(Ease.OutSine);
    }

    private IEnumerator SlideTimer()
    {
        yield return new WaitForSeconds(_maxSlideTime);
        StopSlide();
    }
    

    #endregion

    #region Getters/Setters
    public void SetGraviPad(bool value)
    {
        _onGraviPad = value;
    }
    public bool GetGraviPad()
    {
        return _onGraviPad;
    }

    #endregion

    #region Debug

    private IEnumerator NotifyUpdateSpeed()
    {
        WaitForSecondsRealtime _speedNotifyDelay = new (_notifyVelocityInterval);

        while (gameObject.activeInHierarchy)
        {
            _playerAbsoluteSpeed = c_rb.velocity.magnitude; //X
            _playerVerticalVelocity = c_rb.velocity.y; //Y
            _playerHorizontalSpeed = ExtendedMathUtility.VectorXZMagnitude(c_rb.velocity); //XZ

            EventManager.OnUpdatePlayerSpeedXYZ?.Invoke(_playerAbsoluteSpeed);
            EventManager.OnUpdatePlayerSpeedXZ?.Invoke(_playerHorizontalSpeed);
            EventManager.OnUpdatePlayerSpeedY?.Invoke(_playerVerticalVelocity);  

            yield return _speedNotifyDelay;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawRay(transform.position, _movementDirection * 10f);

        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, c_rb.velocity * 10f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(g_centerOfGravity.position, Vector3.down *  (_surfaceCheckDistance + g_centerOfGravity.localPosition.y));
    }

    #endregion
}

//VELOCITY REFLECTION FORMULA
//velocityDir.x *= Mathf.Sign(origin.x - c_rb.position.x) == Mathf.Sign(velocityDir.x) ? -1 : 1;
//velocityDir.y = Mathf.Abs(velocityDir.y);
//velocityDir.z *= Mathf.Sign(origin.z - c_rb.position.z) == Mathf.Sign(velocityDir.z) ? -1 : 1;
