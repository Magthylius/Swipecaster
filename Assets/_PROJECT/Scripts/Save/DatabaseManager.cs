using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    string casterLocation = "ScriptableObjects/Casters/ID";
    
    [SerializeField] PlayerInventoryData playerData;

    [SerializeField] List<string> liveCaster = new List<string>();
    
    //! Get all casters in the game
    List<UnitObject> allCasters = new List<UnitObject>();
    List<UnitObject> playerAvailableCasters = new List<UnitObject>();
    
    
    [Header("Scriptable Object")] 
    public CasterParty[] parties;
    public CasterParty defaultParty;
    public CasterParty activeParty;

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
        UpdateUnitObjectsWithDataStats();
        
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
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
        CheckCasterIsAlive();

        playerAvailableCasters = new List<UnitObject>();
        
        foreach (CasterDataStats _caster in playerData.casterDatabase)
        {
            for (int i = 0; i < allCasters.Count; i++)
            {
                if (_caster.ID == allCasters[i].ID)
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
        playerData.currencyDatabase = new CurrencyData(1000, 10);
        playerData.casterDatabase.Add(new CasterDataStats("001", 1, 0, true));
        playerData.casterDatabase.Add(new CasterDataStats("002", 1, 0, true));
        playerData.casterDatabase.Add(new CasterDataStats("004", 1, 0, true));
        playerData.partyDatabase.Add(new PartyData("Team A", new List<string>(){"001"}));
        playerData.partyDatabase.Add(new PartyData("Team B", new List<string>(){"001"}));
        playerData.partyDatabase.Add(new PartyData("Team C", new List<string>(){"001"}));
        playerData.partyDatabase.Add(new PartyData("Team D", new List<string>(){"001"}));

        char letter = 'A';
        
        for (int i = 0; i < parties.Length; i++)
        {
            parties[i].activeUnits = defaultParty.activeUnits;
            parties[i].partyName = "Team " + letter;

            letter++;
        }

        activeParty = parties[0];

        Save();
    }

    public void Save()
    {
        UpdateDataStatsWithUnitObjects();
        SaveManager.Save(playerData);
    }
    
    public void SetCasterToAlive(CasterDataStats _caster)
    {
        int CDIndex = playerData.casterDatabase.IndexOf(_caster);
        
        if (!playerData.casterDatabase[CDIndex].IsAlive)
        {
            playerData.casterDatabase[CDIndex].IsAlive = true;
            CheckCasterIsAlive();
            Save();

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
            Save();
        }
        else
            print("Caster already dead");
    }

    public void AddCaster(string id)
    {
        if (playerData.casterDatabase.Any(item => item.ID == id))
        {
            //! maybe a duplicate function?
            print("Already Exist");

            for (int i = 0; i < playerAvailableCasters.Count; i++)
            {
                if (playerAvailableCasters[i].ID == id)
                {
                    playerData.casterDatabase[i].Mastery++;
                    break;
                }
            }
            Save();
        }
        else
        {
            for (int i = 0; i < allCasters.Count; i++)
            {
                if (allCasters[i].ID == id)
                {
                    playerData.casterDatabase.Add(new CasterDataStats(id, 1, 0, true));
                    break;
                }
            }
            Save();
        }
    }

    public void SetArrowTransform(Vector3 arrow)
    {
        playerData.arrowTransform = arrow;
        Save();
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

    // ! true == premium currency
    // ! false == regular currency

    public void PurchaseDeduction(int price, bool isPremium)
    {
        if (isPremium)
        {
            playerData.currencyDatabase.PremiumCurrency -= price;
        }
        else
        {
            playerData.currencyDatabase.Currency -= price;
        }
        
        Save();
    }

    public void AddCurrency(int price, bool isPremium)
    {
        if (isPremium)
        {
            playerData.currencyDatabase.PremiumCurrency += price;
        }
        else
        {
            playerData.currencyDatabase.Currency += price;
        }
        
        Save();
    }

    public void AddCurrency(int amount, CurrencyType type)
    {
        if (type == CurrencyType.NORMAL_CURRENCY) AddCurrency(amount, false);
        else if (type == CurrencyType.PREMIUM_CURRENCY) AddCurrency(amount, true);
    }

    [ContextMenu("Restart Tutorial")]
    public void RestartTutorial()
    {
        playerData.tutorialPhase = TutorialPhase.guideToMap;
    }

    public void SaveTutorialState(TutorialPhase _phase)
    {
        playerData.tutorialPhase = _phase;
        Save();
    }
    
    private void UpdateUnitObjectsWithDataStats() => GetDatabaseStatUnitPairs().ToList().ForEach(pair => pair.Unit.SyncDataForCaster(pair.Stat));
    private void UpdateDataStatsWithUnitObjects() => playerData.casterDatabase = new List<CasterDataStats>(GetDatabaseStatUnitPairs().Select(x => x.Unit.GetCasterData()));
    private IEnumerable<StatUnitPair> GetDatabaseStatUnitPairs() => GetCasterDataStats().Join(AllUnitObjects, stat => stat.ID, unit => unit.ID, (s, u) => new StatUnitPair(s, u));

    #region Accessors

    public List<PartyData> GetPartyData() => playerData.partyDatabase;
    public List<CasterDataStats> GetCasterDataStats() => playerData.casterDatabase;
    public List<UnitObject> PlayerCasters => playerAvailableCasters;
    public List<UnitObject> AllCasters => allCasters;
    public PlayerInventoryData GetPlayerData() => playerData;
    public Vector3 GetArrowTransform() => playerData.arrowTransform;
    public IEnumerable<UnitObject> AllUnitObjects => Resources.LoadAll<UnitObject>(casterLocation);

    public int GetCurrency() => playerData.currencyDatabase.Currency;
    public int GetPremiumCurrency() => playerData.currencyDatabase.PremiumCurrency;
    public TutorialPhase GetTutorialPhase() => playerData.tutorialPhase;

    #endregion
}
