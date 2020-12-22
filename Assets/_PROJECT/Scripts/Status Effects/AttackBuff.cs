using UnityEngine;

public class AttackBuff : StatusEffect
{
    private int _amount;
    public int Amount => Mathf.Abs(_amount);

    public override void DoPreEffect(Entity target)
    {
        if (ShouldClear()) return;
        target.AddCurrentAttack(Amount);
    }
    public override void DoEffectOnAction(Entity target) { }
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();

    public AttackBuff() : base() => _amount = 0;
    public AttackBuff(int turns, int amount) : base(turns) => _amount = amount;
}