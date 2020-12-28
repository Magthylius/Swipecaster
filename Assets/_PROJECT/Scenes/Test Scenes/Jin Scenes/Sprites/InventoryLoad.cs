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

        for (int i = 0; i < playerInventory.GetAllCaster().Count; i++)
        {
            GameObject temp = Instantiate(PanelPrefab, transform.position, Quaternion.identity, transform);
            GameObject tempChild = Instantiate(heroPrefab, transform.position, Quaternion.identity, temp.transform);
            if (playerInventory.GetAllCaster()[i].PortraitArt)
            {
                tempChild.GetComponent<Image>().sprite = playerInventory.GetAllCaster()[i].PortraitArt;
            }

            tempChild.GetComponent<MenuUnitSaver>().unit = playerInventory.GetAllCaster()[i];
        }

        this.GetComponent<MenuParty>().updateTeam();
    }
}
