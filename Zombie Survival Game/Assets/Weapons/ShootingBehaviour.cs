using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    //  variables
    [SerializeField]
    private GameObject m_PrimaryGunTemplate = null;
    [SerializeField]
    private GameObject m_SecondaryGunTemplate = null;

    [SerializeField]
    private GameObject m_PrimarySocket = null;
    [SerializeField]
    private GameObject m_SecondarySocket = null;

    private BasicGun m_PrimaryGun = null;
    private BasicGun m_SecondaryGun = null;

    [SerializeField] private bool m_PrimaryWeapon = true;

    //  functions
    void Awake()
    {
        if (m_PrimaryWeapon)
        {
            m_PrimaryGunTemplate = LayoutMenu.PrimaryWeapon;
        }
        else
        {
            m_PrimaryGunTemplate = LayoutMenu.SecondaryWeapon;
        }

        // spawn guns
        if (m_PrimaryGunTemplate != null && m_PrimarySocket != null)
        {
            var gunObject = Instantiate(m_PrimaryGunTemplate, m_PrimarySocket.transform, true);
            gunObject.transform.localPosition = gunObject.transform.position;
            gunObject.transform.localRotation = gunObject.transform.rotation;
            m_PrimaryGun = gunObject.GetComponent<BasicGun>();
            m_PrimaryGun.enabled = true;

            if (m_PrimaryGun.DoubleWeapon)
            {
                m_SecondaryGunTemplate = m_PrimaryGunTemplate;
            }
        }

        if (m_SecondaryGunTemplate != null && m_SecondarySocket != null)
        {
            var gunObject = Instantiate(m_SecondaryGunTemplate, m_SecondarySocket.transform, true);
            gunObject.transform.localPosition = gunObject.transform.position;
            gunObject.transform.localRotation = gunObject.transform.rotation;
            m_SecondaryGun = gunObject.GetComponent<BasicGun>();
            m_SecondaryGun.enabled = true;
        }

        //secondary weapon should be disabled at start
        if (m_PrimaryWeapon == false)
        {
            DisableWeapon();
        }
    }

    public void PrimaryFire()
    {
        if (m_PrimaryGun != null)
            m_PrimaryGun.Fire();
    }

    public void SecondaryFire()
    {
        if (m_SecondaryGun != null)
        {
            m_SecondaryGun.Fire();
        }
        else
        {
            m_PrimaryGun.RightMouse();
        }
    }

    public void Reload()
    {
        if (m_PrimaryGun != null)
            m_PrimaryGun.Reload();
        if (m_SecondaryGun != null)
            m_SecondaryGun.Reload();
    }

    public void EnableWeapon()
    {
        if (m_PrimaryGun != null) m_PrimaryGun.EnableGun();
        if (m_SecondaryGun != null) m_SecondaryGun.EnableGun();
    }

    public void DisableWeapon()
    {
        if (m_PrimaryGun != null) m_PrimaryGun.DisableGun();
        if (m_SecondaryGun != null) m_SecondaryGun.DisableGun();
    }

    public void RefillAmmo()
    {
        if (m_PrimaryGun != null) m_PrimaryGun.RefillAmmo();
        if (m_SecondaryGun != null) m_SecondaryGun.RefillAmmo();
    }

    public bool CheckRefillAmmoAttempt()
    {
        bool result = false;
        if (m_PrimaryGun != null) result = m_PrimaryGun.CheckRefillAttempt();
        if (m_SecondaryGun == null) return result;

        return (result || m_SecondaryGun.CheckRefillAttempt());
    }

    public int GetClipSize()
    {
        return m_PrimaryGun.ClipSize;
    }

    public int GetClipCount()
    {
        return m_PrimaryGun.ClipCount;
    }
}
