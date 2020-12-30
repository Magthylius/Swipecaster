using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MenuCanvasPage
{
    DatabaseManager databaseManager;
    PlayerInventory playerInventory;

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        playerInventory = PlayerInventory.instance;
    }

    void Update()
    {
        
    }
}
