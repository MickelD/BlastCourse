using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDeffuserTurret : MonoBehaviour
{
    #region Fields
    [Space(5), Header("Pieces"), Space(3)]
    [SerializeField] GameObject g_head;
    [SerializeField] GameObject g_cannon;
    [SerializeField] GameObject g_gunPoint;
    [SerializeField] SphereCollider c_collider;

    [Space(5), Header("Variables"), Space(3)]
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _backRotationSpeed;
    [SerializeField] float _shootAngle;
    [SerializeField] float _rotationAngle;
    [SerializeField] float _shootDelay;
    [SerializeField] float _range;
    [SerializeField] float _instantRange;

    [Space(5), Header("Laser"), Space(3)]
    [SerializeField] LineRenderer c_lr;
    [SerializeField] float _laserDuration;
    [SerializeField] float _laserSteps;
    [SerializeField] float _laserWiggliness;

    #endregion

    #region Variables
    private bool inRange;
    private bool inSight;

    private float laserTimer;
    private float shotTimer;

    private Vector3 headOrientation;
    private Vector3 cannonOrientation;

    private Transform target;

    #endregion

    #region UnityFunctions

    private void OnValidate()
    {
        c_collider.center = g_head.transform.position - transform.position;
        c_collider.radius = _range;
    }

    public void Update()
    {
        if (target != null)
        {
            //Visibility && Angle of Missile
            RaycastHit hit;
            Vector3 distance = target.transform.position - g_head.transform.position;
            if (distance.magnitude <= _range && Physics.Raycast(g_gunPoint.transform.position, target.transform.position - g_gunPoint.transform.position, out hit, _range))
            {
                if (hit.transform.tag.Equals("Rocket"))
                {
                    inRange = true;
                    Vector3 playerDirection = target.transform.position - g_head.transform.position;
                    playerDirection.y = 0;
                    if (Mathf.Abs(Vector3.Angle(g_head.transform.forward, playerDirection)) <= _rotationAngle || distance.magnitude <= _instantRange)
                    {
                        inSight = true;
                    }
                    else inSight = false;
                }
                else inRange = false;
            }
            else inRange = false;

            //The player is in Range
            if (inRange)
            {
                float currentRotationSpeed;
                //In Front
                if (inSight)
                {
                    currentRotationSpeed = _rotationSpeed;
                }
                //Behind
                else
                {
                    currentRotationSpeed = _backRotationSpeed;
                }


                //Rotate Y Axis (Head)
                headOrientation = (target.transform.position - g_head.transform.position);
                headOrientation.y = 0;
                Quaternion headRotation = Quaternion.LookRotation(headOrientation, transform.up);
                g_head.transform.rotation = Quaternion.Slerp(g_head.transform.rotation, headRotation, currentRotationSpeed * Time.deltaTime);
                //Rotate X Axis (Cannon)
                cannonOrientation = (target.transform.position - g_cannon.transform.position);
                g_cannon.transform.localRotation = Quaternion.Euler(Vector3.right * 13 * -cannonOrientation.y);


                //Shoot
                if (shotTimer <= 0 && Mathf.Abs(Vector3.Angle(g_head.transform.forward, headOrientation)) <= _shootAngle)
                {
                    shotTimer = _shootDelay;
                    SetLaser();
                    target.GetComponent<RocketBase>().Diffuse();
                    target = null;
                }
            }
            shotTimer -= Time.deltaTime;
        }

        //Draw Range
        Debug.DrawLine(g_head.transform.position, Quaternion.Euler(0, _rotationAngle, 0) * g_head.transform.forward * _range + g_head.transform.position, Color.blue, 0.2f);
        Debug.DrawLine(g_head.transform.position, Quaternion.Euler(0, -_rotationAngle, 0) * g_head.transform.forward * _range + g_head.transform.position, Color.blue, 0.2f);
        //Debug.Log(Mathf.Abs(Vector3.Angle(head.transform.forward, headOrientation)));

        //Animate && Deactivate Laser
        if (c_lr.enabled)
        {
            laserTimer -= Time.deltaTime;
            WiggleLaser();
            if (laserTimer <= 0)
            {
                laserTimer = _laserDuration;
                c_lr.enabled = false;
            }
        }
    }

    #endregion

    #region Methods
    private void SetLaser()
    {
        if (target != null)
        {
            c_lr.enabled = true;
            Vector3 distance = target.transform.position - g_gunPoint.transform.position;
            bool createRope = true;

            int count = 0;
            c_lr.positionCount = (int)(distance.magnitude / _laserSteps) + 1;
            if (distance.magnitude % _laserSteps > 0)
            {
                c_lr.positionCount++;
            }
            Vector3 previousPos = g_gunPoint.transform.position;
            c_lr.SetPosition(count, g_gunPoint.transform.position);
            count++;

            while (createRope)
            {
                if (((distance.normalized * _laserSteps + previousPos) - g_gunPoint.transform.position).magnitude > distance.magnitude)
                {
                    createRope = false;
                    c_lr.SetPosition(count, target.transform.position);
                }
                else
                {
                    previousPos = distance.normalized * _laserSteps + previousPos;
                    c_lr.SetPosition(count, previousPos);
                    count++;
                }
            }
        }

    }

    private void WiggleLaser()
    {
        for (int i = 1; i < c_lr.positionCount - 1; i++)
        {
            c_lr.SetPosition(i, c_lr.GetPosition(i) + Random.insideUnitSphere * _laserWiggliness / 100);
        }
    }
    #endregion

    #region Trigger&Collision

    private void OnTriggerStay(Collider other)
    {
        
        var collidingTarget = other.GetComponent<RocketBase>();
        if (target == null && collidingTarget != null)
        {
            target = collidingTarget.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
            var collidingTarget = other.GetComponent<RocketBase>();
            if (collidingTarget != null && target == collidingTarget.transform)
            {
                target = null;
            }
        
    }

    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        //Draw Range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(g_head.transform.position, _range);
        //Draw Instant Range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(g_head.transform.position, _instantRange);
        //Draw Angle
        Gizmos.DrawLine(g_head.transform.position, Quaternion.Euler(0, _rotationAngle, 0) * g_head.transform.forward * _range + g_head.transform.position);
        Gizmos.DrawLine(g_head.transform.position, Quaternion.Euler(0, -_rotationAngle, 0) * g_head.transform.forward * _range + g_head.transform.position);
        //Draw Shoot Range
        Gizmos.color = Color.red;
        Gizmos.DrawLine(g_head.transform.position, Quaternion.Euler(0, _shootAngle, 0) * g_head.transform.forward * _range + g_head.transform.position);
        Gizmos.DrawLine(g_head.transform.position, Quaternion.Euler(0, -_shootAngle, 0) * g_head.transform.forward * _range + g_head.transform.position);
    }

    #endregion
}
