using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSyringe : Interactable
{
    protected override void ItemInteracted(Collider player)
    {
        player.GetComponent<HealthShot>().AddSyringe();

        Destroy(gameObject);
    }
}
