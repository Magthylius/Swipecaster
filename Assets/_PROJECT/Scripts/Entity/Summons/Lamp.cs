public class Lamp : Summon
{
    private Unit _latestDamager = null;

    protected override void TakeDamage(Unit damager, int damageAmount)
    {
        base.TakeDamage(damager, damageAmount);

        if (_latestDamager == damager) return;
        _latestDamager = damager;
        damager.TakeHit(damager, Round(damageAmount * GetReboundPercent));
    }

    protected override void EndTurnAllMethods()
    {
        base.EndTurnAllMethods();
        _latestDamager = null;
    }

    protected override void Awake()
    {
        base.Awake();
        SetIsPlayer(true);
    }
}
