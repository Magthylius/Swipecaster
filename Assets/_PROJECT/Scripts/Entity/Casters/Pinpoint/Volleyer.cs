public class Volleyer : Pinpoint
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Volleyer);
        SetDefaultProjectile(new Dual(this));
    }

    #endregion
}