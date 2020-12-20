using System.Collections.Generic;

public enum RuneType
{
    NULL = 0,
    GRON = 1,
    FYOR,
    TEHK,
    AQUA,
    AYRO,
    LIGHT,
    DARK,
    MIND,
    RUNE_TOTAL
}

public struct RuneStorage
{
    public RuneType runeType;
    public int amount;

    public RuneStorage(RuneType _runeType, int _amount)
    {
        runeType = _runeType;
        amount = _amount;
    }
}

public struct FullRuneData
{
    public List<RuneStorage> storage;

    public FullRuneData(bool init = false)
    {
        storage = new List<RuneStorage>();
        RuneStorage fyor = new RuneStorage(RuneType.FYOR, 0);
        RuneStorage aqua = new RuneStorage(RuneType.AQUA, 0);
        RuneStorage gron = new RuneStorage(RuneType.GRON, 0);

        Add(fyor);
        Add(aqua);
        Add(gron);
    }

    public void Add(RuneStorage runeStorage)
    {
        storage.Add(runeStorage);
    }

    public int GetAmount(RuneType type)
    {
        foreach (RuneStorage runeStore in storage)
        {
            if (runeStore.runeType == type) return runeStore.amount;
        }

        return -1;
    }
}