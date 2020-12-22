using UnityEngine;

public class DefenceBuff : StatusEffect
{
    private int _amount;
    public int Amount => Mathf.Abs(_amount);

    public override void DoPreEffect(Entity target)
    {
        if (ShouldClear()) return;
        target.AddCurrentDefence(Amount);
    }
    public override void DoEffectOnAction(Entity target) { }
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();

    public DefenceBuff() : base() => _amount = 0;
    public DefenceBuff(int turns, int amount) : base(turns) => _amount = amount;
}