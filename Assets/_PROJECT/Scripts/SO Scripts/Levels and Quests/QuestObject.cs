using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Quest Object")]
public class QuestObject : ScriptableObject
{
    public string questName;
    [TextArea] public string questDescription;

    public List<LevelObject> questLevels;
}
