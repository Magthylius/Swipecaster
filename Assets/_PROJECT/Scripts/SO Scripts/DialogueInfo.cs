using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UI/Dialogue Info")]
public class DialogueInfo : ScriptableObject
{
    public bool isTriggered;
    public Dialogue dialogue;
    public Sprite speakerSprite;
}
