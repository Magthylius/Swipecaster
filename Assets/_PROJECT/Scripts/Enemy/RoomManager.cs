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
}

[System.Serializable]
public struct RoomSetUp
{
    public RoomScriptable roomSO;
    public int maxEnemySize;
    public bool isRandom;

    public RoomSetUp(RoomScriptable roomSO, int maxEnemySize, bool isRandom)
    {
        this.roomSO = roomSO;
        this.maxEnemySize = maxEnemySize;
        this.isRandom = isRandom;
    }
}

