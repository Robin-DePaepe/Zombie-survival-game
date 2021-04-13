using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Health : MonoBehaviour
{
    //variables
    [SerializeField]
    private int m_maxArmour = 0;

    [SerializeField]
    private int m_CurrentArmour = 0;

    [SerializeField]
    private int m_StartHealth = 10;

    private int m_CurrentHealth;

    [SerializeField]
    private GameObject m_DroppedItem = null;

    [SerializeField]
    private float m_DropChance = 0.0f;

    [SerializeField] private AudioSource m_BreakArmourSound;
    [SerializeField] private AudioSource m_PlayerHitSound;
    [SerializeField] private AudioSource m_Player75PercentSound;
    [SerializeField] private AudioSource m_Player50PercentSound;

    private ScoreBoard m_ScoreBoard;
    private GameEnd m_GameEnd;
    //functions
    public void AddArmour(int amount)
    {
        m_CurrentArmour += amount;

        if (m_CurrentArmour > m_maxArmour)
        {
            m_CurrentArmour = m_maxArmour;
        }
    }

    public void HealPlayer()
    {
        m_CurrentHealth = m_StartHealth;
    }


    void Awake()
    {
        m_CurrentHealth = m_StartHealth;
        m_ScoreBoard = GameObject.Find("ScoreBoard").GetComponent<ScoreBoard>();
        m_GameEnd = GameObject.Find("EndGame").GetComponent<GameEnd>();
    }

    public void Damage(int amount)
    {
        //armour
        if (m_CurrentArmour != 0)
        {
            amount = DamageArmour(amount);
        }
        //sounds
        if (tag == "Friendly")
        {
            if (m_PlayerHitSound != null)
            {
                m_PlayerHitSound.Play();
            }

            if (m_CurrentHealth >= m_StartHealth * 0.75f && m_CurrentHealth - amount <= m_StartHealth * 0.75f && m_Player75PercentSound != null)
            {
                m_Player75PercentSound.Play();
            }

            if (m_CurrentHealth >= m_StartHealth * 0.5f && m_CurrentHealth - amount <= m_StartHealth * 0.5f && m_Player50PercentSound != null)
            {
                m_Player50PercentSound.Play();
            }
        }

        //health
        m_CurrentHealth -= amount;

        //check if player died
        if (m_CurrentHealth <= 0)
        {
            if (tag == "Enemy")
            {
                Ability.AddKill();
                m_ScoreBoard.AddKill();
            }
            if (tag == "Friendly")
            {
                m_GameEnd.EndGame();
            }
            Kill();
        }
    }

    int DamageArmour(int amount)
    {
        if (amount <= m_CurrentArmour)
        {
            m_CurrentArmour -= amount;
            amount = 0;
        }
        else
        {
            amount -= m_CurrentArmour;
            m_CurrentArmour = 0;

            if (m_BreakArmourSound)
            {
                m_BreakArmourSound.Play();
            }
        }
        return amount;
    }
    void Kill()
    {
        DropItem();
        Destroy(gameObject);
    }

    void DropItem()
    {
        if (m_DroppedItem == null)
            return;

        var dropChance = Random.Range(0.0f, 1.0f);

        if (dropChance < m_DropChance)
        {
            Instantiate(m_DroppedItem, transform.position, transform.rotation);
        }
    }
}
