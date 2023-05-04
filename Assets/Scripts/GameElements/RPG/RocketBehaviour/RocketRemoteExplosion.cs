using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketRemoteExplosion : RocketBase
{
    #region Variables

    private RpgRemote _rpg;

    #endregion

    #region Methods

    public void SetRPG(RpgRemote rpg)
    {
        _rpg = rpg;
    }

    public void RemoteExplosion()
    {
        _rpg.RemoveFromList(this);
        base.Explode(transform.position, Vector3.up);
    }

    public override void Explode(Vector3 center, Vector3 direction)
    {
        _rpg.RemoveFromList(this);
        base.Explode(center, direction);
    }

    public override void Diffuse()
    {
        _rpg.RemoveFromList(this);
        base.Diffuse();
    }

    #endregion
}
