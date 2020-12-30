using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private int maxLoadOutSize;
    [SerializeField] private bool isRandom;
    [SerializeField] private List<RoomSetUp> rooms;
    public List<RoomSetUp> Rooms => rooms;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {

    }

    public void SetRoomConfiguration(RoomConfigurationObject configuration)
    {
        for (int i = 0; i < configuration.ActiveRoom.Count; i++)
        {
            var room = configuration.ActiveRoom[i];
            if (room == null) continue;

            rooms.Add(new RoomSetUp(room));
        }
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

