using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public TextMeshProUGUI normalCurrencyText;
    public TextMeshProUGUI premiumCurrencyText;

    DatabaseManager databaseManager;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    } 

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        UpdateTexts();
    }

    void Update()
    {
        
    }

    public void UpdateTexts()
    {
        normalCurrencyText.text = NormalCurrency.ToString();
        premiumCurrencyText.text = PremiumCurrency.ToString();
    }

    public void SpendCurrency(CurrencyType type, int amount)
    {
        if (type == CurrencyType.NORMAL_CURRENCY) databaseManager.PurchaseDeduction(amount, false);
        else if (type == CurrencyType.PREMIUM_CURRENCY) databaseManager.PurchaseDeduction(amount, true);

        UpdateTexts();
    }

    public void AddCurrency(CurrencyType type, int amount)
    {
        if (type == CurrencyType.NORMAL_CURRENCY) databaseManager.AddCurrency(amount, false);
        else if (type == CurrencyType.PREMIUM_CURRENCY) databaseManager.AddCurrency(amount, true);

        UpdateTexts();
    }

    #region Accessors
    public int NormalCurrency => databaseManager.GetCurrency();
    public int PremiumCurrency => databaseManager.GetPremiumCurrency();

    public bool HasEnoughCurrency(CurrencyType type, int amount)
    {
        if (type == CurrencyType.NORMAL_CURRENCY) return databaseManager.GetCurrency() >= amount;
        else if (type == CurrencyType.PREMIUM_CURRENCY) return databaseManager.GetPremiumCurrency() >= amount;

        Debug.LogError("Currency not found!");
        return false;
    }
    #endregion
}
