public class ArtilleryOverhead : Artillery
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Artillery_Overhead);
        SetDefaultProjectile(new SplashOverhead(this));
    }

    #endregion
}