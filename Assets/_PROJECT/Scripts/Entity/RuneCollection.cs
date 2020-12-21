using System;
using System.Collections.Generic;

public struct RuneCollection
{
    private List<RuneStorage> _allRunes;

    public int TotalRuneCount
        => GronRunes.amount + FyorRunes.amount + TehkRunes.amount + AquaRunes.amount
         + LightRunes.amount + DarkRunes.amount + MindRunes.amount + AyroRunes.amount;
    public RuneStorage GronRunes => _allRunes[Convert.ToInt32(RuneType.GRON) - 1];
    public RuneStorage FyorRunes => _allRunes[Convert.ToInt32(RuneType.FYOR) - 1];
    public RuneStorage TehkRunes => _allRunes[Convert.ToInt32(RuneType.TEHK) - 1];
    public RuneStorage AquaRunes => _allRunes[Convert.ToInt32(RuneType.AQUA) - 1];
    public RuneStorage LightRunes => _allRunes[Convert.ToInt32(RuneType.LIGHT) - 1];
    public RuneStorage DarkRunes => _allRunes[Convert.ToInt32(RuneType.DARK) - 1];
    public RuneStorage MindRunes => _allRunes[Convert.ToInt32(RuneType.MIND) - 1];
    public RuneStorage AyroRunes => _allRunes[Convert.ToInt32(RuneType.AYRO) - 1];
    public List<RuneStorage> GetAllStorages => _allRunes;

    public RuneCollection(RuneStorage gron, RuneStorage fyor, RuneStorage tehk, RuneStorage aqua,
        RuneStorage light, RuneStorage dark, RuneStorage mind, RuneStorage ayro)
    {
        _allRunes = new List<RuneStorage>(Convert.ToInt32(RuneType.RUNE_TOTAL) - 1)
        {
            gron,
            fyor,
            tehk,
            aqua,
            light,
            dark,
            mind,
            ayro
        };
    }
}