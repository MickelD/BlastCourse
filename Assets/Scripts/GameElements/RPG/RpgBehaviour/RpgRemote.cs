using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoteBehaviour", menuName = "RPG Behaviour/Remote Detonation")]
public class RpgRemote : RpgBase
{
    #region Fields

    [Space(5), Header("Remote Explosion"), Space(3)]
    [SerializeField] private string _remoteExplosionButtonName;
    [SerializeField] private float _remoteExplosionCooldown;

    #endregion

    #region Variables

    private List<RocketRemoteExplosion> _remoteRockets = new List<RocketRemoteExplosion>();
    private float _remoteExplosionTimer;


    #endregion

    #region UnityFunctions

    //private new void Update()
    //{
    //    base.Update();

    //    _remoteExplosionTimer -= Time.deltaTime;
    //    if (Input.GetButton(_remoteExplosionButtonName) && _remoteExplosionTimer <= 0)
    //    {
    //        _remoteExplosionTimer = _remoteExplosionCooldown;
    //        RemoteExplosion();
    //    }

    //}

    #endregion

    #region Methods

    private void RemoteExplosion()
    {
        //RocketRemoteExplosion closestRocket = null;
        //for (int i = 0; i < _remoteRockets.Count; i++)
        //{
        //    RocketRemoteExplosion rocket = _remoteRockets[i];
        //    float angle = Vector3.Angle(transform.forward, rocket.transform.position - transform.position);
        //    float currestClosestAngle = 360;
        //    if (closestRocket != null) currestClosestAngle = Vector3.Angle(transform.forward, closestRocket.transform.position - transform.position);

        //    if (rocket != null && angle < 90 && angle < currestClosestAngle)
        //    {
        //        closestRocket = rocket;
        //    }
        //}
        //if (closestRocket != null) closestRocket.RemoteExplosion();
    }

    //protected override RocketBase Shoot()
    //{
    //    RocketRemoteExplosion rocket = base.Shoot().gameObject.GetComponent<RocketRemoteExplosion>();
    //    if (rocket != null)
    //    {
    //        _remoteRockets.Add(rocket);
    //        rocket.SetRPG(this);
    //    }

    //    return null;
    //}

    public void RemoveFromList(RocketRemoteExplosion rocket)
    {
        _remoteRockets.Remove(rocket);
    }

    #endregion
}
