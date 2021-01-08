using System;
using System.Collections.Generic;

public enum RuneType
{
    NULL = 0,
    GRON, FYOR, TEHK, KHUA, AYRO,
    RUNE_TOTAL,
    
    LIGHT,
    DARK,
    MIND
}

public struct RuneStorage
{
    public RuneType runeType;
    public int amount;
    public static RuneStorage Null => new RuneStorage(RuneType.NULL, 0);
    public static bool Equals(RuneStorage storage, RuneStorage otherStorage)
        => storage.runeType == otherStorage.runeType && storage.amount == otherStorage.amount;

    public RuneStorage(RuneType _runeType, int _amount)
    {
        runeType = _runeType;
        amount = _amount;
    }
}

public struct RuneRelations
{
    private List<RuneType> _advantage;
    private List<RuneType> _weakness;
    public List<RuneType> Advantage => _advantage;
    public List<RuneType> Weakness => _weakness;

    public static RuneRelations GetRelations(RuneType rune)
    {
        var advant = new List<RuneType>();
        var weak = new List<RuneType>();

        switch (rune)
        {
            case RuneType.GRON:
                advant.Add(RuneType.TEHK); weak.Add(RuneType.AYRO);
                break;
            case RuneType.FYOR:
                advant.Add(RuneType.AYRO); weak.Add(RuneType.KHUA);
                break;
            case RuneType.TEHK:
                advant.Add(RuneType.KHUA); weak.Add(RuneType.GRON);
                break;
            case RuneType.KHUA:
                advant.Add(RuneType.FYOR); weak.Add(RuneType.TEHK);
                break;
            case RuneType.AYRO:
                advant.Add(RuneType.GRON); weak.Add(RuneType.FYOR);
                break;
            case RuneType.NULL:
                break;
        }

        return new RuneRelations(new List<RuneType>(advant), new List<RuneType>(weak));
    }

    public RuneRelations(List<RuneType> advantage, List<RuneType> weakness)
    {
        _advantage = advantage;
        _weakness = weakness;
    }
}

public struct RuneCollection
{
    private List<RuneStorage> _allRunes;

    public int TotalRuneCount
        => GronRunes.amount + FyorRunes.amount + TehkRunes.amount + KhuaRunes.amount + AyroRunes.amount;
    public RuneStorage GronRunes => _allRunes[Convert.ToInt32(RuneType.GRON) - 1];
    public RuneStorage FyorRunes => _allRunes[Convert.ToInt32(RuneType.FYOR) - 1];
    public RuneStorage TehkRunes => _allRunes[Convert.ToInt32(RuneType.TEHK) - 1];
    public RuneStorage KhuaRunes => _allRunes[Convert.ToInt32(RuneType.KHUA) - 1];
    public RuneStorage AyroRunes => _allRunes[Convert.ToInt32(RuneType.AYRO) - 1];
    public List<RuneStorage> GetAllStorages => _allRunes;
    public static RuneCollection Null => new RuneCollection(RuneStorage.Null, RuneStorage.Null, RuneStorage.Null, RuneStorage.Null, RuneStorage.Null);
    public static bool Equals(RuneCollection coll1, RuneCollection coll2)
        => RuneStorage.Equals(coll1.GronRunes, coll2.GronRunes) &&
           RuneStorage.Equals(coll1.FyorRunes, coll2.FyorRunes) &&
           RuneStorage.Equals(coll1.TehkRunes, coll2.TehkRunes) &&
           RuneStorage.Equals(coll1.KhuaRunes, coll2.KhuaRunes) &&
           RuneStorage.Equals(coll1.AyroRunes, coll2.AyroRunes);

    public RuneCollection(RuneStorage gron, RuneStorage fyor, RuneStorage tehk, RuneStorage khua, RuneStorage ayro)
    {
        _allRunes = new List<RuneStorage>(Convert.ToInt32(RuneType.RUNE_TOTAL) - 1)
        {
            gron,
            fyor,
            tehk,
            khua,
            ayro
        };
    }
}