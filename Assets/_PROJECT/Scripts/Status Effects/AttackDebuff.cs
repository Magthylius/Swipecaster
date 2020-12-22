using UnityEngine;

public class AttackDebuff : StatusEffect
{
    private int _amount;
    public int Amount => -Mathf.Abs(_amount);

    public override void DoImmediateEffect(Entity target)
    {
        if (ShouldClear()) return;
        target.AddCurrentAttack(Amount);
    }
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();

    public AttackDebuff() : base() => _amount = 0;
    public AttackDebuff(int turns, int amount) : base(turns) => _amount = amount;
}