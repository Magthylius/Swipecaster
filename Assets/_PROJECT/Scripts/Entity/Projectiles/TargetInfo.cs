using System.Collections.Generic;

public struct TargetInfo
{
    private Unit _focusTarget;
    private List<Unit> _collateralTargets;
    private List<Unit> _grazedTargets;
    private List<Unit> _allCasters;
    private List<Unit> _allFoes;

    public Unit Focus => _focusTarget;
    public List<Unit> Collateral => _collateralTargets;
    public List<Unit> Grazed => _grazedTargets;
    public List<Unit> Casters => _allCasters;
    public List<Unit> Foes => _allFoes;
    public static TargetInfo Null => new TargetInfo(null, new List<Unit>(), new List<Unit>());

    public TargetInfo(Unit focusTarget, List<Unit> collateralTargets, List<Unit> grazedTargets, List<Unit> allCasters = null, List<Unit> allFoes = null)
    {
        _focusTarget = focusTarget;
        _collateralTargets = collateralTargets;
        _grazedTargets = grazedTargets;
        _allCasters = allCasters;
        _allFoes = allFoes;
    }
}