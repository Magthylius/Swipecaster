using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public float transitionSpeed = 2.0f;
    public CanvasGroup partyCanvas;
    public CanvasGroup inventoryCanvas;
    public CanvasGroup homeCanvas;
    public CanvasGroup mapCanvas;
    public CanvasGroup gachaCanvas;
    public CanvasGroup shopCanvas;
    public CanvasGroup settingsCanvas;

    CanvasGroup currentPage;
    CanvasGroup newPage;
    bool pageTransition = false;
    bool isAtHome = true;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        currentPage = homeCanvas;
        currentPage.alpha = 1f;
        currentPage.interactable = true;
        currentPage.blocksRaycasts = true;
        currentPage.transform.SetAsLastSibling();
    }

    void Update()
    {
        if (pageTransition)
        {
            newPage.alpha = Mathf.Lerp(newPage.alpha, 1.0f, transitionSpeed * Time.unscaledDeltaTime);

            if (LerpFunctions.Lerp.NegligibleDistance(newPage.alpha, 1.0f, 0.01f))
            {
                currentPage.alpha = 0f;
                newPage.alpha = 1f;
                currentPage.interactable = false;
                currentPage.blocksRaycasts = false;
                currentPage.GetComponent<MenuCanvasPage>().Reset();

                pageTransition = false;
                currentPage = newPage;
                currentPage.interactable = true;
                currentPage.blocksRaycasts = true;
                newPage = null;
            }
        }
    }

    void ActivateCanvas(CanvasGroup page)
    {
        newPage = page;
        newPage.transform.SetAsLastSibling();
        pageTransition = true;
    }

    #region Buttons
    public void BTN_Party()
    {
        if (pageTransition || partyCanvas == currentPage) return;
        ActivateCanvas(partyCanvas);
    }

    public void BTN_Inventory()
    {
        if (pageTransition || inventoryCanvas == currentPage) return;
        ActivateCanvas(inventoryCanvas);
    }

    public void BTN_HomeMap()
    {
        if (pageTransition) return;
        if (currentPage == homeCanvas || currentPage == mapCanvas) isAtHome = !isAtHome;

        if (isAtHome) ActivateCanvas(homeCanvas);
        else ActivateCanvas(mapCanvas);
    }

    public void BTN_Gacha()
    {
        if (pageTransition || gachaCanvas == currentPage) return;
        ActivateCanvas(gachaCanvas);
    }

    public void BTN_Shop()
    {
        if (pageTransition || shopCanvas == currentPage) return;
        ActivateCanvas(shopCanvas);
    }

    public void BTN_Settings()
    {
        if (pageTransition || settingsCanvas == currentPage) return;
        ActivateCanvas(settingsCanvas);
    }
    #endregion
}
