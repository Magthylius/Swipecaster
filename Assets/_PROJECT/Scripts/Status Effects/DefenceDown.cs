using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DefenceDown : StatusEffect
{
    #region Variables and Properties

    private float _defenceDownPercent;
    public float DefenceDownPercent => Mathf.Abs(_defenceDownPercent);
    public override string StatusName => "Vulnerable";

    #endregion

    #region Override Methods

    public override void DoPreEffect(Entity target)
    {
        if (ShouldClear()) return;
        int defenceToDeduct = Mathf.Abs(Round(target.GetBaseDefence * DefenceDownPercent));
        target.AddCurrentDefence(-defenceToDeduct);
    }
    public override void DoEffectOnAction(Entity target) { }
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();
    protected override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<DefenceDown>().Count();

    #endregion

    public DefenceDown() : base()
    {
        _defenceDownPercent = 0.0f;
    }
    public DefenceDown(int turns, float baseResistance, bool isPermanent, float defDownPercent) : base(turns, baseResistance, isPermanent)
    {
        _defenceDownPercent = Mathf.Clamp01(defDownPercent);
    }
}