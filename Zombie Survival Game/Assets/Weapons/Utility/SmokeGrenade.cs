using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    [SerializeField] private float m_Radius = 15f;
    [SerializeField] private float m_ExplosionTimer = 5f;
    [SerializeField] private float m_SmokeDuration = 10f;
    [SerializeField] private float m_Speed = 20f;

    [SerializeField] private GameObject m_ExplosionFVXTemplate;
    [SerializeField] private AudioSource m_ExplosionSound;

    private Collider[] m_Colliders;
    private float m_Timer;
    private Vector3 m_Directon;
    private bool m_Exploded = false;
    private GameObject m_Smoke;
    private ParticleSystem.ShapeModule m_SmokeShape;
    private void Awake()
    {
        m_Directon = transform.forward;
        Invoke("Explode", m_ExplosionTimer);
    }
    void Update()
    {
        if (m_Exploded)
        {
            m_Timer += Time.deltaTime;

            CheckZombiesInSmoke();
            IncreaseSmokeRadius();

            if (m_Timer > m_SmokeDuration)
            {
                EndSmoke();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!WallDetection())
        {
            transform.position += m_Directon * Time.deltaTime * m_Speed;
        }
        m_Speed *= 0.99f;
    }

    private void Explode()
    {
        if (m_ExplosionFVXTemplate)
        {
            m_Smoke = Instantiate(m_ExplosionFVXTemplate, transform.position, transform.rotation);
            m_SmokeShape = m_Smoke.GetComponent<ParticleSystem>().shape;
        }
        if (m_ExplosionSound)
        {
            m_ExplosionSound.Play();
        }
        m_Exploded = true;

        //reset  speed
        m_Speed = 0f;
    }

    static string[] RAYCAST_MASK = new string[] { "StaticLevel", "DynamicLevel", "Ground", "Enemy", "Friendly" };

    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, m_Directon);
        RaycastHit hitrecord;

        if (Physics.Raycast(collisionRay, out hitrecord, Time.deltaTime * m_Speed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            //by hitting a wall the speed reduces
            m_Speed *= 0.5f;

            //calculate new direction
            m_Directon = -2 * Vector3.Dot(hitrecord.normal, m_Directon) * (hitrecord.normal - m_Directon);
            m_Directon.Normalize();

            return true;
        }
        return false;
    }
    private void CheckZombiesInSmoke()
    {
        m_Colliders = Physics.OverlapSphere(this.transform.position, m_SmokeShape.radius);

        foreach (Collider collider in m_Colliders)
        {
            NavMeshMovementBehaviour zombieMovementBehaviour = collider.GetComponent<NavMeshMovementBehaviour>();

            if (collider.tag == "Enemy" && zombieMovementBehaviour != null)
            {
                zombieMovementBehaviour.SetInSmokeDuration(m_SmokeDuration - m_Timer);
            }
        }
    }

    private void EndSmoke()
    {
        Destroy(m_Smoke);
        Destroy(this.gameObject);
    }

    private void IncreaseSmokeRadius()
    {
        float increaseSpeed = 1.5f;

        if (m_SmokeShape.radius < m_Radius)
        {
            m_SmokeShape.radius += increaseSpeed * Time.deltaTime;
        }
        else
        {
            increaseSpeed += 1f;
        }
    }
}
