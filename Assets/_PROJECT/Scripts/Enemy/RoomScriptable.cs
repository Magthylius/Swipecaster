using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit/Room Object")]
public class RoomScriptable : ScriptableObject
{
    [Header("Enemies")]

    public List<EnemyData> enemies;
   // [Header("Skill")]
    //public Skill UnitSkill;
    //[TextArea(1, 5)] public string SkillDescription;

  //  [Header("UI/Visual")]
 /*   public GameObject FullArtPrefab;
    public GameObject SpriteHolderPrefab;
    public Sprite PortraitArt;*/
}

[System.Serializable]
public struct EnemyData
{
    public UnitObject enemy;
    public int level;

    public EnemyData(UnitObject enemy, int level)
    {
        this.enemy = enemy;
        this.level = level;
    }
}