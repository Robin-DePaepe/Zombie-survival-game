using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsLockedTracker : MonoBehaviour
{
    [SerializeField] static private List<GameObject> m_Items = new List<GameObject>();
    static private List<bool> m_ItemsLockedStatus = new List<bool>();

    static public void AddItem(GameObject item, bool status)
    {
        if (m_Items.IndexOf(item) == -1)
        {
            m_Items.Add(item);
            m_ItemsLockedStatus.Add(status);
        }
    }
    static public bool GetLockedStatus(GameObject Item)
    {
        int index = m_Items.IndexOf(Item);

        if (index == -1)
        {
            return true; //lock the item if it isn't found 
        }
        return m_ItemsLockedStatus[index];
    }

    static public void UnlockItem(GameObject item)
    {
        int index = m_Items.IndexOf(item);

        if (index != -1)
        {
            m_ItemsLockedStatus[index] = false;
        }
    }
}
