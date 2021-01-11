using System;
using System.Collections.Generic;

public struct TargetInfo : IEquatable<TargetInfo>
{
    private Unit _focusTarget;
    private List<Unit> _collateralTargets;
    private List<Unit> _grazedTargets;
    private List<Unit> _allAllies;
    private List<Unit> _allFoes;

    public Unit Focus => _focusTarget;
    public List<Unit> Collateral => _collateralTargets;
    public List<Unit> Grazed => _grazedTargets;
    public List<Unit> Allies => _allAllies;
    public List<Unit> Foes => _allFoes;
    public static TargetInfo Null => new TargetInfo(null, new List<Unit>(), new List<Unit>());

    public TargetInfo(Unit focusTarget, List<Unit> collateralTargets, List<Unit> grazedTargets, List<Unit> allAllies = null, List<Unit> allFoes = null)
    {
        _focusTarget = focusTarget;
        _collateralTargets = collateralTargets;
        _grazedTargets = grazedTargets;
        _allAllies = allAllies;
        _allFoes = allFoes;
    }

    #region Equals and Operators

    public override bool Equals(object obj) => obj is TargetInfo info && Equals(info);
    public bool Equals(TargetInfo other)
        => EqualityComparer<Unit>.Default.Equals(Focus, other.Focus) &&
           EqualityComparer<List<Unit>>.Default.Equals(Collateral, other.Collateral) &&
           EqualityComparer<List<Unit>>.Default.Equals(Grazed, other.Grazed) &&
           EqualityComparer<List<Unit>>.Default.Equals(Allies, other.Allies) &&
           EqualityComparer<List<Unit>>.Default.Equals(Foes, other.Foes);
    public override int GetHashCode()
    {
        int hashCode = 2112851730;
        hashCode = hashCode * -1521134295 + EqualityComparer<Unit>.Default.GetHashCode(Focus);
        hashCode = hashCode * -1521134295 + EqualityComparer<List<Unit>>.Default.GetHashCode(Collateral);
        hashCode = hashCode * -1521134295 + EqualityComparer<List<Unit>>.Default.GetHashCode(Grazed);
        hashCode = hashCode * -1521134295 + EqualityComparer<List<Unit>>.Default.GetHashCode(Allies);
        hashCode = hashCode * -1521134295 + EqualityComparer<List<Unit>>.Default.GetHashCode(Foes);
        return hashCode;
    }
    public static bool operator ==(TargetInfo left, TargetInfo right) => left.Equals(right);
    public static bool operator !=(TargetInfo left, TargetInfo right) => !(left == right);

    #endregion
}