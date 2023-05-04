using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedMeter : MonoBehaviour
{
    [SerializeField] SpeedMeterType _speedMeterType;

    [Space(5), Header("Simple SpeedMeter"), Space(3)]
    [SerializeField] GameObject _simpleSpeedMeterGO;
    [SerializeField] TextMeshProUGUI _simpleSpeedText;

    [Space(5), Header("Complex SpeedMeter"), Space(5)]
    [SerializeField] GameObject _complexSpeedMeterGO;
    [SerializeField] TextMeshProUGUI _complexSpeedTextXYZ;
    [SerializeField] TextMeshProUGUI _complexSpeedTextXZ;
    [SerializeField] TextMeshProUGUI _complexSpeedTextY;

    private Action<float> UpdateSpeedXYZ; 

    public enum SpeedMeterType
    {
        Simple,
        Complex,
        Hidden
    }

    private void OnDisable()
    {
        UnsusbsribeAll();
    }

    private void OnValidate()
    {
        ValidateSpeedmeterType();
    }

    private void Start()
    {
        ValidateSpeedmeterType();
    }

    private void ValidateSpeedmeterType()
    {
        UnsusbsribeAll();
        //print(UpdateSpeedXYZBasedOnMeterType.GetInvocationList()[0].ToString();

        switch (_speedMeterType)
        {
            case SpeedMeterType.Simple:
            default:
                //SIMPLE SPEED METER, DEACTIVATE BROKEN UP VECTOR AND ACTIVATE SIMPLE DISPLAY
                _simpleSpeedMeterGO.SetActive(true);
                _complexSpeedMeterGO.SetActive(false);

                //Asign functionaly to event
                UpdateSpeedXYZ = (float spdXYZ) =>
                {
                    _simpleSpeedText.text = spdXYZ.ToString("00.00");
                };

                //subscribe update methods
                EventManager.OnUpdatePlayerSpeedXYZ += UpdateSpeedXYZ;

                break;

            case SpeedMeterType.Complex:
                //SIMPLE SPEED METER, DEACTIVATE BROKEN UP VECTOR AND ACTIVATE SIMPLE DISPLAY
                _simpleSpeedMeterGO.SetActive(false);
                _complexSpeedMeterGO.SetActive(true);

                //Asign functionaly to event
                UpdateSpeedXYZ = (float spdXYZ) =>
                {
                    _complexSpeedTextXYZ.text = "XYZ: " + spdXYZ.ToString("00.00");
                };

                //subscribe update methods
                EventManager.OnUpdatePlayerSpeedXYZ += UpdateSpeedXYZ;
                EventManager.OnUpdatePlayerSpeedXZ += UpdateSpeedXZ;
                EventManager.OnUpdatePlayerSpeedY += UpdateSpeedY;

                break;

            case SpeedMeterType.Hidden:

                _simpleSpeedMeterGO.SetActive(false);
                _complexSpeedMeterGO.SetActive(false);

                break;
        }
    }

    private void UnsusbsribeAll()
    {
        EventManager.OnUpdatePlayerSpeedXYZ -= UpdateSpeedXYZ;
        EventManager.OnUpdatePlayerSpeedXZ -= UpdateSpeedXZ;
        EventManager.OnUpdatePlayerSpeedY -= UpdateSpeedY;
    }

    private void UpdateSpeedXZ(float spdXZ)
    {
        _complexSpeedTextXZ.text = "XZ: " + spdXZ.ToString("00.00");
    }

    private void UpdateSpeedY(float spdY)
    {
        _complexSpeedTextY.text = "Y: " + Mathf.Abs(spdY).ToString("00.00");
    }
}
