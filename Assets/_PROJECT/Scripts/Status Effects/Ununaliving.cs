public class Ununaliving : StatusTemplate<Ununaliving>
{
    #region Variables and Properties

    public override string StatusName => "Roar of the Undying";

    #endregion

    #region Override Methods

    public override void UpdateStatus() => GetUnit.ActivateUndying();
    protected override void Deinitialise()
    {
        GetUnit.DeactivateUndying();
        base.Deinitialise();
    }

    #endregion

    public Ununaliving() : base() { }
    public Ununaliving(int turns, float baseResistance, bool isPermanent) : base(turns, baseResistance, isPermanent)
    {
    }
}