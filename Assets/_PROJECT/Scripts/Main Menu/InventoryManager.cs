using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MenuCanvasPage
{
    DatabaseManager databaseManager;
    PlayerInventory playerInventory;

    List<UnitObject> casterInventory;

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        playerInventory = PlayerInventory.instance;

        casterInventory = playerInventory.PlayerCasters;
    }

    void Update()
    {
        
    }
}
