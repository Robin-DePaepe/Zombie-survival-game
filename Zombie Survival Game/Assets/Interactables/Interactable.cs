using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            if (Input.GetButtonDown("Interact"))
            {
                ItemInteracted(other);
            }
        }
    }

    protected virtual  void ItemInteracted(Collider player)
    {

    }
}
