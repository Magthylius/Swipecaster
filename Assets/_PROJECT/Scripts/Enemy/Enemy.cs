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

    public int Experience => _experience;
    public int Level => _level;
    public List<UnitEntry> UnitLoadOut => loadOut;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
}
