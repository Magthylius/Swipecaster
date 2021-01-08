public class Ununaliving : StatusTemplate<Ununaliving>
{
    #region Variables and Properties

    public override string StatusName => "Roar of the Undying";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => _unit.ActivateUndying();
    protected override void Deinitialise()
    {
        _unit.DeactivateUndying();
        base.Deinitialise();
    }

    #endregion

    public Ununaliving() : base() { }
    public Ununaliving(int turns, float baseResistance, bool isPermanent) : base(turns, baseResistance, isPermanent)
    {
    }
}