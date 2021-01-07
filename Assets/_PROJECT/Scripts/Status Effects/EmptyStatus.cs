using System.Linq;
using System.Collections.Generic;

public abstract class EmptyStatus<T> : StatusEffect where T : StatusEffect
{
    public override void DoImmediateEffect(TargetInfo info) => _unit.UpdateStatusEffects();
    public override void UpdateStatus() { }
    public override void DoEffectOnAction(TargetInfo info, int totalDamage) { }
    public override void DoOnHitEffect(TargetInfo info, int totalDamage) { }
    public override void DoPostEffect()
    {
        DeductRemainingTurns();
        if (ShouldClear()) InvokeSelfDestructEvent();
    }
    protected override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<T>().Count();
    protected override void Deinitialise()
    {
        UnsubscribeSelfDestructEvent(Deinitialise);
        _unit.GetStatusEffects.Remove(this);
    }

    public EmptyStatus() : base() { }
    public EmptyStatus(int turns, float baseResistance, bool isPermanent)
        : base(turns, baseResistance, isPermanent) { }
}
