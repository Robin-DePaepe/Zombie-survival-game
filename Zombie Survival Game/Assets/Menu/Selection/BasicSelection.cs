using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasicSelection : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected GameObject m_Prefab;
    [SerializeField] Text m_TokenPrize;

    protected Image m_Image;
    private Locked m_Locked;

    protected virtual void Awake()
    {
        m_Image = GetComponent<Image>();

        if (m_Prefab != null)
        {
            m_Locked = m_Prefab.GetComponent<Locked>();
        }

        if (m_Locked != null)
        {
            ItemsLockedTracker.AddItem(m_Prefab, m_Locked.IsLocked);

            if (ItemsLockedTracker.GetLockedStatus(m_Prefab))
            {
                m_Image.color = Color.gray;
                m_TokenPrize.text = m_Locked.UnlockPrize.ToString();
            }
        }
    }
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user left-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            if (m_Locked != null)
            {
                if (ItemsLockedTracker.GetLockedStatus(m_Prefab))
                {
                    //if the buying failed we don't want the gun to move on to be selected
                    if (BuyItem() == false) return;
                }
            }
            LeftMouseClicked();
        }
        if (m_Locked != null)
        {
            if (ItemsLockedTracker.GetLockedStatus(m_Prefab))
                return;
        }
        //Use this to tell when the user right-clicks on the Button
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            RightMouseClicked();
        }
    }

    protected virtual void LeftMouseClicked()
    {

    }

    protected virtual void RightMouseClicked()
    {

    }

    public void ResetColor()
    {
        m_Image.color = Color.white;
    }

    private bool BuyItem()
    {
        int itemPrize = m_Locked.UnlockPrize;

        if (itemPrize <= Tokens.tokens && m_Prefab != null)
        {
            Tokens.ChangeTokens(-itemPrize);
            ItemsLockedTracker.UnlockItem(m_Prefab);
            ResetColor();
            m_TokenPrize.text = "";
            return true;
        }
        return false;
    }
}
