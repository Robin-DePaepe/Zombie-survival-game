using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthShot : MonoBehaviour
{
    [SerializeField] private int m_AmountOfShots;
    [SerializeField] private GameObject m_SyringeTemplate;
    [SerializeField] private GameObject m_Socket;

    private GameObject m_Syringe;
    private UI m_UIScript;
    private Health m_Health;

    private void Awake()
    {
        m_Health = GetComponent<Health>();

        m_Syringe = Instantiate(m_SyringeTemplate, m_Socket.transform.position, m_Socket.transform.rotation);
        //make sure it follows the player
        m_Syringe.transform.parent = gameObject.transform;

        //UI
        GameObject uiObject = GameObject.Find("UI");

        if (uiObject != null)
        {
            m_UIScript = uiObject.GetComponent<UI>();
        }
    }
    public void UseSyringe()
    {
        if (m_Health != null && m_AmountOfShots > 0)
        {
            m_Health.HealPlayer();

            --m_AmountOfShots;
        }
    }
    public void AddSyringe()
    {
        ++m_AmountOfShots;

        if (m_UIScript.SetHealthShotCount == 1)
        {
            m_UIScript.SetHealthShotCount = m_AmountOfShots;
        }
    }
    public int AmountOfShots
    {
        get { return m_AmountOfShots; }
    }

    public void DisableVisual()
    {
        if (m_Syringe != null)
        {
            m_Syringe.SetActive(false);
        }
    }

    public void EnableVisual()
    {
        if (m_Syringe != null)
        {
            m_Syringe.SetActive(true);
        }
    }
}
