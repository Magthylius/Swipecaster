using System.Collections.Generic;

public struct TargetInfo
{
    private Unit _focusTarget;
    private List<Unit> _collateralTargets;
    private List<Unit> _grazedTargets;

    public Unit Focus => _focusTarget;
    public List<Unit> Collateral => _collateralTargets;
    public List<Unit> Grazed => _grazedTargets;
    public static TargetInfo Null => new TargetInfo(null, new List<Unit>(), new List<Unit>());

    public TargetInfo(Unit focusTarget, List<Unit> collateralTargets, List<Unit> grazedTargets)
    {
        _focusTarget = focusTarget;
        _collateralTargets = collateralTargets;
        _grazedTargets = grazedTargets;
    }
}