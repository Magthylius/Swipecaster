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
    public int MaxHealth = 0;
    public int MaxAttack = 0;
    public int MaxDefence = 0;

    [Header("Stat Ratio Multipliers")]
    [SerializeField] private float healthMultiplier = 1.0f;
    [SerializeField] private float attackMultiplier = 1.0f;
    [SerializeField] private float defenceMultiplier = 1.0f;

    public void CalculateMaxStats(Unit unit)
    {
        MaxHealth = Round(unit.GetMaxHealth * healthMultiplier);
        MaxAttack = Round(unit.GetBaseAttack * attackMultiplier);
        MaxDefence = Round(unit.GetBaseDefence * defenceMultiplier);
    }

    public void SetStatMultipliers(float health, float attack, float defence)
    {
        SetHealthMultiplier(health);
        SetAttackMultiplier(attack);
        SetDefenceMultiplier(defence);
    }

    public void SetHealthMultiplier(float multiplier) => healthMultiplier = multiplier;
    public void SetAttackMultiplier(float multiplier) => attackMultiplier = multiplier;
    public void SetDefenceMultiplier(float multiplier) => defenceMultiplier = multiplier;

    private int Round(float number) => Mathf.RoundToInt(number);
}
