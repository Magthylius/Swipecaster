using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButtonBehavior : MonoBehaviour
{
    CurrencyManager currencyManager; //! use currency manager to update texts
    Button _button;

    [Header("References")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI number;
    public TextMeshProUGUI type;
    public Image icon;

    ShopData data;
    ShopShelfHandler handler;

    [Header("Data")]
    public IconData asodelData;
    public IconData gemData;

    void Start()
    {
        _button = GetComponent<Button>();
        currencyManager = CurrencyManager.instance;

        if (_button == null) print("error");
    }

    public void InfoUpdate()
    {
        title.text = data.shopTitle.ToUpper();
        desc.text = data.shopDesc;
        number.text = (transform.GetSiblingIndex() + 1).ToString();

        //type.text = ShopData.CurrencyString(data.shopType);
        type.text = data.buyCost.ToString();
        
        if (data.buyType == CurrencyType.NORMAL_CURRENCY)
        {
            //type.color = asodelData.spriteColor;
            icon.sprite = asodelData.sprite;
            icon.color = asodelData.spriteColor;
        }
        else if (data.buyType == CurrencyType.PREMIUM_CURRENCY)
        {
            //type.color = gemData.spriteColor;
            icon.sprite = gemData.sprite;
            icon.color = gemData.spriteColor;
        }

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
        if (data.HasEnoughCurrency())
        {
            currencyManager.SpendCurrency(data.buyType, data.buyCost);
            currencyManager.AddCurrency(data.shopType, data.shopAmount);
        }
        else print("Not enough currency!");
    }

    public Button button => _button;
}
