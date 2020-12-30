using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Room Configuration Object")]
public class RoomConfigurationObject : ScriptableObject
{
    private const string roomLocation = "ScriptableObjects/Enemy/Rooms";
    private static List<RoomScriptable> _resourceRooms = new List<RoomScriptable>();

    public List<RoomScriptable> ActiveRoom;

    public void UseRoom()
    {

    }

    private RoomScriptable GetRoomByName(string name)
    {
        foreach(var room in _resourceRooms)
        {
            if (room.roomName.Equals(name)) return room;
        }
        return null;
    }
    private void Awake()
    {
        _resourceRooms = Resources.LoadAll<RoomScriptable>(roomLocation).ToList();
    }
}
