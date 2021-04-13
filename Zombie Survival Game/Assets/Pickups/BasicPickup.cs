using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPickup : MonoBehaviour
{
    [SerializeField] private float m_LifeTime = 10.0f;
    [SerializeField] private bool m_LimitedLifeTime;

    void Awake()
    {
        if (m_LimitedLifeTime)
        {
            Invoke("Kill", m_LifeTime);
        }
    }

    protected virtual void OnPickedup(Health health)
    { }

    void OnTriggerEnter(Collider other)
    {
        //only a player can pick these up, so first check for friendly
        if (other.tag != "Friendly")
            return;

        Health health = other.GetComponent<Health>();

        OnPickedup(health);

        Kill();
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
