using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularZombie : MonoBehaviour
{
    private GameObject m_PlayerTarget = null;
    private MeleeAtack m_MeleeAtackScript;
    private MovementBehaviour m_MovementBehaviour;
    NavMeshMovementBehaviour m_NavMesh;

    private void Start()
    {
        //expensive method, use with caution
        PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

        if (player) m_PlayerTarget = player.gameObject;

        m_NavMesh = GetComponent<NavMeshMovementBehaviour>();

        m_MeleeAtackScript = GetComponent<MeleeAtack>();

        if (m_MeleeAtackScript != null)
        {
            m_MeleeAtackScript.PlayerHealth = m_PlayerTarget.GetComponent<Health>();
        }

        m_MovementBehaviour = GetComponent<MovementBehaviour>();
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (m_MovementBehaviour == null)
            return;

        if (m_PlayerTarget == null) return;

        m_MovementBehaviour.Target = m_PlayerTarget;
        m_MovementBehaviour.DesiredLookatPoint = m_PlayerTarget.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")  m_NavMesh.InRange = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")   m_NavMesh.InRange = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (m_PlayerTarget == null) return;

        if (m_MeleeAtackScript == null) return;

        if (other.name == "Player")
        {
            m_MeleeAtackScript.Melee();
        }
    }
}
