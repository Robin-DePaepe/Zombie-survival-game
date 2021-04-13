using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAbility : Ability
{
    [SerializeField] private float m_Range;
    [SerializeField] private GameObject m_ExplosionFVXTemplate;

    private Collider[] m_Colliders;

    protected override void ActivateAbility()
    {
        base.ActivateAbility();

        m_Colliders = Physics.OverlapSphere(transform.position, m_Range);

        foreach (Collider collider in m_Colliders)
        {
            if (collider.tag == "Enemy")
            {
                Health health = collider.GetComponent<Health>();
                if (health != null)
                {
                    health.Damage(100000); //high number to make sure it kills all zombies in range
                }
            }
        }

        if (m_ExplosionFVXTemplate)
        {
            Instantiate(m_ExplosionFVXTemplate, transform.position, transform.rotation);
        }
    }
    protected override void DisableAbility()
    {
        base.DisableAbility();
    }
}
