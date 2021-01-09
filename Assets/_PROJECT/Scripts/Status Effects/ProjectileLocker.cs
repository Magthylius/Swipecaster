using UnityEngine;

public class ProjectileLocker : StatusTemplate<ProjectileLocker>
{
    #region Variables and Properties

    private Projectile _projectile;
    private Projectile ProjectileToLock => _projectile;
    public override string StatusName => "Projectile Lock";

    #endregion

    #region Override Methods

    public override void DoImmediateEffect(TargetInfo info)
    {
        GetUnit.SetProjectileLock(false);
        GetUnit.SubstituteProjectile(ProjectileToLock);
        GetUnit.SetProjectileLock(true);
    }
    protected override void Deinitialise()
    {
        GetUnit.SetProjectileLock(false);
        base.Deinitialise();
    }

    #endregion

    public ProjectileLocker() : base()
    {
        _projectile = null;
    }
    public ProjectileLocker(int turns, float baseResistance, bool isPermanent, Projectile projectile) : base(turns, baseResistance, isPermanent)
    {
        _projectile = projectile;
    }
}