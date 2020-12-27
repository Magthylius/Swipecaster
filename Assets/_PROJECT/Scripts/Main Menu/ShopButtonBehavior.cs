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

    void Start()
    {
        _button = GetComponent<Button>();
        title.text = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Button button => _button;
}
