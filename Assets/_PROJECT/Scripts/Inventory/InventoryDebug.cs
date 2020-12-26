using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDebug : MonoBehaviour
{
    PlayerInventory playerInventory;

    public GameObject imageUnit;

    void Start()
    {
        playerInventory = PlayerInventory.instance;

        for (int i = 0; i < playerInventory.GetPlayerCastersData().Count; i++)
        {
            GameObject temp = Instantiate(imageUnit, transform.position, Quaternion.identity, transform);
            temp.GetComponent<Image>().sprite = playerInventory.GetPlayerCastersData()[i].PortraitArt;
        }
        
    }
}
