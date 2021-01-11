public class Howitzer : Artillery
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Howitzer);
        SetDefaultProjectile(new Overhead(this));
    }

    #endregion
}