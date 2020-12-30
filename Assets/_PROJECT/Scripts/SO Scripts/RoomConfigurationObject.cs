using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Room Configuration Object")]
public class RoomConfigurationObject : ScriptableObject
{
    private const string roomLocation = "ScriptableObjects/Enemy/Rooms";
    private static List<RoomScriptable> _resourceRooms = new List<RoomScriptable>();
    private static Dictionary<string, RoomScriptable> _rooms = new Dictionary<string, RoomScriptable>();

    public RoomScriptable ActiveRoom;

    #region Level & Room Selection

    public void Stage1Room1() => ActiveRoom = GetRoom(nameof(Stage1Room1));
    public void Stage1Room2() => ActiveRoom = GetRoom(nameof(Stage1Room2));

    #endregion

    private RoomScriptable GetRoom(string name)
    {
        if (!_rooms.TryGetValue(name, out var value)) return null;
        return value;
    }
    private void Awake()
    {
        _resourceRooms = Resources.LoadAll<RoomScriptable>(roomLocation).ToList();
        _resourceRooms.ForEach(room => _rooms.Add(room.name, room));
    }
}
