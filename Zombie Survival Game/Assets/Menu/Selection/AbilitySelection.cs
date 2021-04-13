using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySelection : BasicSelection
{
    [SerializeField] private bool m_StartAbility;
    [SerializeField] private int m_Index;

    protected override void Awake()
    {
        base.Awake();

        if (m_StartAbility) LeftMouseClicked();
    }

    protected override void LeftMouseClicked()
    {
        LayoutMenu.AbilityIndex = m_Index;
        LayoutMenu.AbilityButton = this;

        m_Image.color = new Color(0.9f, 0.6f, 0f);
    }
}
