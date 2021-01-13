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

    #region Accessors
    public int NormalCurrency => databaseManager.GetCurrency();
    public int PremiumCurrency => databaseManager.GetPremiumCurrency();
    #endregion
}
