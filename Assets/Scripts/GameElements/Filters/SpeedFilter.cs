using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedFilter : MonoBehaviour
{

    #region Fields
    [Space(5), Header("Properties"), Space(3)]
    [SerializeField] private int _speedLimit;
    [SerializeField] private Vector2 _size;

    [Space(5), Header("Visuals"), Space(3)]
    [SerializeField] private Material _openMat;
    [SerializeField] private Material _closedMat;

    [Space(5), Header("Components"), Space(3)]
    [SerializeField] private Collider _playerBarrier;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Collider _rocketBarrier;
    [SerializeField] private BoxCollider _trigger;
    [SerializeField] private GameObject _speedLimitDisplay;
    #endregion

    private void OnValidate()
    {
        _playerBarrier.transform.localScale = _rocketBarrier.transform.localScale = _renderer.transform.localScale = new Vector3(_size.x, _size.y, 1f);
        _trigger.size = new Vector3(_size.x, _size.y, 0.25f);
        _playerBarrier.transform.localPosition = _rocketBarrier.transform.localPosition = _renderer.transform.localPosition = _trigger.center = Vector3.up * _size.y / 2;

        foreach (TextMeshPro text in _speedLimitDisplay.GetComponentsInChildren<TextMeshPro>())
        {
            text.transform.localPosition = (Vector3.up * _size.y / 2) + text.transform.forward * -0.2f;

            text.transform.localScale = Vector3.one * Mathf.Min(_size.x, _size.y);

            text.text = _speedLimit.ToString();
        }
    }


    private void OnEnable() => EventManager.OnUpdatePlayerSpeedXYZ += CheckForPlayerSpeed;

    private void OnDisable() => EventManager.OnUpdatePlayerSpeedXYZ -= CheckForPlayerSpeed;

    private void OnTriggerEnter(Collider other) => EventManager.OnUpdatePlayerSpeedXYZ -= CheckForPlayerSpeed;

    private void OnTriggerExit(Collider other) => EventManager.OnUpdatePlayerSpeedXYZ += CheckForPlayerSpeed;


    private void CheckForPlayerSpeed(float spd)
    {
        bool canPass = Mathf.RoundToInt(spd) >= _speedLimit;

        _playerBarrier.gameObject.SetActive(!canPass);
        _rocketBarrier.gameObject.SetActive(canPass);
        _renderer.material = canPass ? _openMat : _closedMat;
    }
}
