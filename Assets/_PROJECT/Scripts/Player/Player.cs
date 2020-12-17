using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int _experience;
    private int _level;
    private int _standardCurrency;
    private int _freemiumCurrency;
    private int _premiumCurrency;

    [SerializeField] private List<Unit> loadOut;

    public int Experience => _experience;
    public int Level => _level;
    public int FreemiumCurrency => _freemiumCurrency;
    public int PremiumCurrency => _premiumCurrency;
    public int TotalPremiumCurrency => _freemiumCurrency + _premiumCurrency;
    public List<Unit> UnitLoadOut => loadOut;
}
