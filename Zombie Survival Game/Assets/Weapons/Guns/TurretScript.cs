using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    [SerializeField] private float m_Radius = 75f;
    [SerializeField] private float m_RotationSpeed = 15f;
    [SerializeField] private int m_BulletCount = 500;
    [SerializeField] private GameObject m_BulletTemplate = null;
    [SerializeField] private GameObject m_FireSocket = null;
    [SerializeField] private float m_FireRate = 25.0f;
    [SerializeField] private GameObject m_Gun;

    private float m_Timer;
    private Collider[] m_Colliders;
    private bool m_TargetInSight;
    private float m_FireTimer = 0.0f;
    private Vector3 m_TargetPosition;
    private Transform m_GunTransform;
     private float m_MaxLifeTime = 500f;


    public float LifeTime
    {
        set { m_MaxLifeTime = value; }
    }
    private void Awake()
    {     
        m_GunTransform = m_Gun.transform;
    }
    private void Update()
    {
        m_Timer += Time.deltaTime;

        FindTarget();

        //check if the object should be destroyed
        if (m_Timer >= m_MaxLifeTime || m_BulletCount == 0)
        {
            Destroy(this.gameObject);
        }

        //update firetimer
        if (m_FireTimer > 0.0f)
        {
            m_FireTimer -= Time.deltaTime;
        }

        //check to fire a bullet
        if (m_FireTimer <= 0.0f && m_TargetInSight)
        {
            FireBullet();
        }
        //rotate turret
        if (m_TargetInSight)
        {
            UpdateRotation();
        }
    }

    private void FireBullet()
    {
        //check if the gun is allowed to fire
        if (m_BulletCount > 0 && m_FireSocket != null && m_BulletTemplate != null)
        {
            --m_BulletCount;

            Instantiate(m_BulletTemplate, m_FireSocket.transform.position, m_FireSocket.transform.rotation);

            //respect fireRate
            m_FireTimer += 1.0f / m_FireRate;

        }
    }
    private void FindTarget()
    {
        m_Colliders = Physics.OverlapSphere(this.transform.position, m_Radius);
        Collider closestEnemy = null;
        float shortestDistance = m_Radius;

        foreach (Collider collider in m_Colliders)
        {
            if (collider.tag == "Enemy" ) 
            {
                float distance = (collider.bounds.ClosestPoint(this.transform.position) - this.transform.position).magnitude;

                if (distance < shortestDistance)
                {
                    closestEnemy = collider;
                }
            }
        }
        ShootAtTarget(closestEnemy);
    }
    private void ShootAtTarget(Collider target)
    {
        if (target != null)
        {
            m_TargetInSight = true;
            m_TargetPosition = target.transform.position;
            m_TargetPosition.y += target.bounds.size.y / 2f; //increase the pivot with height the size of the object
        }
        else
        {
            m_TargetInSight = false;
        }
    }
    private void UpdateRotation()
    {
        var rotation = Quaternion.LookRotation(m_GunTransform.position - m_TargetPosition);
        m_GunTransform.rotation = Quaternion.RotateTowards(m_GunTransform.rotation, rotation, m_RotationSpeed * Time.deltaTime);
    }
}
