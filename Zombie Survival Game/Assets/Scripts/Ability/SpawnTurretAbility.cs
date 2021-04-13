using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTurretAbility : Ability
{
    [SerializeField] private GameObject m_Turret = null;

    protected override void ActivateAbility()
    {
        float distanceFromPlayer = 5f;

        Vector3 spawnPosition = transform.position;
        spawnPosition += distanceFromPlayer * transform.forward;

        if (m_Turret != null && IsAbleToPlaceDown(spawnPosition))
        {
            base.ActivateAbility();
            GameObject turret = Instantiate(m_Turret, spawnPosition, m_Turret.transform.rotation);
            turret.GetComponent<TurretScript>().LifeTime = m_AbilityDuration;
        }
    }
    protected override void DisableAbility()
    {
        base.DisableAbility();
    }
    private bool IsAbleToPlaceDown(Vector3 spawnPoint)
    {
        BoxCollider collider = m_Turret.GetComponent<BoxCollider>();
        Ray collisionRay = new Ray(transform.position, transform.forward);

        float distance = (transform.position - spawnPoint).magnitude;

        distance += collider.size.x; //add width size so it doesn't get stuck halfway into a wall

        if (Physics.Raycast(collisionRay,distance ))
        {
            return false;
        }
            return true;
    }
}

