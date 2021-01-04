using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public List<LevelSelectBehavior> levelSelectors;
    SceneTransitionManager stManager;

    void Start()
    {
        stManager = SceneTransitionManager.instance;
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
    }

    public void BTN_EnterLevel(int index)
    {
        stManager.ActivateTransition("BattleScene");
    }
}
