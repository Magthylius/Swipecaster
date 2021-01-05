using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private int maxLoadOutSize;
    [SerializeField] private bool isRandom;
    [SerializeField] private List<RoomSetUp> backupRooms;
    [SerializeField] private List<RoomSetUp> rooms;
    [SerializeField] private LevelConfigurationObject roomConfiguration;
    private int _currentRoomIndex = 0;
    private int _maxRoomIndex = 0;

    public List<RoomSetUp> Level => GetAllRooms();
    public int GetCurrentRoomIndex => _currentRoomIndex;
    public bool AnyRoomsLeft => _currentRoomIndex < _maxRoomIndex;

    public bool SetNextRoomIndex()
    {
        if(!AnyRoomsLeft) return false;
        _currentRoomIndex = Mathf.Clamp(_currentRoomIndex + 1, 0, _maxRoomIndex);
        return true;
    }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        SettleActiveRoom();
    }

    private void SettleActiveRoom()
    {
        rooms = new List<RoomSetUp>();
        if (roomConfiguration.ActiveLevel == null) return;
        int count = roomConfiguration.ActiveLevel.levelRooms.Count;
        for(int i = 0; i < count; i++) rooms.Add(new RoomSetUp(roomConfiguration.ActiveLevel.levelRooms[i]));
        _currentRoomIndex = 0;
        _maxRoomIndex = count - 1;
    }
    private List<RoomSetUp> GetAllRooms()
    {
        if (rooms.Count == 0) 
        { 
            rooms.Add(new RoomSetUp(backupRooms[0].roomSO));
            print("Level Configuration not found.");
        }
        return rooms;
    }
}

[System.Serializable]
public struct RoomSetUp
{
    public RoomObject roomSO;
    public int maxEnemySize;
    public bool isRandom;

    public RoomSetUp(RoomObject roomSO)
    {
        this.roomSO = roomSO;
        bool notNull = roomSO != null;
        maxEnemySize = notNull ? roomSO.maxEnemySize : 4;
        isRandom = notNull ? roomSO.isRandom : false;
    }
}

