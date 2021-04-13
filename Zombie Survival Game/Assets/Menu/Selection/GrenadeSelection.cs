using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeSelection : BasicSelection
{
    [SerializeField] private GameObject m_GrenadeVisualPrefab;
    [SerializeField] private bool m_StartGrenade;

    protected override void Awake()
    {
        base.Awake();

        if (m_StartGrenade) LeftMouseClicked();
    }

    protected override void LeftMouseClicked()
    {
        LayoutMenu.GrenadeSlot = m_Prefab;
        LayoutMenu.GrenadeVisual = m_GrenadeVisualPrefab;
        LayoutMenu.GrenadeButton = this;

        m_Image.color = new Color(0.9f, 0.6f, 0f);
    }
}
