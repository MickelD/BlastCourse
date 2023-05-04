using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDroneHealth : Health
{
    [Space(5), Header("Explosion"), Space(3)]
    [SerializeField] protected Explosion _explosion;

    public override void Die()
    {
        _explosion.Explode(transform.position, transform.up);
        Destroy(this.gameObject);
    }
}
