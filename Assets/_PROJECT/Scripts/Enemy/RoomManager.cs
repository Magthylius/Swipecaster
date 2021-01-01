using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private int maxLoadOutSize;
    [SerializeField] private bool isRandom;
    [SerializeField] private List<RoomSetUp> backupRooms;
    [SerializeField] private List<RoomSetUp> rooms;
    [SerializeField] private RoomAndSceneManagementObject roomConfiguration;

    public List<RoomSetUp> Rooms => GetRooms();
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        SettleActiveRoom();
    }

    private void SettleActiveRoom()
    {
        rooms = new List<RoomSetUp>();
        if(roomConfiguration.ActiveRoom != null) rooms.Add(new RoomSetUp(roomConfiguration.ActiveRoom));
    }
    private List<RoomSetUp> GetRooms()
    {
        if(rooms.Count == 0) rooms.Add(new RoomSetUp(backupRooms[0].roomSO));
        return rooms;
    }
}

[System.Serializable]
public struct RoomSetUp
{
    public RoomScriptable roomSO;
    public int maxEnemySize;
    public bool isRandom;

    public RoomSetUp(RoomScriptable roomSO)
    {
        this.roomSO = roomSO;
        bool notNull = roomSO != null;
        maxEnemySize = notNull ? roomSO.maxEnemySize : 4;
        isRandom = notNull ? roomSO.isRandom : false;
    }
}

