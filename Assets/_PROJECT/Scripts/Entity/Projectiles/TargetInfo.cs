using System.Collections.Generic;

public struct TargetInfo
{
    private Entity _focusTarget;
    private List<Entity> _collateralTargets;
    private List<Entity> _grazedTargets;

    public Entity Focus => _focusTarget;
    public List<Entity> Collateral => _collateralTargets;
    public List<Entity> Grazed => _grazedTargets;
    public static TargetInfo Null => new TargetInfo(null, new List<Entity>(), new List<Entity>());

    public TargetInfo(Entity focusTarget, List<Entity> collateralTargets, List<Entity> grazedTargets)
    {
        _focusTarget = focusTarget;
        _collateralTargets = collateralTargets;
        _grazedTargets = grazedTargets;
    }
}