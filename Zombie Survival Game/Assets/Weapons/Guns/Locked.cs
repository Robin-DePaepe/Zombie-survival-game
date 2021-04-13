using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : MonoBehaviour
{
    [SerializeField] private bool m_Locked;
    [SerializeField] private int m_UnlockPrize;

    public bool IsLocked
    {
        get { return m_Locked; }
    }

   public int UnlockPrize
    {
        get { return m_UnlockPrize; }
    }
}
