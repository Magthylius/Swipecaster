public class Stun : EmptyStatus<Stun>
{
    #region Variables and Properties

    private TurnBaseManager _turnBaseManager;
    public override string StatusName => "Stunned";

    #endregion

    #region Override Methods

    public override void DoImmediateEffect(TargetInfo info) => _unit.SubscribeTurnBeginEvent(SkipTurn);
    protected override void Deinitialise()
    {
        _unit.UnsubscribeTurnBeginEvent(SkipTurn);
        base.Deinitialise();
    }

    #endregion

    private void SkipTurn()
    {
        if (_turnBaseManager == null || ShouldClear()) return;
        _turnBaseManager.EndTurn();
    }

    public Stun() : base()
    {
        _turnBaseManager = null;
    }
    public Stun(int turns, float baseResistance, bool isPermanent, TurnBaseManager turnBase) : base(turns, baseResistance, isPermanent)
    {
        _turnBaseManager = turnBase;
    }
}