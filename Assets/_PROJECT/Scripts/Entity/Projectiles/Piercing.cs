using System.Collections.Generic;
using System.Linq;

public class Piercing : Projectile
{
    private static List<float> _diminishingMultiplier = new List<float>();
    private List<float> _currentMultiplier = new List<float>();
    private float MultiplierAtIndex(int index) => _currentMultiplier[index];

    public override List<float> GetDefaultDiminishingMultiplier => _diminishingMultiplier;
    public override List<float> GetCurrentDiminishingMultiplier => _currentMultiplier;
    public override void SetDiminishingMultiplier(List<float> multiplier) => _currentMultiplier = multiplier;
    public override void ResetDiminishingMultiplier() => SetDiminishingMultiplier(_diminishingMultiplier);

    public override int AssignTargetDamage(Unit damager, TargetInfo info, int damage)
    {
        if (info.Focus == null) return 0;

        float subtotalDamage = damage * _projectileDamageMultiplier;

        //! Graze
        info.Grazed.ForEach(i => i.InvokeGrazeEvent(damager, Round(subtotalDamage)));

        //! Damage
        info.Focus.TakeHit(damager, Round(subtotalDamage * MultiplierAtIndex(0)));

        //! Collateral
        for (int j = 0; j < info.Collateral.Count; j++) info.Collateral[j].InvokeHitEvent(damager, Round(subtotalDamage * MultiplierAtIndex(j + 1)));

        List<Unit> units = new List<Unit>(info.Collateral) { info.Focus };
        return units.Sum(unit => unit != null ? unit.GetTotalDamageInTurn : 0);
    }
    protected override List<Unit> GetCollateralFoes(TargetInfo info)
    {
        var collateral = new List<Unit>();
        int focusIndex = info.Foes.IndexOf(info.Focus);
        for (int i = focusIndex; i < info.Foes.Count; i++)
        {
            if (info.Foes[i] == info.Focus) continue;

            collateral.Add(info.Foes[i]);
        }
        return collateral;
    }

    public Piercing()
    {
        _projectileDamageMultiplier = 1.0f;
        _diminishingMultiplier = new List<float>(4)
        {
            1.0f,
            0.5f,
            0.25f,
            0.13f
        };
        _currentMultiplier = new List<float>(_diminishingMultiplier);
    }
    public Piercing(float damageMultiplier) : base(damageMultiplier)
    {
        _diminishingMultiplier = new List<float>(4)
        {
            1.0f,
            0.5f,
            0.25f,
            0.13f
        };
        _currentMultiplier = new List<float>(_diminishingMultiplier);
    }
    public Piercing(Unit unit)
    {
        SetUnit(unit);
        _projectileDamageMultiplier = 1.0f;
        _diminishingMultiplier = new List<float>(4)
        {
            1.0f,
            0.5f,
            0.25f,
            0.13f
        };
        _currentMultiplier = new List<float>(_diminishingMultiplier);
    }
}