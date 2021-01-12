using System;
using UnityEngine;

public class UnitObject : ScriptableObject
{
    [Header("General")]
    public string ID;
    public string CharacterName;
    public int CharacterMastery;
    [Range(3, 5)] public int BaseRarity;
    public ArchTypeMajor ArchTypeMajor = ArchTypeMajor.None;
	public RuneType RuneAlignment;
    public string CharacterDescription;
    public bool IsAlive;

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
    public string SkillName;
    [TextArea(3, 5)] public string SkillDescription;
    public GameObject SummonPrefab;

    [Header("UI/Visual")]
    public GameObject FullBodyPrefab;
    public GameObject SpriteHolderPrefab;
    public Sprite PortraitArt;
    public Sprite FullBodyArt;
    public Sprite SummonArt;

    #region Utility Methods

    public string GetArchAsString => ArchTypeMajor.ToString();

    public void SyncDataForCaster(CasterDataStats data)
	{
		if(data.ID != ID) return;
		CurrentLevel = data.CurLevel;
        CharacterMastery = data.Mastery;
        IsAlive = data.IsAlive;
        CalculateActualStats();
	}
	
	public CasterDataStats GetCasterData() => new CasterDataStats(ID, CurrentLevel, CharacterMastery, IsAlive);

    public GameObject InstantiateUnit(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject unitObject = null;
        if (FullBodyPrefab == null) return unitObject;
        unitObject = Instantiate(FullBodyPrefab, position, rotation, parent);
        
        var unit = unitObject.GetComponent<Unit>();
        if (unit == null) return unitObject;
        unit.SetBaseUnit(this);
        
        var spriteRenderer = unitObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return unitObject;
        spriteRenderer.sprite = FullBodyArt;

        return unitObject;
    }

    public GameObject InstantiateSummon(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject summonObject = null;
        if (SummonPrefab == null) return summonObject;
        summonObject = Instantiate(SummonPrefab, position, rotation, parent);

        var spriteRenderer = summonObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null || SummonArt == null) return summonObject;
        spriteRenderer.sprite = SummonArt;

        return summonObject;
    }

    public virtual void CalculateRandomisedStats() => CalculateActualStats(1200, 500, 500);

    #endregion

    #region ID Specific Methods

    public virtual ActiveSkill GetUnitActiveSkill(Unit unit) => null;

    #endregion

    #region Public Calculation Method

    public void CalculateActualStats() => CalculateActualStats(MaxHealth, MaxAttack, MaxDefence);

    #endregion

    #region Private & Protected Methods

    protected void CalculateActualStats(int maxHealth, int maxAttack, int maxDefence)
    {
        StatInfo currentInfo;

        currentInfo = GenerateStatInfo(maxHealth);
        LevelTotalHealth = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));

        currentInfo = GenerateStatInfo(maxAttack);
        LevelTotalAttack = Mathf.RoundToInt(CalculateLevelParabolicStat(currentInfo) + CalculateLevelLinearStat(currentInfo));

        currentInfo = GenerateStatInfo(maxDefence);
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

    private void ClampCurrentLevel()
    {
        if (CurrentLevel < 1) CurrentLevel = 1;
        if (CurrentLevel > MaxLevel) CurrentLevel = MaxLevel;
    }

    private void UpdateSkillData(ActiveSkill skill)
    {
        if (skill == null) return;
        SkillName = skill.Name;
        SkillDescription = skill.Description;
    }

    private void OnValidate()
    {
        ClampCurrentLevel();
        CalculateActualStats();
        UpdateSkillData(GetUnitActiveSkill(null));
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