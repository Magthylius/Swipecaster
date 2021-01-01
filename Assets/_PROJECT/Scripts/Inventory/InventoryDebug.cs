using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDebug : MonoBehaviour
{
    DatabaseManager databaseManager;

    public GameObject imageUnit;

    void Start()
    {
        databaseManager = DatabaseManager.instance;

        for (int i = 0; i < databaseManager.PlayerCasters.Count; i++)
        {
            GameObject temp = Instantiate(imageUnit, transform.position, Quaternion.identity, transform);
            if (databaseManager.PlayerCasters[i].PortraitArt)
            {
                temp.GetComponent<Image>().sprite = databaseManager.PlayerCasters[i].PortraitArt;
            }
        }
        
    }
}
