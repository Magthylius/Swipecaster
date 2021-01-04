using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Quest/Room Object")]
public class RoomObject : ScriptableObject
{
    [Header("Enemies")]
    public List<EnemyData> enemies;
    public int maxEnemySize;
    public bool isRandom;
}

[System.Serializable]
public struct EnemyData
{
    public UnitObject enemySO;
    public int level;

    public EnemyData(UnitObject enemySO, int level)
    {
        this.enemySO = enemySO;
        this.level = level;
    }
}