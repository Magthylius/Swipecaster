using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Unit/Room Configuration Object")]
public class RoomAndSceneManagementObject : ScriptableObject
{
    private const string _roomLocation = "ScriptableObjects/Enemy/Rooms";
    private static List<RoomScriptable> _resourceRooms = new List<RoomScriptable>();
    private static Dictionary<string, RoomScriptable> _rooms = new Dictionary<string, RoomScriptable>();
    
    private const int MainMenuIndex = 0;
    private const int BattleStageIndex = 1;

    public RoomScriptable ActiveRoom;

    #region Level & Room Selection

    public void Stage1Room1() => EnterStage(nameof(Stage1Room1));
    public void Stage1Room2() => EnterStage(nameof(Stage1Room2));
    public void Stage1Room3() => EnterStage(nameof(Stage1Room3));

    #endregion

    public void ReturnToMainMenu()
    {
        ActiveRoom = null;
        SceneManager.LoadScene(MainMenuIndex, LoadSceneMode.Single);
    }

    private void EnterStage(string name)
    {
        InitialiseRoomResources();
        ActiveRoom = GetRoom(name, out var isActive);
        if(isActive) SceneManager.LoadScene(BattleStageIndex, LoadSceneMode.Single);
    }
    private RoomScriptable GetRoom(string name, out bool active)
    {
        active = _rooms.TryGetValue(name, out var value);
        return value;
    }
    private void InitialiseRoomResources()
    {
        if (_resourceRooms.Count != 0) return;
        _resourceRooms = new List<RoomScriptable>(Resources.LoadAll<RoomScriptable>(_roomLocation));
        _resourceRooms.ForEach(room => _rooms.Add(room.name, room));
    }
}
