using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{
    [SerializeField] private GameObject m_Socket;
    [SerializeField] private int m_AmountOfGrenades = 3;
    private GameObject m_Grenade;
    private GameObject m_GrenadeVisualTemplate;
    private GameObject m_GrenadeVisual;
    private UI m_UIScript;
    private int m_CurrentAmountOfGrenades;

    public GameObject GrenadeVisual
    {
        get { return m_GrenadeVisualTemplate; }
    }
    private void Awake()
    {
        m_Grenade = LayoutMenu.GrenadeSlot;
        m_GrenadeVisualTemplate = LayoutMenu.GrenadeVisual;
        m_CurrentAmountOfGrenades = m_AmountOfGrenades;

        m_GrenadeVisual = Instantiate(m_GrenadeVisualTemplate, m_Socket.transform.position, m_Socket.transform.rotation);
        //make sure it follows the player
        m_GrenadeVisual.transform.parent = gameObject.transform;

        //UI
        GameObject uiObject = GameObject.Find("UI");

        if (uiObject != null)
        {
            m_UIScript = uiObject.GetComponent<UI>();
        }
    }

    public void Throw()
    {
        if (m_Grenade != null)
        {
            Instantiate(m_Grenade, m_Socket.transform.position, m_Socket.transform.rotation);

            --m_CurrentAmountOfGrenades;
        }
    }

    public int AmountOfGrenades
    {
        get { return m_CurrentAmountOfGrenades; }
    }

    public void RefillNades()
    {
        m_CurrentAmountOfGrenades = m_AmountOfGrenades;

        if (m_UIScript.SetUtilityCount == 1)
        {
            m_UIScript.SetUtilityCount = m_AmountOfGrenades;
        }
    }
    public void DisableVisual()
    {
        if (m_GrenadeVisual != null)
        {
            m_GrenadeVisual.SetActive(false);
        }
    }

    public void EnableVisual()
    {
        if (m_GrenadeVisual != null)
        {
            m_GrenadeVisual.SetActive(true);
        }
    }
}
