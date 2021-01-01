using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyCasterInventoryManager : MonoBehaviour
{
    DatabaseManager databaseManager;

    public Transform castersParent;

    CasterParty curParty;
    
    List<UnitObject> casterInventory;
    List<GameObject> casterInvList;
    [SerializeField]List<string> curPartyList;

    void Start()
    {
        databaseManager = DatabaseManager.instance;

        UpdateCasterInventory();
    }

    public void UpdateCasterInventory()
    {
        ClearCasterInventory();
        databaseManager.RefreshInventory();
        casterInventory = new List<UnitObject>();
        casterInventory = databaseManager.PlayerCasters;

        foreach (UnitObject unit in casterInventory)
        {
            GameObject caster = FindNextInactiveChild();
            caster.SetActive(true);
            caster.GetComponent<PartyInventoryBehaviour>().Init(unit);
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

    public void GetCurrentOpenedParty(GameObject _party)
    {
        curParty = _party.GetComponent<PartyGroupBehavior>().party;
        curPartyList = new List<string>();

        foreach (var unit in curParty.activeUnits)
        {
            curPartyList.Add(unit.ID);
        }
        
        UpdateCasterInventory();
        DisablePickedCaster();

    }

    void DisablePickedCaster()
    {

        foreach (GameObject casters in casterInvList)
        {
            Image[] temp = casters.GetComponentsInChildren<Image>();
            
            foreach (Image img in temp)
            {
                var tempColor = img.color;
                tempColor.a = 1f;
                img.color = tempColor;
            }
            
        }
        
        foreach (string unit in curPartyList)
        {
            for (int i = 0; i < casterInvList.Count; i++)
            {
                if (casterInvList[i].GetComponent<PartyInventoryBehaviour>().GetId() == unit)
                {
                    Image[] temp = casterInvList[i].GetComponentsInChildren<Image>();
                    
                    foreach (Image img in temp)
                    {
                        var tempColor = img.color;
                        tempColor.a = 0.1f;
                        img.color = tempColor;
                    }
                }
            }
        }
    }
    

    GameObject FindNextInactiveChild()
    {
        foreach (GameObject child in casterInvList) if (!child.activeInHierarchy) return child;
    
        Debug.LogError("No caster inventory space left");
        return null;
    }


}
