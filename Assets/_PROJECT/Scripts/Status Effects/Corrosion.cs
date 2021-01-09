using UnityEngine;

public class Corrosion : StatusTemplate<Corrosion>
{
    #region Variables and Properties

    private int _stackCount;
    public int StackCount => Mathf.Abs(_stackCount);
    public override string StatusName => "Corrosion";
    private StatusEffect StatusToApply => Create.A_Status.Vulnerability(_remainingTurns);

    #endregion

    #region Override Methods

    public override void DoImmediateEffect(TargetInfo info)
    {
        for (int i = 0; i < _stackCount; i++) GetUnit.AddStatusEffect(StatusToApply);
    }

    #endregion

    public Corrosion() : base()
    {
        _stackCount = 0;
    }
    public Corrosion(int turns, float baseResistance, bool isPermanent, int stackCount) : base(turns, baseResistance, isPermanent)
    {
        _stackCount = Mathf.Abs(stackCount);
    }
}