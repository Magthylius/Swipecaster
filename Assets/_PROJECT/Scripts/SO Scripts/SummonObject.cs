using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/Summon Object")]
public class SummonObject : ScriptableObject
{
    [Header("General")]
    public string SummonName;
    public RuneType RuneAlignment;
    public string Description;

    [Header("Max Stats")]
    public int MaxHealth;
    public int MaxAttack;
    public int MaxDefence;

    [Header("Stat Ratio Multipliers")]
    [SerializeField] private float healthMultiplier = 0.85f;
    [SerializeField] private float attackMultiplier = 0.0f;
    [SerializeField] private float defenceMultiplier = 1.5f;

    public void CalculateMaxStats(Unit unit)
    {
        MaxHealth = Round(unit.GetMaxHealth * healthMultiplier);
        MaxAttack = Round(unit.GetBaseAttack * attackMultiplier);
        MaxDefence = Round(unit.GetBaseDefence * defenceMultiplier);
    }

    private int Round(float number) => Mathf.RoundToInt(number);
}
