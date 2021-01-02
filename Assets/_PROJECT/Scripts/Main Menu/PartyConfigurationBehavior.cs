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

    List<UnitObject> casterInventory;
    List<GameObject> casterInvList;
    List<UnitObject> castersList;


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
        partyInputField.text = configurationParty.partyName;

        UpdateCasterInventory();
        DisablePickedCaster();
    }

    //! Fadeout selected casters in selected party
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

        foreach (UnitObject unit in configurationParty.activeUnits)
        {
            for (int i = 0; i < casterInvList.Count; i++)
            {
                if (casterInvList[i].GetComponent<PartyInventoryBehaviour>().GetId() == unit.ID)
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

    public void UpdateCasters()
    {
        castersList = new List<UnitObject>();
        castersList = configurationParty.activeUnits;
    }

    public void Remove(int slot)
    {
        
    }

    public void SaveParty()
    {
        configurationParty.partyName = partyInputField.text;
        configurationParty.activeUnits = castersList;
        curParty.GetComponent<PartyGroupBehavior>().UpdateAll();

        curParty = null;
    }
}