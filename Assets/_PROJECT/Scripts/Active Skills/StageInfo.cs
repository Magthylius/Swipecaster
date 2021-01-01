using System.Collections.Generic;
using UnityEngine;

public struct StageInfo
{
    private List<Transform> _casterSet;
    private List<Transform> _enemySet;
    private List<Transform> _casterEntitySet;
    private List<Transform> _enemyEntitySet;

    public List<Transform> CasterSet => _casterSet;
    public List<Transform> EnemySet => _enemySet;
    public List<Transform> CasterEntitySet => _casterEntitySet;
    public List<Transform> EnemyEntitySet => _enemyEntitySet;

    public StageInfo(List<Transform> casterSet, List<Transform> enemySet, List<Transform> casterEntitySet, List<Transform> enemyEntitySet)
    {
        _casterSet = casterSet;
        _enemySet = enemySet;
        _casterEntitySet = casterEntitySet;
        _enemyEntitySet = enemyEntitySet;
    }
}