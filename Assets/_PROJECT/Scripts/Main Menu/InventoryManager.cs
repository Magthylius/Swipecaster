using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MenuCanvasPage
{
    DatabaseManager databaseManager;
    PlayerInventory playerInventory;

    List<UnitObject> casterInventory;

    public GameObject uiCasterObject;
    public Transform castersParent;

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        playerInventory = PlayerInventory.instance;

        UpdateInventory();
    }

    void UpdateInventory()
    {
        casterInventory = new List<UnitObject>();
        casterInventory = playerInventory.PlayerCasters;

        foreach (UnitObject unit in casterInventory)
        {
            GameObject spawn = Instantiate(uiCasterObject, castersParent);
            spawn.GetComponent<CasterInventoryBehavior>().Init(unit);
        }
    }
}
