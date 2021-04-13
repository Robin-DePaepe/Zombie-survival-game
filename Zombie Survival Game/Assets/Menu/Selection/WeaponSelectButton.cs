using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelectButton : BasicSelection
{
    [SerializeField] private bool m_PrimaryStartWeapon;
    [SerializeField] private bool m_SecondaryStartWeapon;

    private Color m_GreenTint;
    protected override void Awake()
    {
        base.Awake();

        m_GreenTint = new Color(0.6f, 1f, 0.6f);

        if (m_PrimaryStartWeapon ) LeftMouseClicked();
        if (m_SecondaryStartWeapon) RightMouseClicked();
    }

    protected override void LeftMouseClicked()
    {
        SetAsPrimary();
    }

    protected override void RightMouseClicked()
    {
        //check if the weapon isn't already primary
        if (LayoutMenu.PrimaryWeapon != m_Prefab)
        {
            LayoutMenu.SecondaryWeapon = m_Prefab;
            LayoutMenu.SecondaryWeaponButton = this;
            m_Image.color = Color.cyan;
        }
    }

    public void SetAsPrimary()
    {
        //check if the weapon isn't already secondary
        if (LayoutMenu.SecondaryWeapon != m_Prefab)
        {
            LayoutMenu.PrimaryWeapon = m_Prefab;
            LayoutMenu.PrimaryWeaponButton = this;
            m_Image.color = m_GreenTint;
        }
    }
}


