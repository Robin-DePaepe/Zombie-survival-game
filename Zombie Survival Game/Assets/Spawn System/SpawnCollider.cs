using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    private bool m_Entered = false;

    public bool Entered
    {
        get { return m_Entered; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            m_Entered = true;
            Invoke("Kill", 5f); //after a couple seconds to make sure the spawnscripts have read the boolean change
        }
    }

    private void Kill()
    {
        Destroy(this.gameObject);
    }
}
