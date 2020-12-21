using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [SerializeField] private int maxLoadOutSize;
    [SerializeField] private List<RoomScriptable> rooms;

    public List<RoomScriptable> Rooms => rooms;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        int listsize = Random.Range(1, maxLoadOutSize);

    }
}

/*public static Enemy Instance;

private int _experience;
private int _level;


[SerializeField] private int maxLoadOutSize;
[SerializeField] private GameObject inventoryHolder;
[SerializeField] private List<UnitEntry> loadOut;
[SerializeField] private List<UnitEntry> enemyLibrary;
public int Experience => _experience;
public int Level => _level;
public List<UnitEntry> UnitLoadOut => loadOut;
public List<UnitEntry> EnemyLibrary => enemyLibrary;
private void Awake()
{
    if (Instance != null) Destroy(gameObject);
    else Instance = this;
}

private void Start()
{
    int listsize = Random.Range(1, maxLoadOutSize);
    loadOut = new List<UnitEntry>(listsize);
    for (int i = 0; i < loadOut.Count; i++)
    {
        loadOut[i] = enemyLibrary[Random.Range(0, enemyLibrary.Count)];
    }
}*/