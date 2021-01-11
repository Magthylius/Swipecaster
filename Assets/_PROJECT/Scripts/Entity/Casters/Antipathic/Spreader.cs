public class Spreader : Antipathic
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetDefaultProjectile(new Blast(this));
        SetArchMinor(ArchTypeMinor.Spreader);
    }

    #endregion
}