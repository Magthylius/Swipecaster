using UnityEngine;

public class Corrosion : EmptyStatus<Corrosion>
{
    #region Variables and Properties

    private int _stackCount;
    public int StackCount => Mathf.Abs(_stackCount);
    public override string StatusName => "Corrosion";
    private StatusEffect OnHitStatus => Create.A_Status.DefenceDown(_remainingTurns);

    #endregion

    #region Override Methods

    public override void DoEffectOnAction(TargetInfo info, int totalDamage)
    {
        for (int i = 0; i < _stackCount; i++)
        {
            info.Focus.AddStatusEffect(OnHitStatus);
            if (info.Collateral.Count == 0) continue;
            info.Collateral.ForEach(u => u.AddStatusEffect(OnHitStatus));
        }
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