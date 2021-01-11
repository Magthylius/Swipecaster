public class PinpointPiercer : Pinpoint
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Pinpoint_Piercer);
        SetDefaultProjectile(new Piercing(this));
    }

    #endregion
}