using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWallOnTimer : MonoBehaviour
{
    [SerializeField] private float m_Duration;
    [SerializeField] private GameObject m_TriggerCollider;

    private float m_Timer = 0f;
    private bool m_Active = false;
    private SpawnCollider m_SpawnCollider;

    private void Awake()
    {
        if (m_TriggerCollider != null)
        {
            m_SpawnCollider = m_TriggerCollider.GetComponent<SpawnCollider>();
        }
    }

    void Update()
    {
        if (m_SpawnCollider != null)
        {
            m_Active = m_SpawnCollider.Entered;
        }
        if (m_Active)
        {
            m_Timer += Time.deltaTime;

            if (m_Timer >= m_Duration)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
