using UnityEngine;

[System.Serializable]
public enum CurrencyType
{
    NULL = 0,
    PREMIUM_CURRENCY,
    NORMAL_CURRENCY,
    ACTUAL_CURRENCY
}

[CreateAssetMenu(menuName = "Shop/Shop Data")]
public class ShopData : ScriptableObject
{
    [Header("Shop Settings")]
    public string shopTitle;
    [TextArea] public string shopDesc;
    public CurrencyType shopType;
    public int shopAmount;

    [Header("Buy Settings")]
    public int buyCost;
    public CurrencyType buyType;

    public static string CurrencyString(CurrencyType type)
    {
        if (type == CurrencyType.NORMAL_CURRENCY) return "NORMAL";
        else if (type == CurrencyType.PREMIUM_CURRENCY) return "PREMIUM";
        else if (type == CurrencyType.ACTUAL_CURRENCY) return "ACTUAL";

        return "NULL";
    }

    public bool HasEnoughCurrency()
    {
        switch(buyType)
        {
            case CurrencyType.NORMAL_CURRENCY:
                return DatabaseManager.instance.GetCurrency() >= buyCost;

            case CurrencyType.PREMIUM_CURRENCY:
                return DatabaseManager.instance.GetPremiumCurrency() >= buyCost;
        }

        return false;
    }
}
