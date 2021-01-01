using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Unit/Room Configuration Object")]
public class RoomAndSceneManagementObject : ScriptableObject
{
    private const string _roomLocation = "ScriptableObjects/Enemy/Rooms";
    private static bool _isInitialised = false;
    private static List<RoomScriptable> _resourceRooms = new List<RoomScriptable>();
    private static Dictionary<string, RoomScriptable> _rooms = new Dictionary<string, RoomScriptable>();
    private static EnergyManager _energyManager = null;

    private const int MainMenuIndex = 0;
    private const int BattleStageIndex = 1;

    [SerializeField] private float smallEnergyCost;
    [SerializeField] private float mediumEnergyCost;
    [SerializeField] private float highEnergyCost;
    [SerializeField] private float superHighEnergyCost;
    public RoomScriptable ActiveRoom;

    #region Level & Room Selection

    public void Stage1Room1() => EnterStage(nameof(Stage1Room1), smallEnergyCost);
    public void Stage1Room2() => EnterStage(nameof(Stage1Room2), smallEnergyCost);
    public void Stage1Room3() => EnterStage(nameof(Stage1Room3), smallEnergyCost);

    #endregion

    public void ReturnToMainMenu()
    {
        ActiveRoom = null;
        SceneManager.LoadScene(MainMenuIndex, LoadSceneMode.Single);
    }

    private void EnterStage(string name, float energy)
    {
        InitialiseIfAny();
        ActiveRoom = GetRoom(name, out var isActive);
        if (!isActive || _energyManager == null) return;

        _energyManager.SpendEnergy(Mathf.Abs(energy));
        SceneManager.LoadScene(BattleStageIndex, LoadSceneMode.Single);
    }
    private RoomScriptable GetRoom(string name, out bool active)
    {
        active = _rooms.TryGetValue(name, out var value);
        return value;
    }
    private void InitialiseIfAny()
    {
        if (!NotInitialised) return;
        _isInitialised = true;

        _resourceRooms = new List<RoomScriptable>(Resources.LoadAll<RoomScriptable>(_roomLocation));
        _resourceRooms.ForEach(room => _rooms.Add(room.name, room));
        _energyManager = EnergyManager.instance;
    }

    private bool NotInitialised => !_isInitialised;
}
