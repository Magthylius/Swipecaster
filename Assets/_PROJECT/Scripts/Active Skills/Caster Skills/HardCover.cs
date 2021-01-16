using UnityEngine;

[System.Serializable]
public class HardCover : CasterSkill
{
    [SerializeField] private int _stunTurns = 1;

    public override string Description
        => $"STUNS all enemies for {_stunTurns} turn(s).";

    public override void TriggerSkill(TargetInfo targetInfo, BattlestageManager battleStage)
    {
        if (targetInfo.Foes == null || targetInfo.Foes.Count == 0) return;

        targetInfo.Foes.ForEach(foe => foe.AddStatusEffect(Create.A_Status.Stun(_stunTurns, TurnBaseManager.instance)));
        ResetChargeAndEffectDuration();
    }

    public HardCover(Unit unit)
    {
        _startEffectDuration = 0;
        _maxSkillCharge = 5;
        _chargeGainPerTurn = 1;
        _freezeSkillCharge = true;
        _unit = unit;
        EffectDuration0();
    }

    public HardCover(int maxSkillCharge, int startEffectDuration, Unit unit, bool ignoreDuration = false)
        : base(maxSkillCharge, startEffectDuration, unit, ignoreDuration) { }
}
