using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Level Object")]
public class LevelObject : ScriptableObject
{
    public string levelName;
    [TextArea] public string levelDescription;

    public List<RoomScriptable> levelRooms;
    public bool randomRooms;
}
