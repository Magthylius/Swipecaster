using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LerpFunctions;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    [Header("Settings")]
    public float transitionSpeed = 2.0f;

    [Header("Canvas Pages")]
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

    [Header("Settings")]
    public RectTransform bottomOverlay;
    public float overlaySpeed = 4f;
    FlexibleRect bottomOverlayFR;

    bool showBottomOverlay = true;
    bool allowOverlayTransition = false;

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

        bottomOverlayFR = new FlexibleRect(bottomOverlay);
    }

    void Update()
    {
        if (pageTransition)
        {
            newPage.alpha = Mathf.Lerp(newPage.alpha, 1.0f, transitionSpeed * Time.unscaledDeltaTime);

            if (Lerp.NegligibleDistance(newPage.alpha, 1.0f, 0.01f))
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

        if (allowOverlayTransition)
        {
            if (showBottomOverlay)
            {
                allowOverlayTransition = !bottomOverlayFR.Lerp(bottomOverlayFR.originalPosition, overlaySpeed * Time.unscaledDeltaTime);
            }
            else
            {  
                allowOverlayTransition = !bottomOverlayFR.Lerp(bottomOverlayFR.GetBodyOffset(new Vector2(0, -1)), overlaySpeed * Time.unscaledDeltaTime, 1f);
            }
        }
    }

    void ActivateCanvas(CanvasGroup page)
    {
        newPage = page;
        newPage.transform.SetAsLastSibling();
        pageTransition = true;
    }

    #region Accessors
    public void ShowBottomOverlay()
    {
        showBottomOverlay = true;
        allowOverlayTransition = true;
    }

    public void HideBottomOverlay()
    {
        showBottomOverlay = false;
        allowOverlayTransition = true;
    }
    #endregion

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
