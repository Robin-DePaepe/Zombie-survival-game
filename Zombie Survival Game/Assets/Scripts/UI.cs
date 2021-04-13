using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text m_HealthshotCount;
    [SerializeField] private Text m_UtilityCount;
    [SerializeField] private Text m_GunInfo;
    [SerializeField] private Text m_AmmoSizes;

    private int m_ClipSize;
    private int m_ClipCount;
    public int SetHealthShotCount
    {
        set { m_HealthshotCount.text = value.ToString(); }
        get //acts like a boolean to check if its set
        {
            if (m_HealthshotCount.text == "") return 0; 
            else return 1;
        }
    }

    public int SetUtilityCount
    {
        set { m_UtilityCount.text = value.ToString(); }
        get//acts like a boolean to check if its set
        {
            if (m_UtilityCount.text == "") return 0;
            else return 1;
        }
    }

    public int SetClipSize
    {
        set { m_ClipSize = value; }
    }

    public int SetClipCount
    {
        set { m_ClipCount = value; }
    }

    public void ActivateAmmoInfo()
    {
        m_GunInfo.enabled = true;
        m_AmmoSizes.text = m_ClipSize.ToString() + " / " + m_ClipCount.ToString();
    }
    public void ResetText()
    {
        m_UtilityCount.text = "";
        m_HealthshotCount.text = "";
        m_AmmoSizes.text = "";
        m_GunInfo.enabled = false;
    }
}
