using UnityEngine;

public class DefenceDebuff : StatusEffect
{
    private int _amount;
    public int Amount => -Mathf.Abs(_amount);

    public override void DoImmediateEffect(Entity target)
    {
        if (ShouldClear()) return;
        target.AddCurrentDefence(Amount);
    }
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();

    public DefenceDebuff() : base() => _amount = 0;
    public DefenceDebuff(int turns, int amount) : base(turns) => _amount = amount;
}