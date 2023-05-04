using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpgBase : ScriptableObject 
{
    #region Variables

    private RpgStats _stats;
    private LayerMask _aimLayerMask;
    private float _maxAimDistance;
    private Transform _cameraTransform;
    private Vector3 _fireOffset;

    #endregion

    #region Methods

    public virtual void InitializeValues(RpgStats stats, LayerMask aimLayerMask, float maxAimDistance,Transform cameraTransform, Vector3 fireOffset)
    {
        _stats = stats;
        _aimLayerMask = aimLayerMask;
        _maxAimDistance = maxAimDistance;
        _cameraTransform = cameraTransform;
        _fireOffset = fireOffset;
    }

    public virtual void Tick()
    {

    }

    public virtual void PrimaryFire()
    {
        //if (Physics.Raycast(g_playerCam.transform.position, g_playerCam.transform.forward, out RaycastHit hitInfo, _maxAimDistance, _aimLayerMask, QueryTriggerInteraction.Ignore))
        //{
        //    _targetPoint = hitInfo.point;
        //}
        //else
        //{
        //    _targetPoint = g_playerCam.transform.position + g_playerCam.transform.forward * _maxAimDistance;
        //}

        //_targetDirection = (_targetPoint - g_rocketOrigin.position).normalized;

        //c_animator.SetTrigger(_fireTriggerAnimatorTag);

        //RocketBase instantiatedRocket = Object.Instantiate(g_rocketPrefab, g_rocketOrigin.position, Quaternion.LookRotation(_targetDirection)).GetComponent<RocketBase>();

        //instantiatedRocket.SetVelocity(_targetDirection * _rocketSpeed);

        //if (RPGDebugger.Instance != null)
        //{
        //    RPGDebugger.Instance.DrawLine(_targetPoint, g_rocketOrigin.position);
        //    RPGDebugger.Instance.DrawRay(g_rocketOrigin.position, _targetDirection);
        //    RPGDebugger.Instance.DrawPoint(_targetPoint);
        //}

        //return instantiatedRocket;
    }

    public virtual void SecondaryFire()
    {

    }

    #endregion

    #region Debug


    #endregion
}
