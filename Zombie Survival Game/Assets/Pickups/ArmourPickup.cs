using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPickup : BasicPickup
{
    [SerializeField] private int m_AmountOfArmour;
    protected override void OnPickedup(Health health)
    {
        health.AddArmour(m_AmountOfArmour);
    }
}
