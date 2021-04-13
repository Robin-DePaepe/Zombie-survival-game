using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    //variables
    private Plane m_CursorMovementPlane;

    [SerializeField]
    protected float m_SensitivityY = 5.0f;

    [SerializeField]
    protected float m_SensitivityX = 5.0f;

    [SerializeField]
    protected float m_Smoothing = 2.0f;

    [SerializeField] private ShootingBehaviour m_PrimaryWeapon;
    [SerializeField] private ShootingBehaviour m_SecondaryWeapon;

    [SerializeField] private AudioSource m_AmmoRefillFailed;
    [SerializeField] private AudioSource m_AmmoRefillSucces;

    Vector2 m_MouseLook;
    Vector2 m_SmoothV;

    private bool m_RefillAmoAttemptAvailable = true;
    private bool m_SniperScopeActive;
    private bool m_TapFireLeftMouse;
    private bool m_TapFireRightMouse;
    private bool m_PrimaryWeaponSelected = true;
    private bool m_GrenadeSelected = false;
    private bool m_HealthShotSelected = false;
    private MeleeAtack m_MeleeAtack;
    private MovementBehaviour m_MovementBehaviour;
    private ThrowGrenade m_Grenade;
    private HealthShot m_HealthShot;
    private GameObject m_PauseMenu;
    private Canvas m_ScoreBoardCanvas;
    private UI m_UIScript;
    [SerializeField] private Ability[] m_Abilitys;

    //functions
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //grenade related
        m_Grenade = GetComponent<ThrowGrenade>();
        if (m_Grenade != null) m_Grenade.DisableVisual();

        //healthshot related
        m_HealthShot = GetComponent<HealthShot>();
        if (m_HealthShot != null) m_HealthShot.DisableVisual();

        //pause menu
        m_PauseMenu = GameObject.Find("PauseMenu");

        if (m_PauseMenu != null)
        {
            m_PauseMenu.gameObject.SetActive(false);
        }

        //UI
        GameObject uiObject = GameObject.Find("UI");

        if (uiObject != null)
        {
            m_UIScript = uiObject.GetComponent<UI>();
        }
        //scoreboard
        GameObject scoreBoard = GameObject.Find("ScoreBoard");

        if (scoreBoard != null)
        {
            m_ScoreBoardCanvas = scoreBoard.gameObject.GetComponent<Canvas>();
            m_ScoreBoardCanvas.enabled = false;
        }
        //remove the scope if no sniper did 
        GameObject sniperScope = GameObject.Find("scope");
        if (sniperScope)
        {
            sniperScope.SetActive(false);
        }
        SelectPrimary();
    }
    void Awake()
    {
        m_CursorMovementPlane = new Plane(
        new Vector3(0.0f, 1.0f, 0.0f), transform.position);

        m_MeleeAtack = GetComponent<MeleeAtack>();

        m_MovementBehaviour = GetComponent<MovementBehaviour>();

        //ability related
        for (int i = 0; i < m_Abilitys.Length; i++)
        {
            //enable all of them
            m_Abilitys[i].enabled = true;
            //disable the incorrect ones
            if (i != LayoutMenu.AbilityIndex)
            {
                m_Abilitys[i].enabled = false;
            }
        }
    }


    private void Update()
    {
        HandleMovementInput();
        HandleAttackingInput();
        HandleSlotSelection();
        HandleMenus();
        CheckRefillAmmoAttempt();
    }

    void HandleMovementInput()
    {

        if (m_MovementBehaviour == null) return;

        if (Time.timeScale == 0f) return;

        //movement

        var horizontalMovement = Input.GetAxisRaw("MovementHorizontal");
        var verticalMovement = Input.GetAxisRaw("MovementVertical");

        var movement = horizontalMovement * this.transform.right + verticalMovement * this.transform.forward;

        //make sure the player doesn't walk upwards
        movement.y = 0;

        //check if the character is on the ground
        Ray characterRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(characterRay, out RaycastHit hitInfo, 1000f, LayerMask.GetMask("Ground")))
        {
            //player is on the ground
            if (hitInfo.distance < 3f)
            {
                //jump
                if (Input.GetButtonDown("Jump"))
                {
                    m_MovementBehaviour.Jump();
                }

                //sprint 
                if (Input.GetAxis("Sprint") > 0.0f) m_MovementBehaviour.Sprint();
            }
            else //player is in the air so slows down
            {
                movement.x *= 0.2f;
                movement.z *= 0.2f;
            }
        }

        m_MovementBehaviour.DesiredMovementDirection = movement;

        //rotation
        var mouseDirection = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        if (m_SniperScopeActive)
        {
            mouseDirection = Vector2.Scale(mouseDirection, new Vector2(m_SensitivityX * m_Smoothing / 2f, m_SensitivityY * m_Smoothing / 2f)); //slows down in half to be more precise when scoping
            m_SniperScopeActive = false;
        }
        else
        {
            mouseDirection = Vector2.Scale(mouseDirection, new Vector2(m_SensitivityX * m_Smoothing, m_SensitivityY * m_Smoothing));
        }
        m_SmoothV.x = Mathf.Lerp(m_SmoothV.x, mouseDirection.x, 1f / m_Smoothing);
        m_SmoothV.y = Mathf.Lerp(m_SmoothV.y, mouseDirection.y, 1f / m_Smoothing);
        m_MouseLook += m_SmoothV;

        ////make sure the player can't make vertical spins
        m_MouseLook.y = Mathf.Clamp(m_MouseLook.y, -40, 50);

        m_MovementBehaviour.DesiredLookatPoint = m_MouseLook;
    }

    void HandleAttackingInput()
    {
        if (m_GrenadeSelected)
        {
            if (Input.GetButtonDown("PrimaryFire"))
            {
                m_Grenade.Throw();
                m_UIScript.SetUtilityCount = m_Grenade.AmountOfGrenades;

                if (m_Grenade.AmountOfGrenades == 0)
                {
                    DeselectAll();

                    if (m_PrimaryWeaponSelected)
                    {
                        SelectPrimary();
                    }
                    else
                    {
                        SelectSecondary();
                    }
                }
            }
        }
        else if (m_HealthShotSelected)
        {
            if (Input.GetButtonDown("PrimaryFire"))
            {
                m_HealthShot.UseSyringe();
                m_UIScript.SetHealthShotCount = m_HealthShot.AmountOfShots;

                if (m_HealthShot.AmountOfShots == 0)
                {
                    DeselectAll();

                    if (m_PrimaryWeaponSelected)
                    {
                        SelectPrimary();
                    }
                    else
                    {
                        SelectSecondary();
                    }
                }
            }
        }
        else if (m_PrimaryWeaponSelected)
        {
            HandleWeapon(m_PrimaryWeapon);
        }
        else
        {
            HandleWeapon(m_SecondaryWeapon);
        }

        if (Input.GetButtonDown("Melee"))
            m_MeleeAtack.Melee();
    }

    void HandleWeapon(ShootingBehaviour weapon)
    {
        if (weapon == null)
            return;

        //fire
        if (m_TapFireLeftMouse)
        {
            if (Input.GetButtonDown("PrimaryFire"))
                weapon.PrimaryFire();
        }
        else
        {
            if (Input.GetAxis("PrimaryFire") > 0.0f)
                weapon.PrimaryFire();
        }

        if (m_TapFireRightMouse)
        {
            if (Input.GetButtonDown("SecondaryFire"))
                weapon.SecondaryFire();
        }
        else
        {
            if (Input.GetAxis("SecondaryFire") > 0.0f)
                weapon.SecondaryFire();
        }

        //reload
        if (Input.GetButtonDown("Reload"))
        {
            weapon.Reload();
        }
    }
    void HandleSlotSelection()
    {
        if (Input.GetButtonDown("Slot1"))
        {
            DeselectAll();
            SelectPrimary();
        }
        else if (Input.GetButtonDown("Slot2"))
        {
            DeselectAll();
            SelectSecondary();
        }
        else if (Input.GetButtonDown("Slot3"))
        {
            if (m_HealthShot != null)
            {
                if (m_HealthShot.AmountOfShots != 0)
                {
                    DeselectAll();
                    SelectHealthShot();
                }
            }
        }

        else if (Input.GetButtonDown("Slot4"))
        {
            if (m_Grenade != null)
            {
                if (m_Grenade.AmountOfGrenades != 0)
                {
                    DeselectAll();
                    SelectGrenade();
                }
            }
        }
    }

    void HandleMenus()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (m_PauseMenu != null)
            {
                Time.timeScale = 0f;
                m_PauseMenu.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (Input.GetButtonDown("ScoreBoard"))
        {
            m_ScoreBoardCanvas.enabled = true;
        }
        else if (Input.GetButtonUp("ScoreBoard"))
        {
            m_ScoreBoardCanvas.enabled = false;
        }
    }
    public void SetSniperScoping()
    {
        m_SniperScopeActive = true;
    }

    public bool TapFireLeftMouse
    {
        set { m_TapFireLeftMouse = value; }
    }
    public bool TapFireRightMouse
    {
        set { m_TapFireRightMouse = value; }
    }

    public void RefillAmmo()
    {
        m_Grenade.RefillNades();
        m_PrimaryWeapon.RefillAmmo();
        m_SecondaryWeapon.RefillAmmo();
        m_RefillAmoAttemptAvailable = true;
    }
    private void SelectGrenade()
    {
        m_GrenadeSelected = true;
        m_Grenade.EnableVisual();

        m_UIScript.SetUtilityCount = m_Grenade.AmountOfGrenades;
    }

    private void SelectPrimary()
    {
        m_PrimaryWeaponSelected = true;
        m_PrimaryWeapon.EnableWeapon();

        m_UIScript.SetClipCount = m_PrimaryWeapon.GetClipCount();
        m_UIScript.SetClipSize = m_PrimaryWeapon.GetClipSize();
        m_UIScript.ActivateAmmoInfo();
    }

    private void SelectSecondary()
    {
        m_PrimaryWeaponSelected = false;
        m_SecondaryWeapon.EnableWeapon();

        m_UIScript.SetClipCount = m_SecondaryWeapon.GetClipCount();
        m_UIScript.SetClipSize = m_SecondaryWeapon.GetClipSize();
        m_UIScript.ActivateAmmoInfo();
    }

    private void SelectHealthShot()
    {
        m_HealthShotSelected = true;
        m_HealthShot.EnableVisual();

        m_UIScript.SetHealthShotCount = m_HealthShot.AmountOfShots;
    }
    private void DeselectAll()
    {
        m_SecondaryWeapon.DisableWeapon();
        m_PrimaryWeapon.DisableWeapon();

        m_Grenade.DisableVisual();
        m_HealthShot.DisableVisual();

        m_GrenadeSelected = false;
        m_HealthShotSelected = false;

        m_UIScript.ResetText();
    }

    private void CheckRefillAmmoAttempt()
    {
        if (Input.GetButtonDown("xButton") && m_RefillAmoAttemptAvailable)
        {
            if (m_PrimaryWeaponSelected)
            {
                if (m_PrimaryWeapon.CheckRefillAmmoAttempt())
                {
                    m_PrimaryWeapon.RefillAmmo();

                    if (m_AmmoRefillSucces)
                    {
                        m_AmmoRefillSucces.Play();
                    }
                }
                else
                {
                    m_RefillAmoAttemptAvailable = false;

                    if (m_AmmoRefillFailed)
                    {
                        m_AmmoRefillFailed.Play();
                    }
                }
            }
            else
            {
                if (m_SecondaryWeapon.CheckRefillAmmoAttempt())
                {
                    m_SecondaryWeapon.RefillAmmo();

                    if (m_AmmoRefillSucces)
                    {
                        m_AmmoRefillSucces.Play();
                    }
                }
                else
                {
                    m_RefillAmoAttemptAvailable = false;

                    if (m_AmmoRefillFailed)
                    {
                        m_AmmoRefillFailed.Play();
                    }
                }
            }
        }
    }
}
