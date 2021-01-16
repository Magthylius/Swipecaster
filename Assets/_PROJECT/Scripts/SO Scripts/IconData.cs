using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconData", menuName = "Icons/IconData")]
public class IconData : ScriptableObject
{
    public string spriteName;
    public Sprite sprite;
    public Color spriteColor;
}
