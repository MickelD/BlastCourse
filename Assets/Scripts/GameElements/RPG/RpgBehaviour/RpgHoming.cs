using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingBehaviour", menuName = "RPG Behaviour/Homing Launcher")]
public class RpgHoming : RpgBase
{
    //#region Fields

    //[Space(5), Header("Directed Laser"), Space(3)]
    //[SerializeField] private string _targetingButtonName;
    //[SerializeField] private float _maxTargetingDistance;
    //[SerializeField] private LineRenderer c_lr;

    //#endregion

    //#region Variables

    //private RocketDirected _rocket;

    //#endregion

    //#region UnityFunctions

    //protected new void Update()
    //{
    //    base.Update();

    //    if (Input.GetButton(_targetingButtonName))
    //    {
    //        c_lr.enabled = true;

    //        RaycastHit hit;
    //        if (Physics.Raycast(g_playerCam.transform.position, g_playerCam.transform.forward, out hit, _maxTargetingDistance, _aimLayerMask, QueryTriggerInteraction.Ignore))
    //        {
    //            _rocket.SetTarget(hit.point);

    //            c_lr.startColor = Color.green;
    //            c_lr.endColor = Color.green;
    //            c_lr.SetPosition(0,g_rocketOrigin.position);
    //            c_lr.SetPosition(1, hit.point);
    //        }
    //        else
    //        {
    //            _rocket.SetIsTargeting(false);

    //            c_lr.startColor = Color.red;
    //            c_lr.endColor = Color.red;
    //            c_lr.SetPosition(0, g_rocketOrigin.position);
    //            c_lr.SetPosition(1, g_playerCam.transform.forward * _maxTargetingDistance);
    //        }
    //    }
    //    else
    //    {
    //        _rocket.SetIsTargeting(false);
    //        c_lr.enabled = false;
    //    }
    //}

    //#endregion

    //#region Methods

    //protected override RocketBase Shoot()
    //{
    //    if (_rocket == null)
    //    {
    //        _rocket = base.Shoot().gameObject.GetComponent<RocketDirected>();
    //        _rocket.SetSpeed(_rocketSpeed);
    //    }

    //    return null;
    //}

    //#endregion
}
