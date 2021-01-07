using System.Collections.Generic;

public class HardCover : CasterSkill
{
    public override string Description
        => "STUNS all enemies (1 turn)";

    public override TargetInfo GetActiveSkillTargets(Unit focusTarget, List<Unit> allCasters, List<Unit> allFoes)
        => new TargetInfo(focusTarget, null, null, allCasters, allFoes);

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Foes == null || targetInfo.Foes.Count == 0) return;

        targetInfo.Foes.ForEach(foe => foe.AddStatusEffect(Create.A_Status.Stun(1, TurnBaseManager.instance)));
        ResetSkillCharge();
    }

    public HardCover(Unit unit)
    {
        _startEffectDuration = 0;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _ignoreDuration = true;
        _unit = unit;
        EffectDuration0();
    }

    public HardCover(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
