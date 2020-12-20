using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Instance;

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
        for(int i = 0; i < loadOut.Count; i++)
        {
            loadOut[i] = enemyLibrary[Random.Range(0, enemyLibrary.Count)];
        }
    }
}
