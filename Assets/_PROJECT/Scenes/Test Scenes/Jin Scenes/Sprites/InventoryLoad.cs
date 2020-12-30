using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryLoad : MonoBehaviour
{
    PlayerInventory playerInventory;

    public GameObject PanelPrefab;
    public GameObject heroPrefab;

    void Start()
    {
        playerInventory = PlayerInventory.instance;

        for (int i = 0; i < playerInventory.AllCasters.Count; i++)
        {
            GameObject temp = Instantiate(PanelPrefab, transform.position, Quaternion.identity, transform);
            GameObject tempChild = Instantiate(heroPrefab, transform.position, Quaternion.identity, temp.transform);
            if (playerInventory.AllCasters[i].PortraitArt)
            {
                tempChild.GetComponent<Image>().sprite = playerInventory.AllCasters[i].PortraitArt;
            }

            tempChild.GetComponent<MenuUnitSaver>().unit = playerInventory.AllCasters[i];
        }

        this.GetComponent<MenuParty>().updateTeam();
    }
}
