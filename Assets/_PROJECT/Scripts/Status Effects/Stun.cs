public class Stun : StatusTemplate<Stun>
{
    #region Variables and Properties

    private TurnBaseManager _turnBaseManager;
    public override string StatusName => "Stunned";

    #endregion

    #region Override Methods

    public override void DoImmediateEffect(TargetInfo info) => GetUnit.SubscribeSelfTurnBeginEvent(SkipTurn);
    protected override void Deinitialise()
    {
        GetUnit.UnsubscribeSelfTurnBeginEvent(SkipTurn);
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