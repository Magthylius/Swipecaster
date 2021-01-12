using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopButtonBehavior : MonoBehaviour
{
    Button _button;
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI number;
    public TextMeshProUGUI type;

    ShopData data;

    void Start()
    {
        _button = GetComponent<Button>();
        //title.text = gameObject.name;
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

    public void Setup(ShopData _data)
    {
        data = _data;
    }

    public void SetupImmediate(ShopData _data)
    {
        Setup(_data);
        InfoUpdate();
    }

    public Button button => _button;
}
