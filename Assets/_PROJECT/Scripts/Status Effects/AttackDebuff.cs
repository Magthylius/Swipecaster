using UnityEngine;

public class AttackDebuff : StatusEffect
{
    private int _amount;
    public int Amount => -Mathf.Abs(_amount);

    public override void DoPreEffect(Entity target)
    {
        if (ShouldClear()) return;
        target.AddCurrentAttack(Amount);
    }
    public override void DoEffectOnAction(Entity target) { }
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();

    public AttackDebuff() : base() => _amount = 0;
    public AttackDebuff(int turns, int amount) : base(turns) => _amount = amount;
}