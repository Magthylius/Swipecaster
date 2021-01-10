public struct StatUnitPair
{
    public CasterDataStats Stat;
    public UnitObject Unit;
    public StatUnitPair(CasterDataStats stat, UnitObject unit)
    {
        Stat = stat;
        Unit = unit;
    }
}