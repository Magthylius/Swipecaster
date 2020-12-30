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

        for (int i = 0; i < playerInventory.PlayerCasters.Count; i++)
        {
            GameObject temp = Instantiate(imageUnit, transform.position, Quaternion.identity, transform);
            if (playerInventory.PlayerCasters[i].PortraitArt)
            {
                temp.GetComponent<Image>().sprite = playerInventory.PlayerCasters[i].PortraitArt;
            }
        }
        
    }
}
