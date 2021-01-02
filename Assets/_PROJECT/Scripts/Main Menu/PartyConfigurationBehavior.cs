using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyConfigurationBehavior : MonoBehaviour
{
    public static PartyConfigurationBehavior instance;
    
    CasterParty configurationParty;
    DatabaseManager databaseManager;

    public Transform castersParent;
    public UIFlexibleGrid grid;
    public List<Image> portraitList;
    public TMP_InputField partyInputField;

    GameObject curParty;
    CasterParty curPartyData;

    List<UnitObject> casterInventory;
    List<GameObject> casterInvList;
    List<UnitObject> curPartyList;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        databaseManager = DatabaseManager.instance;
        UpdateCasterInventory();
    }

    #region PlayerInventory

    //! Update Player's Casters Inventory
    void UpdateCasterInventory()
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

    //! Disable unused Portraits
    void ClearCasterInventory()
    {
        casterInvList = new List<GameObject>();
        for (int i = 0; i < castersParent.childCount; i++)
        {
            casterInvList.Add(castersParent.GetChild(i).gameObject);
            castersParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    //! Open Selected Party
    public void GetCurrentOpenedParty(GameObject _party)
    {
        curParty = _party;
        curPartyData = curParty.GetComponent<PartyGroupBehavior>().party;
        curPartyList = new List<UnitObject>();

        foreach (var unit in curPartyData.activeUnits)
        {
            curPartyList.Add(unit);
        }

        partyInputField.text = curPartyData.partyName;

        UpdateCasterInventory();
        DisablePickedCaster();
    }

    //! Fadeout selected casters in selected party
    void DisablePickedCaster()
    {
        foreach (GameObject casters in casterInvList)
        {
            Button[] temp = casters.GetComponentsInChildren<Button>();

            foreach (Button _button in temp)
            {
                _button.interactable = true;
            }
        }

        foreach (UnitObject unit in curPartyList)
        {
            for (int i = 0; i < casterInvList.Count; i++)
            {
                if (casterInvList[i].GetComponent<PartyInventoryBehaviour>().GetId() == unit.ID)
                {
                    Button[] temp = casterInvList[i].GetComponentsInChildren<Button>();

                    foreach (Button _button in temp)
                    {
                        _button.interactable = false;
                    }
                }
            }
        }
    }

    //! Fine Next Inactive portrait
    GameObject FindNextInactiveChild()
    {
        foreach (GameObject child in casterInvList)
            if (!child.activeInHierarchy)
                return child;

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
            if (i < configurationParty.activeUnits.Count)
                portraitList[i].sprite = configurationParty.activeUnits[i].PortraitArt;
            else portraitList[i].sprite = null;
        }
    }

    //! Update casters and Portraits before saving
    void UpdatePortraitsPreSave()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < curPartyList.Count)
                portraitList[i].sprite = curPartyList[i].PortraitArt;
            else portraitList[i].sprite = null;
        }
    }
    

    public void Remove(int slot)
    {
        if (slot >= curPartyList.Count)
        {
            print("party empty!");
            return;
        }
        
        curPartyList.RemoveAt(slot);
        UpdatePortraitsPreSave();
        DisablePickedCaster();
    }

    public void RemoveDirectly(UnitObject unit)
    {
        curPartyList.Remove(unit);
        UpdatePortraitsPreSave();
        DisablePickedCaster();
    }
    
    public void Add(UnitObject unit)
    {
        if (curPartyList.Count >= 4)
        {
            print("party full!");
            return;
        }
        
        curPartyList.Add(unit);
        UpdatePortraitsPreSave();
    }

    public void SaveParty()
    {
        curPartyData.partyName = partyInputField.text;
        curPartyData.activeUnits = curPartyList;
        curParty.GetComponent<PartyGroupBehavior>().UpdateAll();

        curParty = null;
        curPartyData = null;
    }
}