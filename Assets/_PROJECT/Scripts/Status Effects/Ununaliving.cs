public class Ununaliving : StatusTemplate<Ununaliving>
{
    #region Variables and Properties

    public override string StatusName => "Scar of the Undying";

    #endregion

    #region Override Methods

    public override void DoImmediateEffect(TargetInfo info) => GetUnit.ActivateUndying();
    protected override void Deinitialise()
    {
        GetUnit.DeactivateUndying();
        base.Deinitialise();
    }

    #endregion

    public Ununaliving() : base() { }
    public Ununaliving(int turns, float baseResistance, bool isPermanent) : base(turns, baseResistance, isPermanent) { }
}