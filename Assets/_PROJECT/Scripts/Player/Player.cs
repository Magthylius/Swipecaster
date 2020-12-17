using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private int _experience;
    private int _level;
    private int _standardCurrency;
    private int _freemiumCurrency;
    private int _premiumCurrency;

    [SerializeField] private int maxLoadOutSize;
    [SerializeField] private GameObject inventoryHolder;
    [SerializeField] private List<UnitEntry> loadOut;

    public int Experience => _experience;
    public int Level => _level;
    public int FreemiumCurrency => _freemiumCurrency;
    public int PremiumCurrency => _premiumCurrency;
    public int TotalPremiumCurrency => _freemiumCurrency + _premiumCurrency;
    public List<UnitEntry> UnitLoadOut => loadOut;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
}
