using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    [SerializeField] private GameObject m_ZombieType;
    [SerializeField] private GameObject m_TriggerColider;

    [SerializeField] private int m_Amount;
    [SerializeField] private float m_SpawnDelay;
    [SerializeField] private int m_Waves = 1;
    [SerializeField] private float m_WaveDelay;

    private SpawnCollider m_SpawnCollider;
    private float m_Timer = 0f;
    private int m_ZombiesLeftInWave;
    private bool m_Active = false;
    private void Awake()
    {
        if (m_TriggerColider != null)
        {
            m_SpawnCollider = m_TriggerColider.GetComponent<SpawnCollider>();
        }
        m_ZombiesLeftInWave = m_Amount;
    }
    void Update()
    {
        if (m_SpawnCollider != null)
        {
            m_Active = m_SpawnCollider.Entered;
        }
        m_Timer += Time.deltaTime;

        if (m_Active)
        {
            HandleSpawning();
        }
    }
    private void SpawnZombie()
    {
        if (m_ZombieType == null) return;
        Instantiate(m_ZombieType, transform.position, Quaternion.identity);
    }

    void HandleSpawning()
    {
        if (m_Timer >= m_SpawnDelay && m_ZombiesLeftInWave > 0)
        {
            SpawnZombie();
            m_Timer = 0f;
            --m_ZombiesLeftInWave;

            if (m_ZombiesLeftInWave == 0)
            {
                Invoke("NewWave", m_WaveDelay);
            }
        }
    }

    private void NewWave()
    {
        m_ZombiesLeftInWave = m_Amount;
        --m_Waves;

        if (m_Waves == 0)
        {
            Kill();
        }
    }
    private void Kill()
    {
        Destroy(this.gameObject);
    }
}
