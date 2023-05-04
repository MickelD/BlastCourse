using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMethods;

public class RpgHolder : MonoBehaviour
{
    #region LauncherData

    public enum FiringMode
    {
        classic,
        remote,
        homing
    }

    [System.Serializable] protected class RpgCollection
    {
        [SerializeField, Space(3)] public RpgData _classicRPG;
        [SerializeField, Space(3)] public RpgData _remoteRPG;
        [SerializeField, Space(3)] public RpgData _homingRPG;
    }

    [System.Serializable]
    protected class RpgData
    {
        [SerializeField] public RpgBase _rpgBehaviour;
        [SerializeField] public RpgStats _rpgStats;
        [SerializeField] public Vector3 _fireOriginOffset;
    }

    #endregion

    #region Fields

    [Space(5), Header("Shared Values"), Space(3)]
    [SerializeField] private Vector3 _fireOrigin;
    [SerializeField] private LayerMask _aimLayerMask;
    [SerializeField] private float _maxAimDistance;

    [Space(5), Header("RPG Collection"), Space(3)]
    [SerializeField] private FiringMode _fireMode;
    [SerializeField] private RpgCollection _rpgCollection;

    [Space(5), Header("Inputs"), Space(3)]
    [SerializeField] private string _primaryFireButtonName;
    [SerializeField] private string _secondaryFireButtonName;

    #endregion

    #region Variables

    private RpgData _currentRpg;

    #endregion

    #region UnityFunctions

    private void OnValidate()
    {
        SetFiringMode(_fireMode);
    }

    private void Start()
    {
        SetFiringMode(FiringMode.classic);
    }

    private void Update()
    {
        //Launcher Behaviour

        _currentRpg._rpgBehaviour.Tick();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + _fireOrigin + _currentRpg._fireOriginOffset, "gzFireOrigin", true);
    }

    #endregion

    #region Methods

    public void InitializeAllRpgBehaviours()
    {
        foreach (RpgData rpgData in ExtendedDataUtility.GetAllFieldsFromTypeInObject<RpgData>(_rpgCollection))
        {
            //rpgData._rpgBehaviour.InitializeValues(rpgData._rpgStats);
        }
    }

    public void SetFiringMode(FiringMode fireMode)
    {
        _fireMode = fireMode;

        _currentRpg = _fireMode switch
        {
            FiringMode.classic => _rpgCollection._classicRPG,
            FiringMode.remote => _rpgCollection._remoteRPG,
            FiringMode.homing => _rpgCollection._homingRPG,

            _ => _rpgCollection._classicRPG,
        };
    }

    #endregion
}
