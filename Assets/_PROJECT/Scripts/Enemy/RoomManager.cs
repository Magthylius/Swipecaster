using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private int maxLoadOutSize;
    [SerializeField] private bool isRandom;
    [SerializeField] private List<RoomSetUp> rooms;
    [SerializeField] private RoomConfigurationObject roomConfiguration;

    public List<RoomSetUp> Rooms => rooms;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        SettleActiveRoom();
    }

    private void SettleActiveRoom()
    {
        rooms = new List<RoomSetUp>();
        rooms.Add(new RoomSetUp(roomConfiguration.ActiveRoom));
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
        maxEnemySize = roomSO.maxEnemySize;
        isRandom = roomSO.isRandom;
    }
}

