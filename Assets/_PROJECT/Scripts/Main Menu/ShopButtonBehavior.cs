using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButtonBehavior : MonoBehaviour
{
    DatabaseManager dataManager;
    Button _button;

    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI number;
    public TextMeshProUGUI type;

    ShopData data;
    ShopShelfHandler handler;

    void Start()
    {
        _button = GetComponent<Button>();
        dataManager = DatabaseManager.instance;

        if (_button == null) print("error");
    }

    void Update()
    {
        
    }

    public void InfoUpdate()
    {
        title.text = data.shopTitle.ToUpper();
        desc.text = data.shopDesc;
        number.text = (transform.GetSiblingIndex() + 1).ToString();

        type.text = ShopData.CurrencyString(data.shopType);
    }

    public void Setup(ShopData _data, ShopShelfHandler _handler)
    {
        data = _data;
        handler = _handler;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Buy);
    }

    public void SetupImmediate(ShopData _data, ShopShelfHandler _handler)
    {
        Setup(_data, _handler);
        InfoUpdate();
    }

    public void Buy()
    {
        if (data.buyType == CurrencyType.NORMAL_CURRENCY)
        {
            if (dataManager.GetCurrency() >= data.buyCost)
            {
                dataManager.PurchaseDeduction(data.buyCost, false);


                if (data.shopType == CurrencyType.NORMAL_CURRENCY) dataManager.AddCurrency(data.shopAmount, false);
                else if (data.shopType == CurrencyType.PREMIUM_CURRENCY) dataManager.AddCurrency(data.shopAmount, true);
            }
            else
            {
                print("Not enough currency!");
            }
        }
        else if (data.buyType == CurrencyType.PREMIUM_CURRENCY)
        {
            if (dataManager.GetPremiumCurrency() >= data.buyCost)
            {
                dataManager.PurchaseDeduction(data.buyCost, true);

                if (data.shopType == CurrencyType.NORMAL_CURRENCY) dataManager.AddCurrency(data.shopAmount, false);
                else if (data.shopType == CurrencyType.PREMIUM_CURRENCY) dataManager.AddCurrency(data.shopAmount, true);
            }
            else
            {
                print("Not enough currency!");
            }
        }
    }

    public Button button => _button;
}
