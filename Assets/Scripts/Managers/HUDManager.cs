using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    #region Fields
    [Space(5), Header("Health"), Space(3)]
    [SerializeField] Health c_healthInfo;
    [SerializeField] GameObject g_healthUI;
    [SerializeField] TextMeshProUGUI c_text;
    [SerializeField] Image c_healthBar;
    [SerializeField] float _hideUITimer;
    #endregion

    #region Variables

    //Health
    private float hideHealthTimer;
    private float currentHealth;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        //Heath
        hideHealthTimer = _hideUITimer;
    }

    private void Update()
    {
        //Health
        if (hideHealthTimer <= 0)
        {
            ShowHideHealthBar(false);
        }
        else
        {
            hideHealthTimer -= Time.deltaTime;
        }
        UpdateHealthBar(c_healthInfo.GetHealth(),c_healthInfo.GetMaxHealth());
    }

    #endregion

    #region Methods
    //Health
    public void UpdateHealthBar(float value, float max)
    {
        c_healthBar.fillAmount = value / max;
        c_text.text = value + " / " + max;
        if(currentHealth != value)
        {
            ShowHideHealthBar(true);
        }
        currentHealth = value;
    }
    public void ShowHideHealthBar(bool isVisisble)
    {
        g_healthUI.SetActive(isVisisble);
        if (isVisisble) hideHealthTimer = _hideUITimer;
    }

    #endregion
}
