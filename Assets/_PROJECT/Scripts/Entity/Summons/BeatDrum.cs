public class BeatDrum : Summon
{
    protected override void Awake()
    {
        base.Awake();
        SetIsPlayer(true);
    }
}
