using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutMenu : MonoBehaviour
{
    [SerializeField] Text m_TokenCount = null;

    static GameObject m_PrimaryWeapon;
    static GameObject m_SecondaryWeapon;
    static GameObject m_GrenadeSlot;
    static GameObject m_GrenadeVisual;
    static int m_AbilityIndex;

    static WeaponSelectButton m_PrimaryWeaponButtonRef;
    static WeaponSelectButton m_SecondaryWeaponButtonRef;
    static GrenadeSelection m_GrenadeButtonRef;
    static AbilitySelection m_AbilityButtonRef;

    // Update is called once per frame
    void Update()
    {
        m_TokenCount.text = Tokens.tokens.ToString();
    }

    static public int AbilityIndex
    {
        set
        {
            if (m_AbilityButtonRef != null) m_AbilityButtonRef.ResetColor();
            m_AbilityIndex = value;
        }
        get { return m_AbilityIndex; }
    }

    static public GameObject GrenadeVisual
    {
        get { return m_GrenadeVisual; }
        set { m_GrenadeVisual = value; }
    }
    static public GameObject GrenadeSlot
    {
        set
        {
            if (m_GrenadeButtonRef != null) m_GrenadeButtonRef.ResetColor();
            m_GrenadeSlot = value;
        }
        get { return m_GrenadeSlot; }
    }
    static public GameObject PrimaryWeapon
    {
        set
        {
            if (m_PrimaryWeaponButtonRef != null) m_PrimaryWeaponButtonRef.ResetColor();
            m_PrimaryWeapon = value;
        }
        get { return m_PrimaryWeapon; }
    }

    static public GameObject SecondaryWeapon
    {
        set
        {
            if (m_SecondaryWeaponButtonRef != null) m_SecondaryWeaponButtonRef.ResetColor();
            m_SecondaryWeapon = value;
        }
        get { return m_SecondaryWeapon; }
    }

    static public WeaponSelectButton PrimaryWeaponButton
    {
        set { m_PrimaryWeaponButtonRef = value; }
    }

    static public WeaponSelectButton SecondaryWeaponButton
    {
        set { m_SecondaryWeaponButtonRef = value; }
    }

    static public GrenadeSelection GrenadeButton
    {
        set { m_GrenadeButtonRef = value; }
    }
    static public AbilitySelection AbilityButton
    {
        set { m_AbilityButtonRef = value; }
    }
}
