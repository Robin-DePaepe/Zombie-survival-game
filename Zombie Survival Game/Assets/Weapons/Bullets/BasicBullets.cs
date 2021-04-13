using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullets : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30.0f;
    [SerializeField]
    private float _lifeTime = 10.0f;

    [SerializeField]
    private int _damage = 5;

    private void Awake()
    {
        Invoke("Kill", _lifeTime);
    }

    void FixedUpdate()
    {
        if (!WallDetection())
            transform.position += transform.forward * Time.deltaTime * _speed;
    }

    static string[] RAYCAST_MASK = new string[] { "StaticLevel", "DynamicLevel" };
    bool WallDetection()
    {
        Ray collisionRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(collisionRay,
            Time.deltaTime * _speed, LayerMask.GetMask(RAYCAST_MASK)))
        {
            Kill();
            return true;
        }

        return false;
    }

    void Kill()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        //make sure we only hit friendly or enemies
        if (other.tag != "Friendly" && other.tag != "Enemy")
            return;

        //only hit the opposing team
        if (other.tag == tag)
            return;

        Health otherHealth = other.GetComponent<Health>();

        if (otherHealth != null)
        {
            otherHealth.Damage(_damage);
            Kill();
        }
    }
}
