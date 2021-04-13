using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAmo : Interactable
{
    protected override void ItemInteracted(Collider player)
    {
        player.GetComponent<PlayerCharacter>().RefillAmmo();
    }
}
