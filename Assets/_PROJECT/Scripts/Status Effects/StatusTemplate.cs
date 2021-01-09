using System.Linq;
using System.Collections.Generic;

public abstract class StatusTemplate<T> : StatusEffect where T : StatusEffect
{
    public override void DoImmediateEffect(TargetInfo info) => GetUnit.UpdateStatusEffects();
    public override void UpdateStatus() { }
    public override void DoEffectOnAction(TargetInfo info, int totalDamage) { }
    public override void DoOnHitEffect(Unit damager, TargetInfo info, int totalDamage) { }
    public override void DoPostEffect()
    {
        DeductRemainingTurns();
        if (ShouldClear()) InvokeSelfDestructEvent();
    }
    protected override int GetCountOfType(List<StatusEffect> statusList) => statusList.OfType<T>().Count();
    protected override void Deinitialise()
    {
        UnsubscribeSelfDestructEvent(Deinitialise);
        GetUnit.GetStatusEffects.Remove(this);
        GetUnit.UpdateStatusEffects();
    }

    public StatusTemplate() : base() { }
    public StatusTemplate(int turns, float baseResistance, bool isPermanent)
        : base(turns, baseResistance, isPermanent) { }
}
