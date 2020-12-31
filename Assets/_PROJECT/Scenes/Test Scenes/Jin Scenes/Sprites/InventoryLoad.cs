using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryLoad : MonoBehaviour
{
    DatabaseManager databaseManager;

    public GameObject PanelPrefab;
    public GameObject heroPrefab;

    void Start()
    {
        databaseManager = DatabaseManager.instance;

        for (int i = 0; i < databaseManager.AllCasters.Count; i++)
        {
            GameObject temp = Instantiate(PanelPrefab, transform.position, Quaternion.identity, transform);
            GameObject tempChild = Instantiate(heroPrefab, transform.position, Quaternion.identity, temp.transform);
            if (databaseManager.AllCasters[i].PortraitArt)
            {
                tempChild.GetComponent<Image>().sprite = databaseManager.AllCasters[i].PortraitArt;
            }

            tempChild.GetComponent<MenuUnitSaver>().unit = databaseManager.AllCasters[i];
        }

        this.GetComponent<MenuParty>().updateTeam();
    }
}
