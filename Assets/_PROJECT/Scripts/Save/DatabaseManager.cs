using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LerpFunctions;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    string casterLocation = "ScriptableObjects/Casters/Generic";
    
    [SerializeField] PlayerInventoryData playerData;

    [SerializeField] List<string> liveCaster = new List<string>();
    
    //! Get all casters in the game
    List<UnitObject> allCasters = new List<UnitObject>();
    List<UnitObject> playerAvailableCasters = new List<UnitObject>();
    

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;    
        
        UnitObject[] tempCaster = Resources.LoadAll<UnitObject>(casterLocation);

        foreach (var _caster in tempCaster)
        {
            allCasters.Add(_caster);
        }
        
        LoadData();

        foreach (var caster in playerData.casterDatabase)
        {
            if (caster.IsAlive)
            {
                liveCaster.Add(caster.ID);
            }
        }

        CheckCasterIsAlive();
        RefreshInventory();
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveManager.Save(playerData);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.Load();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {

        }
    }

    public void RefreshInventory()
    {
        foreach (var id in liveCaster)
        {
            for (int i = 0; i < allCasters.Count; i++)
            {
                if (id == allCasters[i].ID)
                {
                    if (!playerAvailableCasters.Contains(allCasters[i]))
                    {
                        playerAvailableCasters.Add(allCasters[i]);
                    }
                }
            }
        }
    }

    void LoadData()
    {
        playerData = SaveManager.Load();

        //! Check if the save file didn't exist and make a new save with default character
        if (playerData == null)
        {
            GenerateNewSaveData();
        }
    }
    
    public void GenerateNewSaveData()
    {
        playerData = new PlayerInventoryData();
        playerData.casterDatabase.Add(new CasterDataStats("001", 1, 0, true));
        playerData.casterDatabase.Add(new CasterDataStats("002", 1, 0, true));
        playerData.casterDatabase.Add(new CasterDataStats("003", 1, 0, true));
        playerData.casterDatabase.Add(new CasterDataStats("004", 1, 0, true));
        playerData.partyDatabase.Add(new PartyData("Team A", new List<string>(){"001", "002", "003", "004"}));
        playerData.partyDatabase.Add(new PartyData("Team B", new List<string>(){"001", "002", "003", "004"}));
        playerData.partyDatabase.Add(new PartyData("Team C", new List<string>(){"001", "002", "003", "004"}));
        playerData.partyDatabase.Add(new PartyData("Team D", new List<string>(){"001", "002", "003", "004"}));
        SaveManager.Save(playerData);
        playerData = null;
    }
    
    public void SetCasterToAlive(CasterDataStats _caster)
    {
        int CDIndex = playerData.casterDatabase.IndexOf(_caster);
        
        if (!playerData.casterDatabase[CDIndex].IsAlive)
        {
            playerData.casterDatabase[CDIndex].IsAlive = true;
            CheckCasterIsAlive();
            SaveManager.Save(playerData);
            
        }
        else
            print("Caster already alive");
    }

    public void SetCasterToDead(CasterDataStats _caster)
    {
        int CDIndex = playerData.casterDatabase.IndexOf(_caster);
        
        if (playerData.casterDatabase[CDIndex].IsAlive)
        {
            playerData.casterDatabase[CDIndex].IsAlive = false;
            CheckCasterIsAlive();
            SaveManager.Save(playerData);
        }
        else
            print("Caster already dead");
    }

    public void AddCaster(string _id)
    {
        if (playerData.casterDatabase.Any(item => item.ID == _id))
        {
            //! maybe a duplicate function?
            print("Already Exist");

            for (int i = 0; i < playerAvailableCasters.Count; i++)
            {
                if (playerAvailableCasters[i].ID == _id)
                {
                    playerData.casterDatabase[i].Mastery++;
                    break;
                }
            }
            SaveManager.Save(playerData);
        }
        else
        {
            for (int i = 0; i < allCasters.Count; i++)
            {
                if (allCasters[i].ID == _id)
                {
                    playerData.casterDatabase.Add(new CasterDataStats(_id, 1, 0, true));
                    break;
                }
            }
            SaveManager.Save(playerData);
        }
    }

    public void SetArrowTransform(Vector3 arrow)
    {
        playerData.arrowTransform = arrow;
        SaveManager.Save(playerData);
    }

    void CheckCasterIsAlive()
    {
        liveCaster = new List<string>();
        
        foreach (var caster in playerData.casterDatabase)
        {
            if (caster.IsAlive)
            {
                liveCaster.Add(caster.ID);
            }
        }
    }
    
    
    #region Accessors
    
    public List<PartyData> GetPartyData() => playerData.partyDatabase;
    public List<CasterDataStats> GetCasterDataStats() => playerData.casterDatabase;
    public List<UnitObject> PlayerCasters => playerAvailableCasters;
    public List<UnitObject> AllCasters => allCasters;
    public PlayerInventoryData GetPlayerData() => playerData;
    public Vector3 GetArrowTransform() => playerData.arrowTransform;

    #endregion


}
