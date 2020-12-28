using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit/Unit Object")]
public class UnitObject : ScriptableObject
{
    [Header("General")]
    public string ID;
    [Range(3, 5)] public int BaseRarity;
	public RuneType RuneAlignment;
    public string CharacterDescription;
	
	[Header("Max Stats")]
	[Range(1, 100)] public int MaxLevel;
    public int MaxHealth;
    public int MaxAttack;
    public int MaxDefence;
    
	[Header("Current Stats")]
	[Range(1, 100)] public int CurrentLevel;
	public int LevelTotalHealth;
	public int LevelTotalAttack;
	public int LevelTotalDefence;
	
	[Header("Stat Ratio Multipliers")]
    [SerializeField] private float baseStatMultiplier = 1/3f;
    [SerializeField] private float paraCapMultiplier = 0.6f;
    [SerializeField] private float baseStatCapMultiplier = 0.4f;
	
    [Header("Skill")]
    [TextArea(1, 5)] public string SkillDescription;

    [Header("UI/Visual")]
    public GameObject FullArtPrefab;
    public GameObject SpriteHolderPrefab;
    public Sprite PortraitArt;
	
	#region Private Methods
	
	private void CalculateActualStats()
    {
        StatInfo currentInfo;

        //! Health
        currentInfo = GenerateStatInfo(MaxHealth);
        LevelTotalHealth = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));

        //! Attack
        currentInfo = GenerateStatInfo(MaxAttack);
        LevelTotalAttack = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));

        //! Defence
        currentInfo = GenerateStatInfo(MaxDefence);
        LevelTotalDefence = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));
    }
	
    private StatInfo GenerateStatInfo(float maxCap)
        => new StatInfo(maxCap * baseStatMultiplier, maxCap * paraCapMultiplier, maxCap * baseStatCapMultiplier);

    private float CalculateLevelParabolicStat(StatInfo info)
    {
        int actualLevel = Convert.ToInt32(CurrentLevel) - Convert.ToInt32(MaxLevel);
        float paraGradient = info.paraCap / -(MaxLevel * MaxLevel);
        return paraGradient * (actualLevel * actualLevel) + info.paraCap;
    }

    private float CalculateLevelLinearStat(StatInfo info)
    {
        float linearGradient = (info.baseStatCap - info.baseStat) / MaxLevel;
        return linearGradient * CurrentLevel + info.baseStat;
    }
	
	private void OnValidate()
	{
		if(CurrentLevel < 1) CurrentLevel = 1;
		if(CurrentLevel > MaxLevel) CurrentLevel = MaxLevel;
		
		CalculateActualStats();
	}

    #endregion
}

public struct StatInfo
{
    public float baseStat;
    public float paraCap;
    public float baseStatCap;

    public StatInfo(float baseStat, float paraCap, float baseStatCap)
    {
        this.baseStat = baseStat;
        this.paraCap = paraCap;
        this.baseStatCap = baseStatCap;
    }
};