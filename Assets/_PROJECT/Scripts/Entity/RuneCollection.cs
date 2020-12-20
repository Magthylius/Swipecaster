using System;
using System.Collections.Generic;

public struct RuneCollection
{
    private List<RuneStorage> _allRunes;

    public int TotalRuneCount
        => GroundRunes.amount + FireRunes.amount + ElectricRunes.amount + AquaRunes.amount
         + LightRunes.amount + DarkRunes.amount + MindRunes.amount + AirRunes.amount;
    public RuneStorage GroundRunes => _allRunes[Convert.ToInt32(RuneType.GROUND) - 1];
    public RuneStorage FireRunes => _allRunes[Convert.ToInt32(RuneType.FIRE) - 1];
    public RuneStorage ElectricRunes => _allRunes[Convert.ToInt32(RuneType.ELECTRIC) - 1];
    public RuneStorage AquaRunes => _allRunes[Convert.ToInt32(RuneType.AQUA) - 1];
    public RuneStorage LightRunes => _allRunes[Convert.ToInt32(RuneType.LIGHT) - 1];
    public RuneStorage DarkRunes => _allRunes[Convert.ToInt32(RuneType.DARK) - 1];
    public RuneStorage MindRunes => _allRunes[Convert.ToInt32(RuneType.MIND) - 1];
    public RuneStorage AirRunes => _allRunes[Convert.ToInt32(RuneType.AIR) - 1];
    public List<RuneStorage> GetAllStorages => _allRunes;

    public RuneCollection(RuneStorage ground, RuneStorage fire, RuneStorage electric, RuneStorage aqua,
        RuneStorage light, RuneStorage dark, RuneStorage mind, RuneStorage air)
    {
        _allRunes = new List<RuneStorage>(Convert.ToInt32(RuneType.RUNE_TOTAL) - 1);
        _allRunes.Add(ground);
        _allRunes.Add(fire);
        _allRunes.Add(electric);
        _allRunes.Add(aqua);
        _allRunes.Add(light);
        _allRunes.Add(dark);
        _allRunes.Add(mind);
        _allRunes.Add(air);
    }
}