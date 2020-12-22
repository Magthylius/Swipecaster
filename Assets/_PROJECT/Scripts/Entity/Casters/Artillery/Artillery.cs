public class Artillery : Caster
{
    protected override void Awake()
    {
        base.Awake();

        SetArchMajor(ArchTypeMajor.Artillery);
    }
}