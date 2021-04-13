using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ability : MonoBehaviour
{
    [SerializeField] private int m_KillsToUnlock;
    [SerializeField] protected float m_AbilityDuration;

    static private int m_KillCount;
    private bool m_AbilityAvailable, m_AbilityActive;

    [SerializeField] private AudioSource m_AblitiyAvailableSound = null;

    private void Awake()
    {
        m_KillCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Ability") && m_AbilityAvailable)
        {
            ActivateAbility();
        }
        CheckAbilityReady();
    }
    static public void AddKill()
    {
            ++m_KillCount;        
    }
    void CheckAbilityReady()
    {
        if (m_KillCount == m_KillsToUnlock && m_AbilityActive == false)
        {
            m_AbilityAvailable = true;
            Debug.Log("ability active");

            if (m_AblitiyAvailableSound != null)
            {
                m_AblitiyAvailableSound.Play();
            }
        }
    }
    protected virtual void ActivateAbility()
    {
            m_AbilityAvailable = false;
            m_AbilityActive = true;
            Invoke("DisableAbility", m_AbilityDuration);
            Debug.Log("ability on");
    }
    protected virtual void DisableAbility()
    {
        m_AbilityActive = false;
        m_KillCount = 0;
    }
}
