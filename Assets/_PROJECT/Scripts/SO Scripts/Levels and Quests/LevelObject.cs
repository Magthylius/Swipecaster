using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/Level Object")]
public class LevelObject : ScriptableObject
{
    public string levelName;
    [TextArea] public string levelDescription;

    public List<RoomObject> levelRooms;
    public bool randomRooms;
}
