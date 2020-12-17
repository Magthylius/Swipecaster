using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Unit/Unit Object")]
public class UnitObject : ScriptableObject
{
    [Header("General")]
    public string ID;
    [Range(3, 5)] public int BaseRarity;
    public int MaxLevel;
    public int MaxHealth;
    public int MaxAttack;
    public int MaxDefence;
    public string CharacterDescription;

    [Header("Skill")]
    //public Skill UnitSkill;
    [TextArea(1, 5)] public string SkillDescription;

    [Header("UI/Visual")]
    public GameObject FullArtPrefab;
    public GameObject SpriteHolderPrefab;
    public Sprite PortraitArt;
}
