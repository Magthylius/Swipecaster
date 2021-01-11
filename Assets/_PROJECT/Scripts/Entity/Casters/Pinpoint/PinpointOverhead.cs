public class PinpointOverhead : Pinpoint
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Pinpoint_Overhead);
        SetDefaultProjectile(new Overhead(this));
    }

    #endregion
}