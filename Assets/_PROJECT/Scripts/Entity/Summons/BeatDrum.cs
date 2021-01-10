public class BeatDrum : Summon
{
    protected override void OnDestroy()
    {
        base.OnDestroy();
        transform.parent.gameObject.SetActive(false);
    }

    protected override void Awake()
    {
        base.Awake();
        SetIsPlayer(true);
    }
}
