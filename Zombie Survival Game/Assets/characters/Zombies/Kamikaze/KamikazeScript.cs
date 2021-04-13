using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeScript : MonoBehaviour
{
    private GameObject m_PlayerTarget = null;

    private MovementBehaviour m_MovementBehaviour;
    private bool m_HasAttacked = false;
    static private bool m_ProgramShutdown = false;

    static public bool ProgramEnd
    {
        set
        {
            m_ProgramShutdown = value;
        }
    }
    private void Start()
    {
        //expensive method, use with caution
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

        if (player) m_PlayerTarget = player.gameObject;

        m_MovementBehaviour = GetComponent<MovementBehaviour>();

        m_ProgramShutdown = false;
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (m_MovementBehaviour == null || m_PlayerTarget == null)
            return;

        m_MovementBehaviour.Target = m_PlayerTarget;
        m_MovementBehaviour.DesiredLookatPoint = m_PlayerTarget.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_HasAttacked) return;

        if (m_PlayerTarget == null) return;

        if (other.name == "Player")
        {
            Explode();
        }
    }

    private void Kill()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (!m_HasAttacked && !m_ProgramShutdown)
        {
            Explode();
        }
    }
    private void Explode()
    {
        Grenade explosion = this.GetComponent<Grenade>();

        if (explosion != null)
        {
            explosion.Explode();
        }
        m_HasAttacked = true;

        Kill();
    }
}
