using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tokens : MonoBehaviour
{
    static private int m_Tokens = 3;

    static public int tokens
    {
        get { return m_Tokens; }
    }
    static public void ChangeTokens(int amount)
    {
        m_Tokens += amount;
    }
}
