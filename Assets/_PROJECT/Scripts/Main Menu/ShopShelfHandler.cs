using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopShelfHandler : MonoBehaviour
{
    public List<ShopData> shopDataList;
    public GameObject shopObjectPrefab;
    public Transform shopParent;

    void Start()
    {
        for (int i = 0; i < shopDataList.Count; i++)
        {
            GameObject shelf = Instantiate(shopObjectPrefab, shopParent);
            ShopButtonBehavior behav = shelf.GetComponent<ShopButtonBehavior>();
            behav.SetupImmediate(shopDataList[i], this);

            /*if (shopDataList[i].shopType == CurrencyType.NORMAL_CURRENCY) behav.button.onClick.AddListener(PurchaseCash);
            else if (shopDataList[i].shopType == CurrencyType.PREMIUM_CURRENCY) behav.button.onClick.AddListener(PurhaseGems);

            if (shopDataList[i].buyType == CurrencyType.NORMAL_CURRENCY) behav.button.onClick.AddListener(DeductCash);
            else if (shopDataList[i].buyType == CurrencyType.PREMIUM_CURRENCY) behav.button.onClick.AddListener(DeductGems);*/
        }
        
    }
    void Update()
    {
        
    }

    public void PurhaseGems(int amt)
    {

    }

    public void PurchaseCash(int amt)
    {

    }

    public void DeductGems(int amt)
    {

    }

    public void DeductCash(int amt)
    {

    }
}
