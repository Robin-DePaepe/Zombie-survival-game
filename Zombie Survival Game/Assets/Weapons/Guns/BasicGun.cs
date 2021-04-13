using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicGun : MonoBehaviour
{
    #region initialization

    //regular ammo
    [SerializeField]
    private GameObject m_BulletTemplate = null;

    [SerializeField]
    private int m_ClipSize = 50;

    [SerializeField]
    private int m_TotalAmountOfClips = 10;

    [SerializeField]
    private float m_ReloadTime = 2f;

    [SerializeField]
    private float m_FireRate = 25.0f;

    [SerializeField]
    private List<Transform> m_FireSockets = new List<Transform>();

    [SerializeField] private AudioSource m_FireSound;

    [SerializeField] private AudioSource m_OutOfAmmoSound;

    //grenade launcher ammo
    [SerializeField]
    private GameObject m_GrenadeBulletTemplate = null;

    [SerializeField]
    private float m_LauncherReloadTime = 5f;

    [SerializeField]
    private Transform m_LauncherSocket = null;

    [SerializeField]
    private int m_TotalAmountOfGrenades = 3;

    [SerializeField] private AudioSource m_LauncherSound;

    //sniper scope
    [SerializeField] private bool m_Sniper = false;

    //tapfire
    [SerializeField] private bool m_TapFire = false;

    //left and right handed
    [SerializeField] private bool m_DoubleWeapon = false;

    //privates not serialized

    //material color related
    private Color m_ReloadingColor = Color.red;
    private Color m_LauncherReloadingColor = Color.magenta;
    private Color m_StartColor;
    private Material m_AttachedMaterial;

    private bool m_TriggerPulled = false;
    private bool m_RightMousePulled = false;
    private bool m_ReloadFailed = false;

    private int m_CurrentAmmo = 50;
    private int m_AmmoInClips;
    private int m_CurrentLaucherAmmo = 1;
    private int m_LaucherAmmoInStock;

    private float m_FireTimer = 0.0f;

    private Camera m_Camera;
    private PlayerCharacter m_PlayerCharacter;
    private GameObject m_SniperScope;
    private bool m_IsReloading = false;
    // private bool m_IsSelected= false;
    #endregion initialization

    public int ClipSize
    {
        get { return m_ClipSize; }
    }

    public int ClipCount
    {
        get { return m_TotalAmountOfClips; }
    }
    public int CurrentAmmo
    {
        get
        {
            return m_CurrentAmmo;
        }
    }
    public bool DoubleWeapon
    {
        get { return m_DoubleWeapon; }
    }
    private void Awake()
    {
        m_CurrentAmmo = m_ClipSize;
        m_AmmoInClips = (m_TotalAmountOfClips - 1) * m_ClipSize; // - 1 caus of the starting clip
        m_LaucherAmmoInStock = m_TotalAmountOfGrenades - 1;

        m_Camera = transform.parent.transform.parent.GetComponentInChildren<Camera>();
        m_PlayerCharacter = transform.parent.transform.parent.GetComponentInChildren<PlayerCharacter>();

        if (m_Sniper)
        {
            m_SniperScope = GameObject.Find("scope");
            if (m_SniperScope)
            {
                m_SniperScope.SetActive(false);
            }
        }

        //get the materialcolor and material
        Renderer renderer = GetComponentInChildren<Renderer>();

        if (renderer)
        {
            m_AttachedMaterial = renderer.material;

            if (m_AttachedMaterial) m_StartColor = m_AttachedMaterial.GetColor("_Color");
        }
    }

    private void Update()
    {
        //handle the countdown of the fire timer
        if (m_FireTimer > 0.0f)
            m_FireTimer -= Time.deltaTime;

        if (m_FireTimer <= 0.0f && m_TriggerPulled && !m_IsReloading)
            FireProjectile();

        if (m_RightMousePulled)
        {
            FireGrenade();
            if (m_Sniper)
            {
                ScopeIn();
            }
        }
        else
        {
            ScopeOut();
        }
        //the trigger will release by itself, 
        //if we still are firing, we will receive new fire input
        m_TriggerPulled = false;
        m_RightMousePulled = false;
    }

    private void FireProjectile()
    {
        //no ammo, we can't fire
        if (m_CurrentAmmo <= 0)
            return;

        //no bullet to fire
        if (m_BulletTemplate == null)
            return;

        //consume a bullet
        --m_CurrentAmmo;

        //fire bullet
        for (int i = 0; i < m_FireSockets.Count; i++)
        {
            if (m_SniperScope == null || m_SniperScope.activeSelf == false)
            {
                Instantiate(m_BulletTemplate, m_FireSockets[i].position, m_FireSockets[i].rotation);
            }
            else
            {
                Vector3 lookAtPosition = m_Camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
                Instantiate(m_BulletTemplate, lookAtPosition, m_Camera.transform.rotation);
            }
        }

        //call reload if its empty 
        if (m_CurrentAmmo == 0)
        {
            Reload();
        }
        //set the time so we respect the firerate
        m_FireTimer += 1.0f / m_FireRate;

        if (m_FireSound)
        {
            m_FireSound.Play();
        }
    }

    private void FireGrenade()
    {
        //no bullet to fire
        if (m_GrenadeBulletTemplate == null || m_LauncherSocket == null)
            return;

        if (m_CurrentLaucherAmmo > 0)
        {
            --m_CurrentLaucherAmmo;

            Instantiate(m_GrenadeBulletTemplate, m_LauncherSocket.position, m_LauncherSocket.rotation);

            if (m_LauncherSound)
            {
                m_LauncherSound.Play();
            }

            if (m_LaucherAmmoInStock > 0)
            {
                Invoke("ReloadLauncher", m_LauncherReloadTime);

                if (m_AttachedMaterial)
                {
                    m_AttachedMaterial.SetColor("_Color", m_LauncherReloadingColor);
                    Invoke("ResetColor", m_ReloadTime);
                }
            }
            else
            {
                if (m_OutOfAmmoSound)
                {
                    m_OutOfAmmoSound.Play();
                }
            }
        }
    }

    private void ScopeIn()
    {
        if (m_Camera != null && m_SniperScope != null)
        {
            m_SniperScope.SetActive(true);
            m_Camera.fieldOfView = 30;
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            m_PlayerCharacter.SetSniperScoping();
        }
    }

    private void ScopeOut()
    {
        if (m_Camera != null && m_SniperScope != null)
        {
            m_Camera.fieldOfView = 60;
            m_SniperScope.SetActive(false);
            gameObject.GetComponentInChildren<Renderer>().enabled = true;
        }
    }
    public void Fire()
    {
        m_TriggerPulled = true;
    }

    public void RightMouse()
    {
        m_RightMousePulled = true;
    }

    public void Reload()
    {
        if (m_AmmoInClips > 0 && m_CurrentAmmo < m_ClipSize)
        {
            if (m_AttachedMaterial)
            {
                m_AttachedMaterial.SetColor("_Color", m_ReloadingColor);
                Invoke("ResetColor", m_ReloadTime);
            }

            Invoke("RefillClip", m_ReloadTime);
            m_IsReloading = true;
            m_ReloadFailed = false;
        }
        else
        {
            m_ReloadFailed = true;
            if (m_OutOfAmmoSound)
            {
                m_OutOfAmmoSound.Play();
            }
        }
    }
    private void RefillClip()
    {
        int missingBullets = m_ClipSize - m_CurrentAmmo;

        if (m_AmmoInClips >= missingBullets)
        {
            m_AmmoInClips -= missingBullets;
            m_CurrentAmmo = m_ClipSize;
        }
        else
        {
            m_CurrentAmmo += m_AmmoInClips;
            m_AmmoInClips = 0;
        }
        m_IsReloading = false;
    }

    private void ReloadLauncher()
    {
        m_CurrentLaucherAmmo = 1;
        --m_LaucherAmmoInStock;
    }

    void ResetColor()
    {
        if (!m_AttachedMaterial) return;

        m_AttachedMaterial.SetColor("_Color", m_StartColor);
    }

    public void DisableGun()
    {
        gameObject.SetActive(false);
    }

    public void EnableGun()
    {
        gameObject.SetActive(true);
        HandleSelectFire();
    }

    private void HandleSelectFire()
    {
        //get the correct tap fire
        if (m_Sniper)
        {
            m_PlayerCharacter.TapFireLeftMouse = true;
            m_PlayerCharacter.TapFireRightMouse = false;
        }
        else if (m_GrenadeBulletTemplate != null)
        {
            m_PlayerCharacter.TapFireLeftMouse = m_TapFire;
            m_PlayerCharacter.TapFireRightMouse = true;
        }
        else
        {
            m_PlayerCharacter.TapFireLeftMouse = m_TapFire;
            m_PlayerCharacter.TapFireRightMouse = m_TapFire;
        }
    }
    public void RefillAmmo()
    {
        m_AmmoInClips = (m_TotalAmountOfClips - 1) * m_ClipSize;
        m_LaucherAmmoInStock = m_TotalAmountOfGrenades - 1;
    }

    public bool CheckRefillAttempt()
    {
        //true if there is only ammo left in the last reloaded clip
        return (m_AmmoInClips == 0 && m_CurrentAmmo > 0 && m_ReloadFailed == false);
    }
}
