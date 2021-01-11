public class Kinectist : Caster
{
    private bool triggerOnce = false;
    public bool TriggerOnce { get => triggerOnce; set => triggerOnce = value; }

    protected void ResetTrigger() => triggerOnce = false;

    protected override void Awake()
    {
        base.Awake();
        SetArchMajor(ArchTypeMajor.Kinectist);
        ResetTrigger();
    }
}
