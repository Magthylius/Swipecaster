using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PierceDamageUp : StatusTemplate<PierceDamageUp>
{
    #region Variables and Properties

    private const int MaxPierceSize = 4;
    private List<float> _additiveMultiplier;
    public List<float> DamageUpMultiplier => MultiplierSum(_unit.GetCurrentProjectile.GetCurrentDiminishingMultiplier, _additiveMultiplier);
    public List<float> ResetDamageUpMultiplier => MultiplierSubtract(_unit.GetCurrentProjectile.GetCurrentDiminishingMultiplier, _additiveMultiplier);
    public override string StatusName => "Piercing Damage Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => _unit.GetCurrentProjectile.SetDiminishingMultiplier(DamageUpMultiplier);
    protected override void Deinitialise()
    {
        _unit.GetCurrentProjectile.SetDiminishingMultiplier(ResetDamageUpMultiplier);
        base.Deinitialise();
    }

    #endregion

    private static List<float> MultiplierSum(List<float> a, List<float> b)
    {
        List<float> sum = new List<float>();
        for(int i = 0; i < MaxPierceSize; i++) sum.Add(a[i] + b[i]);
        return sum;
    }

    private static List<float> MultiplierSubtract(List<float> a, List<float> b)
    {
        List<float> substraction = new List<float>();
        for (int i = 0; i < MaxPierceSize; i++) substraction.Add(a[i] - b[i]);
        return substraction;
    }

    public PierceDamageUp() : base()
    {
        _additiveMultiplier = new List<float>(4)
        {
            1.0f,
            0.5f,
            0.25f,
            0.13f
        };
    }
    public PierceDamageUp(int turns, float baseResistance, bool isPermanent, List<float> additiveMultiplier) : base(turns, baseResistance, isPermanent)
    {
        if(additiveMultiplier == null || additiveMultiplier.Count == 0 || additiveMultiplier.Count != MaxPierceSize)
        {
            _additiveMultiplier = new List<float>(4)
            {
                1.0f,
                1.0f,
                0.45f,
                0.37f
            };
        }
        else
        {
            _additiveMultiplier = additiveMultiplier;
        }
    }
}