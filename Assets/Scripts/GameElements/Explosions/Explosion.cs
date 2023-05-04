using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Explosion
{
    #region Fields

    [Space(5), Header("Prefab"), Space(3)]
    [SerializeField] GameObject g_explosionPrefab;

    [Space(5), Header("Stats"), Space(3)]
    [field: SerializeField] public float BlastRadius;
    [field: SerializeField] public float BlastForce;
    [field: SerializeField] public float Damage;
    [field: SerializeField] public Health.Source Source;

    [Space(5), Header("Utility"), Space(3)]
    [field: SerializeField] public ExplosionRules ExplosionRules;

    #endregion

    #region Methods

    public void Explode(Vector3 origin, Vector3 normal)
    {
        GameObject _instantiatedExplosion = Object.Instantiate(g_explosionPrefab, origin, Quaternion.identity);
        Object.Destroy(_instantiatedExplosion, ExplosionRules.ExplosionLifetime);

        _instantiatedExplosion.transform.localScale = Vector3.one * BlastRadius;

        //add Impulse
        Collider[] hitColliders = new Collider[10];
        int numColliders = Physics.OverlapSphereNonAlloc(origin, BlastRadius, hitColliders, ExplosionRules.ExplosionLayerMask);

        for (int i = 0; i < numColliders; i++)
        {
            Rigidbody hitRB = hitColliders[i].gameObject.GetComponent<Rigidbody>();

            if (hitRB != null)
            {
                PlayerMovement playerMovement = hitRB.GetComponent<PlayerMovement>();
                if (playerMovement != null) //THIS IS THE PLAYER, notify that we started rocketjump
                {
                    playerMovement.SetRocketJump(origin, this, normal);
                }
                else //THIS IS A PHYSICS OBJECT
                {
                    //direction & distance
                    Vector3 dir = hitRB.position - origin;

                    //applyforce
                    hitRB.AddForce(BlastForce 
                                 * DetermineModifierBySegmentSquared(dir.sqrMagnitude) 
                                 * ApplyDirectionModifier(dir, ExplosionRules.ObjectDirectionDistribution),
                                   ForceMode.Impulse);
                }

                //debug
                Debug.DrawRay(origin, (hitRB.position - origin).normalized * BlastRadius, Color.red, 1f);
            }

            Health hitHP = hitColliders[i].gameObject.GetComponent<Health>();

            if(hitHP != null)
            {
                hitHP.SufferDamage(Damage, Source);
            }
        }
    }

    #endregion

    #region Static Methods

    /// <summary>
    /// Multiplies Y component of passed direction by passed modifier.
    /// Returns resulting vector normalized
    /// </summary>
    public Vector3 ApplyDirectionModifier(Vector3 dirToModify, Vector2 modiferToUse)
    {
        dirToModify.x *= modiferToUse.x;
        dirToModify.z *= modiferToUse.x;
        dirToModify.y *= modiferToUse.y;
        return dirToModify.normalized;
    }

    /// <summary>
    /// Determines the strenght of the blast force based on the Explosion Subdivisions of this Explosion
    /// </summary>
    public float DetermineExplosionForceBySegment(float distance)
    {
        //Use Linq statement to find the first valid explosion subdivision
        return BlastForce * ExplosionRules.ExplosionForceSubdivisions
        .Where(t => distance <= BlastRadius * t.x).FirstOrDefault().y;

        //There is a chance that using a Linq statement generates more garbage than this weird loop, so keep it here just in case
        //foreach (Vector2 explosionSegment in ExplosionRules.ExplosionForceSubdivisions)
        //{
        //    if (distance <= BlastRadius * explosionSegment.x)
        //    {
        //        usedBlastForce = BlastForce * explosionSegment.y;
        //        break;
        //    }
        //    else
        //    {
        //        continue;
        //    }
        //}
    }

    /// <summary>
    /// Determines the Multiplier based on the Explosion Subdivisions of this Explosion
    /// </summary>
    public float DetermineModifierBySegment(float distance)
    {
        //Use Linq statement to find the first valid explosion subdivision
        return ExplosionRules.ExplosionForceSubdivisions
        .Where(t => distance <= BlastRadius * t.x).FirstOrDefault().y;
    }

    /// <summary>
    /// Uses a squared magnitude for the distance comparison
    /// </summary>
    public float DetermineModifierBySegmentSquared(float squaredDistance)
    {
        //Use Linq statement to find the first valid explosion subdivision
        return ExplosionRules.ExplosionForceSubdivisions
        .Where(t => squaredDistance <= (BlastRadius * t.x) * (BlastRadius * t.x)).FirstOrDefault().y;
    }

    #endregion
}
