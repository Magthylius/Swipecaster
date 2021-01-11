public class Debuffer : Antipathic
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Debuffer);
    }

    #endregion
}
