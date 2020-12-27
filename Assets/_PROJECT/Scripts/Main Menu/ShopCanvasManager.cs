using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopCanvasManager : MenuCanvasPage
{
    [System.Serializable]
    public struct ShopShelf
    {
        public List<ShopButtonBehavior> buttonList;
        public Button.ButtonClickedEvent onClick;
    }

    public List<ShopShelf> shelfList;

    void Start()
    {
        foreach (ShopShelf shelf in shelfList)
        {
            foreach (ShopButtonBehavior button in shelf.buttonList)
            {
                button.button.onClick = shelf.onClick;
            }
        }
    }

    void Update()
    {
        
    }

    public override void Reset()
    {
        
    }

    public void PurchaseOne()
    {
        print("You are a simp!");
    }
}
