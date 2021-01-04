using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Unit/Room Configuration Object")]
public class RoomAndSceneManagementObject : ScriptableObject
{
    private const string _roomLocation = "ScriptableObjects/Enemy/Rooms";
    private static bool _isInitialised = false;
    private static List<RoomObject> _resourceRooms = new List<RoomObject>();
    private static Dictionary<string, RoomObject> _rooms = new Dictionary<string, RoomObject>();
    private static EnergyManager _energyManager = null;
    private static SceneTransitionManager _sceneManager = null;

    private const string MainMenuName = "MainMenuScene";
    private const string BattleStageName = "BattleScene";

    [SerializeField] private float smallEnergyCost;
    [SerializeField] private float mediumEnergyCost;
    [SerializeField] private float highEnergyCost;
    [SerializeField] private float superHighEnergyCost;
    public RoomObject ActiveRoom;

    #region Level & Room Selection

    public void Stage1Room1() => EnterStage(nameof(Stage1Room1), smallEnergyCost);
    public void Stage1Room2() => EnterStage(nameof(Stage1Room2), smallEnergyCost);
    public void Stage1Room3() => EnterStage(nameof(Stage1Room3), smallEnergyCost);

    #endregion

    public void ReturnToMainMenu()
    {
        InitialiseIfAny();
        ActiveRoom = null;
        if (_sceneManager == null) return;

        _sceneManager.ActivateTransition(MainMenuName);
    }

    private void EnterStage(string name, float energy)
    {
        InitialiseIfAny();
        ActiveRoom = GetRoom(name, out var isActive);
        if (!isActive || _energyManager == null) return;

        _energyManager.SpendEnergy(Mathf.Abs(energy));
        _sceneManager.ActivateTransition(BattleStageName);
    }
    private RoomObject GetRoom(string name, out bool active)
    {
        active = _rooms.TryGetValue(name, out var value);
        return value;
    }
    private void InitialiseIfAny()
    {
        if (NotInitialised)
        {
            _isInitialised = true;
            _resourceRooms = new List<RoomObject>(Resources.LoadAll<RoomObject>(_roomLocation));
            _resourceRooms.ForEach(room => _rooms.Add(room.name, room));
        }
        
        if(_energyManager == null) _energyManager = EnergyManager.instance;
        if(_sceneManager == null) _sceneManager = SceneTransitionManager.instance;
    }

    private bool NotInitialised => !_isInitialised;
}
