public class Modal : Enhancer
{
    #region Protected Override Methods

    protected override void Awake()
    {
        base.Awake();
        SetArchMinor(ArchTypeMinor.Modal);
    }

    #endregion
}