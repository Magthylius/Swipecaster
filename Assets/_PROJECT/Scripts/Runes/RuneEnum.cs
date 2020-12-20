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