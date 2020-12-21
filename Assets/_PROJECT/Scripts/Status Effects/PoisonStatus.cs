using UnityEngine;

public class PoisonStatus : StatusEffect
{
    private int _damagePerTurn;
    public int DamagePerTurn => -Mathf.Abs(_damagePerTurn);

    public override void DoImmediateEffect(Entity target) { }
    public override void DoPostEffect(Entity target)
    {
        target.AddHealth(DamagePerTurn);
        DeductRemainingTurns();
    }

    public PoisonStatus(int turns, int damagePerTurn) : base(turns) => _damagePerTurn = damagePerTurn;
}