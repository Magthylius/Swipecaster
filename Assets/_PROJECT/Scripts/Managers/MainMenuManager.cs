using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public CanvasGroup partyCanvas;
    public CanvasGroup inventoryCanvas;
    public CanvasGroup homeMapCanvas;
    public CanvasGroup gachaCanvas;
    public CanvasGroup shopCanvas;
    public CanvasGroup settingsCanvas;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Buttons
    public void BTN_Party()
    {

    }

    public void BTN_Inventory()
    {

    }

    public void BTN_HomeMap()
    {

    }

    public void BTN_Gacha()
    {

    }

    public void BTN_Shop()
    {

    }

    public void BTN_Settings()
    {

    }
    #endregion
}
