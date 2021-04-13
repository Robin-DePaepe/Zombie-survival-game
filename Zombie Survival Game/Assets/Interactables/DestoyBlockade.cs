using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyBlockade : Interactable
{
    [SerializeField] private GameObject m_Blockade;
    protected override void ItemInteracted(Collider player)
    {
        if (m_Blockade != null)
            Destroy(m_Blockade);
    }
}
