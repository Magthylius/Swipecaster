using UnityEngine;

public class PriorityUp : EmptyStatus<PriorityUp>
{
    #region Variables and Properties

    private int _priorityIncrement;
    private int PriorityIncrementAmount => Mathf.Abs(_unit.GetUnitPriority + _priorityIncrement);
    private int PriorityResetAmount => Mathf.Abs(_unit.GetUnitPriority - _priorityIncrement);
    public override string StatusName => "Priority Up";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => _unit.SetUnitPriority(PriorityIncrementAmount);
    protected override void Deinitialise()
    {
        _unit.SetUnitPriority(PriorityResetAmount);
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