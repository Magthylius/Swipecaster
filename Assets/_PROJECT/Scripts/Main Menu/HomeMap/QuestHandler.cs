using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public LevelConfigurationObject levelConfiguration;
    public List<LevelSelectBehavior> levelSelectors;
    public string nextLevelName = "BattleScene"; 

    EnergyManager energyManager;
    SceneTransitionManager stManager;
    QuestObject qObject;

    float _energyCost;

    void Start()
    {
        energyManager = EnergyManager.instance;
        stManager = SceneTransitionManager.instance;
        levelConfiguration = Obtain.A_Scriptable.LevelConfiguration;
    }

    void SetupLevelSelectors(QuestObject questObject)
    {
        if (levelSelectors.Count < questObject.questLevels.Count)
        {
            Debug.LogError("Level selectors less than quest levels! Init failed!");
            return;
        }

        for (int i = 0; i < levelSelectors.Count; i++)
        {
            if (i < questObject.questLevels.Count)
            {
                LevelObject level = questObject.questLevels[i];
                levelSelectors[i].gameObject.SetActive(true);
                levelSelectors[i].Setup(level.levelName, level.levelDescription);
            }
            else
            {
                levelSelectors[i].gameObject.SetActive(false);
            }
        }
    }

    public void BTN_TriggerLevel(QuestObject questObject)
    {
        SetupLevelSelectors(questObject);
        qObject = questObject;
    }

    public void BTN_SelectLevel(int index)
    {
        levelConfiguration.ActiveLevel = qObject.questLevels[index];
    }

    public void BTN_SetEnergyCost(float energy)
    {
        _energyCost = energy;
    }

    public void BTN_EnterLevel()
    {
        energyManager.SpendEnergy(_energyCost);
        stManager.ActivateTransition(nextLevelName);
    }
}
