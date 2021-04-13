using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtack : MonoBehaviour
{
    [SerializeField] private float m_AttackRange = 3f;
    [SerializeField] private int m_AttackDamage = 3;
    [SerializeField] private float m_AttackDelay = 3f;
    [SerializeField] private bool m_PlayerMelee = false;

    private bool m_Attack = false;
    private float m_Timer = 0f;
    private Collider[] m_Colliders;
    private Health m_PlayerHealth;

    private void Update()
    {
        m_Timer += Time.deltaTime;

        if (m_Attack && m_Timer >= m_AttackDelay)
        {
            Attack();
            m_Timer = 0f;
        }
        m_Attack = false;
    }

    public void Melee()
    {
        m_Attack = true;
    }

    private void Attack()
    {
        //the player has to aim towards the zombie
        if (m_PlayerMelee)
        {
            Ray collisionRay = new Ray(transform.position, transform.forward);
            RaycastHit hitrecord;

            if (Physics.Raycast(collisionRay, out hitrecord, m_AttackRange, LayerMask.GetMask("Enemy")))
            {
                Health health = hitrecord.collider.GetComponent<Health>();

                if (health != null)
                {
                    health.Damage(m_AttackDamage);
                }
            }
        }
        else
        {
            //zombies know they are in range if the attack is called
            if (m_PlayerHealth != null)
            {
                m_PlayerHealth.Damage(m_AttackDamage);
            }
        }
    }

    public Health PlayerHealth
    {
        set { m_PlayerHealth = value; }
    }
}
