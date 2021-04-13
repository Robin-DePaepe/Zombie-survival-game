using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Grenade : MonoBehaviour
{
    [SerializeField] private float m_Radius = 15f;
    [SerializeField] private float m_ExplosionTimer = 5f;
    [SerializeField] private int m_Damage = 50;
    [SerializeField] private float m_Force = 1000f;
    [SerializeField] private float m_Speed = 10f;
    [SerializeField] private bool m_ExplodeOnImpact = false;

    [SerializeField] private GameObject m_ExplosionFVXTemplate;
    [SerializeField] private AudioSource m_ExplosionSound;

    private Collider[] m_Colliders;
    private Vector3 m_Directon;
    private string m_Tag;
    private float m_TeamDamageDropOff = 0.25f;
    private void Awake()
    {
        m_Directon = transform.forward;
        m_Tag = gameObject.tag;
        Invoke("Explode", m_ExplosionTimer);
    }


    private void FixedUpdate()
    {
        if (!WallDetection())
        {
            transform.position += m_Directon * Time.deltaTime * m_Speed;
        }
        m_Speed *= 0.99f;
    }

    public void Explode()
    {
        m_Colliders = Physics.OverlapSphere(this.transform.position, m_Radius);

        foreach (Collider collider in m_Colliders)
        {
            if (!collider.isTrigger)
            {
                Vector3 direction = collider.transform.position - this.transform.position;
                direction.Normalize();

                Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
                Health health = collider.GetComponent<Health>();

                float distance = (collider.bounds.ClosestPoint(this.transform.position) - this.transform.position).magnitude;

                float rangeDrop = m_Radius - distance;
                float rangeDropRatio = rangeDrop / m_Radius;

                if (rigidbody != null)
                {
                    rigidbody.AddForce(direction * m_Force * rangeDropRatio);
                }
                if (health != null)
                {
                    if (collider.tag == m_Tag)//the rocket does less damage on the objects with the same tag
                    {
                        health.Damage((int)(m_Damage * rangeDropRatio * m_TeamDamageDropOff));
                    }
                    else
                    {
                        health.Damage((int)(m_Damage * rangeDropRatio));
                    }
                }
            }
        }
        if (m_ExplosionFVXTemplate)
        {
            Instantiate(m_ExplosionFVXTemplate, transform.position, transform.rotation);
        }
        if (m_ExplosionSound)
        {
            m_ExplosionSound.Play();
        }

        Destroy(this.gameObject);
    }

    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, m_Directon);
        RaycastHit hitrecord;

        if (Physics.Raycast(collisionRay, out hitrecord, Time.deltaTime * m_Speed))
        {
            if (m_ExplodeOnImpact)
            {
                Explode();
            }
            m_Speed *= 0.5f;
     
            //calculate new direction
            m_Directon = -2 * Vector3.Dot(hitrecord.normal, m_Directon) * (hitrecord.normal - m_Directon);
            m_Directon.Normalize();

            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false && m_ExplodeOnImpact)
        {
            Explode();
        }
    }
}
