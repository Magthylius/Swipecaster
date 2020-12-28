using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public PlayerInventoryData playerData;
    
    PlayerInventory playerInventory;
    
    void Start()
    {
        playerInventory = PlayerInventory.instance;
        
        playerData = SaveManager.Load();
        
        playerInventory.SetPlayerInventory(playerData.inventoryCasterDataSave);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.Save(playerData);
        }
    }

}
