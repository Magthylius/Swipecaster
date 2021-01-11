public class Enhancer : Caster
{
    protected override void Awake()
    {
        base.Awake();
        SetArchMajor(ArchTypeMajor.Enhancer);
    }
}
