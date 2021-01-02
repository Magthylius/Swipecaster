using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public List<LevelSelectBehavior> levelSelectors;

    void Start()
    {
    }

    void Update()
    {
        
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
}
