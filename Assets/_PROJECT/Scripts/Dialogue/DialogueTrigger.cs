using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GuidedDialogue dialogue;

    public void TriggerDialogue()
    {
        if(dialogue.dialogueInfo.isTriggered) return;
        if (DialogueManager.instance.tutorialPhase == TutorialPhase.guideFinish) return;

        DialogueManager.instance.StartDialogue(dialogue);
        dialogue.dialogueInfo.isTriggered = true;
    }
}
