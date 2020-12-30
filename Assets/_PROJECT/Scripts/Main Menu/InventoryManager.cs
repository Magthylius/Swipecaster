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

    List<GameObject> casterInvList;

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        playerInventory = PlayerInventory.instance;

        UpdateCasterInventory();
    }

    public void UpdateCasterInventory()
    {
        ClearCasterInventory();
        databaseManager.RefreshInventory();
        casterInventory = new List<UnitObject>();
        casterInventory = playerInventory.PlayerCasters;

        foreach (UnitObject unit in casterInventory)
        {
            GameObject caster = FindNextInactiveChild();
            caster.SetActive(true);
            caster.GetComponent<CasterInventoryBehavior>().Init(unit);
        }
    }

    public void ClearCasterInventory()
    {
        casterInvList = new List<GameObject>();
        for (int i = 0; i < castersParent.childCount; i++)
        {
            casterInvList.Add(castersParent.GetChild(i).gameObject);
            castersParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    GameObject FindNextInactiveChild()
    {
        foreach (GameObject child in casterInvList) if (!child.activeInHierarchy) return child;

        Debug.LogError("No caster inventory space left");
        return null;
    }
}
