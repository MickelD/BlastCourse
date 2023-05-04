using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Explosion Params", menuName = "Game Rules/Explosion Parameters"), System.Serializable]
public class ExplosionRules : ScriptableObject
{
    [field: SerializeField]
    [field: Tooltip("How much should the surface normal of the explosion affect the resulting trajectory")]
    public float SurfaceNormalInfluence { get; private set; }

    [field: SerializeField]
    [field: Tooltip("FOR OBJECTS: X is multiple of BlastForce to apply to XZ direction, Y is multiple of BlastForce to apply to Y direction")]
    public Vector2 ObjectDirectionDistribution { get; private set; }

    [field: SerializeField]
    [field: Tooltip("FOR PLAYERS: X is multiple of BlastForce to apply to XZ direction, Y is multiple of BlastForce to apply to Y direction")]
    public Vector2 PlayerDirectionDistribution { get; private set; }

    [field: Space(5)]
    [field: Tooltip("X (dist): Multiple of BlastRadius up until which BlastForce is affected by corresponding Mult. value " +
                  "\nY (mult): Multiple of BlastForce applied up until corresponding Dist. value")]
    [field: SerializeField]
    public Vector2[] ExplosionForceSubdivisions { get; private set; }


    [field: Space(5)]
    [field: SerializeField]
    public float ExplosionLifetime { get; private set; }

    [field: Space(5)]
    [field: Tooltip("Layers to apply forces to")]
    [field: SerializeField]
    [SerializeField] public LayerMask ExplosionLayerMask { get; private set; }
}
