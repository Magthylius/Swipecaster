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
            shelf.GetComponent<ShopButtonBehavior>().SetupImmediate(shopDataList[i]);
        }
        
    }
    void Update()
    {
        
    }
}
