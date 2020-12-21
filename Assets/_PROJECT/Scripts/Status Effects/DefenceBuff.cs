using UnityEngine;

public class DefenceBuff : StatusEffect
{
    private int _amount;
    public int Amount => Mathf.Abs(_amount);

    public override void DoImmediateEffect(Entity target) => target.AddCurrentDefence(Amount);
    public override void DoPostEffect(Entity target) => DeductRemainingTurns();

    public DefenceBuff(int turns, int amount) : base(turns) => _amount = amount;
}