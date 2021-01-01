using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance;

    MainMenuManager mainMenuManager;
    bool inMainMenu = false;

    [Header("Setup")]
    [Min(1f)] public float maxEnergy;
    public bool startWithMaxEnergy;

    float currentEnergy;

    [Header("Recovery")]
    public float recoveryGapSeconds = 20f; //! needs to be extracted from database to avoid cheating
    float recoveryTimer = 0f;
    bool allowRecovery = false;

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void Start()
    {
        if (startWithMaxEnergy) currentEnergy = maxEnergy;
        
        CheckMainMenu();
        UpdateEnergyUI();
    }
    
    void LateUpdate()
    {
        if (allowRecovery)
        {
            recoveryTimer += Time.unscaledDeltaTime;
            if (recoveryTimer >= recoveryGapSeconds)
            {
                recoveryTimer -= recoveryGapSeconds;
                AddEnergy(1f);
            }
        }    
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene") CheckMainMenu();
    }

    public float GetCurrentEnergy => currentEnergy;

    public void AddEnergy(float energyAdd)
    {
        currentEnergy += energyAdd;
        UpdateEnergyUI();
    }

    public void SpendEnergy(float energyCost)
    {
        currentEnergy -= energyCost;
        UpdateEnergyUI();
    }

    public void CheckMainMenu()
    {
        mainMenuManager = MainMenuManager.instance;
        inMainMenu = !(mainMenuManager == null);
    }

    void UpdateEnergyUI()
    {
        if (inMainMenu)
        {
            mainMenuManager.UpdateEnergyFill(currentEnergy, maxEnergy);
        }
    }

    #region Debugs
    [ContextMenu("Spend 20 Energy")]
    public void DEBUG_Spend20Energy()
    {
        SpendEnergy(20f);
    }
    #endregion
}
