using ConversionFunctions;
using System;
using System.Collections.Generic;
using System.Linq;

public enum RuneType
{
    NULL = -1,
    GRON, FYOR, TEHK, KHUA, AYRO,
    RUNE_TOTAL
}

public struct RuneStorage : IEquatable<RuneStorage>
{
    public RuneType runeType;
    public int amount;
    public static RuneStorage Null => new RuneStorage(RuneType.NULL, 0);
    
    public RuneStorage(RuneType _runeType, int _amount)
    {
        runeType = _runeType;
        amount = _amount;
    }

    #region Equals and Operators

    public override bool Equals(object obj) => obj is RuneStorage storage && Equals(storage);
    public bool Equals(RuneStorage other) => runeType == other.runeType && amount == other.amount;
    public override int GetHashCode()
    {
        int hashCode = -1298878394;
        hashCode = hashCode * -1521134295 + runeType.GetHashCode();
        hashCode = hashCode * -1521134295 + amount.GetHashCode();
        return hashCode;
    }
    public static bool operator ==(RuneStorage left, RuneStorage right) => left.Equals(right);
    public static bool operator !=(RuneStorage left, RuneStorage right) => !(left == right);

    #endregion
}

public struct RuneRelations
{
    private List<RuneType> _advantage;
    private List<RuneType> _weakness;
    public readonly List<RuneType> AdvantageList => _advantage;
    public readonly List<RuneType> WeaknessList => _weakness;
    public readonly RuneType SingleAdvantage => _advantage.Single();
    public readonly RuneType SingleWeakness => _weakness.Single();

    public static RuneRelations GetRelations(RuneType rune)
    {
        var (Advantage, Weakness) = rune switch
        {
            RuneType.GRON => (Advantage: RuneType.TEHK, Weakness: RuneType.AYRO),
            RuneType.FYOR => (Advantage: RuneType.AYRO, Weakness: RuneType.KHUA),
            RuneType.TEHK => (Advantage: RuneType.KHUA, Weakness: RuneType.GRON),
            RuneType.KHUA => (Advantage: RuneType.FYOR, Weakness: RuneType.TEHK),
            RuneType.AYRO => (Advantage: RuneType.GRON, Weakness: RuneType.FYOR),
            _ => (Advantage: RuneType.NULL, Weakness: RuneType.NULL),
        };

        return new RuneRelations(new List<RuneType>() { Advantage }, new List<RuneType>() { Weakness });
    }

    public RuneRelations(List<RuneType> advantage, List<RuneType> weakness)
    {
        _advantage = advantage;
        _weakness = weakness;
    }
}

public struct RuneCollection : IEquatable<RuneCollection>
{
    private Dictionary<RuneType, RuneStorage> _allRunes;

    public readonly int TotalRuneCount
        => GronRunes.amount + FyorRunes.amount + TehkRunes.amount + KhuaRunes.amount + AyroRunes.amount;
    public readonly RuneStorage GronRunes => _allRunes[RuneType.GRON];
    public readonly RuneStorage FyorRunes => _allRunes[RuneType.FYOR];
    public readonly RuneStorage TehkRunes => _allRunes[RuneType.TEHK];
    public readonly RuneStorage KhuaRunes => _allRunes[RuneType.KHUA];
    public readonly RuneStorage AyroRunes => _allRunes[RuneType.AYRO];
    public readonly Dictionary<RuneType, RuneStorage> GetAllStorages => _allRunes;
    public readonly RuneStorage GetRuneStorage(RuneType runeType) => _allRunes[runeType];
    public static RuneCollection Null => new RuneCollection(RuneStorage.Null, RuneStorage.Null, RuneStorage.Null, RuneStorage.Null, RuneStorage.Null);
    public static bool REquals(RuneCollection coll1, RuneCollection coll2)
        => coll1.GronRunes == coll2.GronRunes &&
           coll1.FyorRunes == coll2.FyorRunes &&
           coll1.TehkRunes == coll2.TehkRunes &&
           coll1.KhuaRunes == coll2.KhuaRunes &&
           coll1.AyroRunes == coll2.AyroRunes;

    public RuneCollection(RuneStorage gron, RuneStorage fyor, RuneStorage tehk, RuneStorage khua, RuneStorage ayro)
    {
        _allRunes = new Dictionary<RuneType, RuneStorage>
        {
            { RuneType.GRON, gron },
            { RuneType.FYOR, fyor },
            { RuneType.TEHK, tehk },
            { RuneType.KHUA, khua },
            { RuneType.AYRO, ayro }
        };
    }

    #region Equals and Operators

    public override bool Equals(object obj) => obj is RuneCollection collection && Equals(collection);
    public bool Equals(RuneCollection other)
        => GronRunes.Equals(other.GronRunes) &&
           FyorRunes.Equals(other.FyorRunes) &&
           TehkRunes.Equals(other.TehkRunes) &&
           KhuaRunes.Equals(other.KhuaRunes) &&
           AyroRunes.Equals(other.AyroRunes);
    public override int GetHashCode()
    {
        int hashCode = -2088774972;
        hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<RuneType, RuneStorage>>.Default.GetHashCode(_allRunes);
        hashCode = hashCode * -1521134295 + GronRunes.GetHashCode();
        hashCode = hashCode * -1521134295 + FyorRunes.GetHashCode();
        hashCode = hashCode * -1521134295 + TehkRunes.GetHashCode();
        hashCode = hashCode * -1521134295 + KhuaRunes.GetHashCode();
        hashCode = hashCode * -1521134295 + AyroRunes.GetHashCode();
        hashCode = hashCode * -1521134295 + TotalRuneCount.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<RuneType, RuneStorage>>.Default.GetHashCode(GetAllStorages);
        return hashCode;
    }
    public static bool operator ==(RuneCollection left, RuneCollection right) => left.Equals(right);
    public static bool operator !=(RuneCollection left, RuneCollection right) => !(left == right);

    #endregion
}