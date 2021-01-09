using UnityEngine;

public class PriorityUp : StatusTemplate<PriorityUp>
{
    #region Variables and Properties

    private int _priorityIncrement;
    private int PriorityIncrementAmount => Mathf.Abs(GetUnit.GetUnitPriority + _priorityIncrement);
    private int PriorityResetAmount => Mathf.Abs(GetUnit.GetUnitPriority - _priorityIncrement);
    public override string StatusName => "Priority Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => GetUnit.SetUnitPriority(PriorityIncrementAmount);
    protected override void Deinitialise()
    {
        GetUnit.SetUnitPriority(PriorityResetAmount);
        base.Deinitialise();
    }

    #endregion

    public PriorityUp() : base()
    {
        _priorityIncrement = 0;
    }
    public PriorityUp(int turns, float baseResistance, bool isPermanent, int priorityIncrement) : base(turns, baseResistance, isPermanent)
    {
        _priorityIncrement = Mathf.Abs(priorityIncrement);
    }
}