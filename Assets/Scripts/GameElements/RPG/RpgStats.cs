using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RPG Stats", menuName = "Game Rules/RPG Stats"), System.Serializable]
public class RpgStats : ScriptableObject
{

    [field: Header("Launcher Stats")]
    [field: SerializeField] public float FireSpeed { get; private set; }
    [field: SerializeField, Range(0f, 1f)] public float Cost { get; private set; }

    [Header("Rocket Stats")]
    [SerializeField] public GameObject RocketPrefab;
    [field: SerializeField] public float RocketSpeed { get; private set; }

    [Header("Explosion Stats")]
    [SerializeField] public Explosion Explosion;
}
