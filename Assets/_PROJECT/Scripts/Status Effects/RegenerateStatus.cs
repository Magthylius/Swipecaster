using UnityEngine;

public class RegenerateStatus : StatusEffect
{
    private int _healPerTurn;
    public int HealPerTurn => Mathf.Abs(_healPerTurn);

    public override void DoImmediateEffect(Entity target) { }
    public override void DoPostEffect(Entity target)
    {
        target.AddHealth(HealPerTurn);
        DeductRemainingTurns();
    }

    public RegenerateStatus(int turns, int healPerTurn) : base(turns) => _healPerTurn = healPerTurn;
}