using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyConfigurationBehavior : MonoBehaviour
{
    CasterParty configurationParty;
    DatabaseManager databaseManager;

    public Transform castersParent;
    public UIFlexibleGrid grid;
    public List<Image> portraitList;
    public TMP_InputField partyInputField;

    GameObject curParty;
    CasterParty curPartyData;
    List<UnitObject> castersList;
    
    List<UnitObject> casterInventory;
    List<GameObject> casterInvList;
    List<string> curPartyList;
    


    void Start()
    {
        databaseManager = DatabaseManager.instance;

        UpdateCasterInventory();
    }

    #region PlayerInventory
    
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
        curParty = _party;
        curPartyData = curParty.GetComponent<PartyGroupBehavior>().party;
        curPartyList = new List<string>();

        foreach (var unit in curPartyData.activeUnits)
        {
            curPartyList.Add(unit.ID);
        }

        partyInputField.text = curPartyData.partyName;
        
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

    #endregion
    
    public void SetConfigParty(CasterParty party)
    {
        configurationParty = party;
        grid.CalculateLayoutInputHorizontal();
    }
    public void UpdatePortraits()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < configurationParty.activeUnits.Count) portraitList[i].sprite = configurationParty.activeUnits[i].PortraitArt;
            else portraitList[i].sprite = null;
        }
    }
    
    public void UpdateCasters()
    {
        castersList = new List<UnitObject>();
        castersList = curPartyData.activeUnits;
    }

    public void Remove(int slot)
    {
        if ()
        {
            
        }
    }
    
    public void SaveParty()
    {
        curPartyData.partyName = partyInputField.text;
        curParty.GetComponent<PartyGroupBehavior>().UpdateAll();

        curParty = null;
        curPartyData = null;
    }
    
    
}
