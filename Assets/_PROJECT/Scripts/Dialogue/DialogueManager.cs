using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Animator animator;
    public TMP_Text name;
    public TMP_Text dialogueText;
    private Queue<string> sentences;
    public List<GuidedDialogue> guidedDialogues = new List<GuidedDialogue>();
    public GameObject buttonParent;
    public static DialogueManager instance;
    public TutorialPhase tutorialPhase;

    #region Singleton
    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    #endregion

    private void Start()
    {
        sentences = new Queue<string>();
        tutorialPhase = DatabaseManager.instance.GetTutorialPhase();
        //DatabaseManager.instance.SaveTutorialState(tutorialPhase);

        switch (tutorialPhase)
        {
            case TutorialPhase.guideToMap:
                guideToMap();
                return;

            case TutorialPhase.guideToGacha:
                guideToGacha();
                return;

            case TutorialPhase.guideToParty:
                guideToParty();
                return;

            case TutorialPhase.guideFinish:
                return;
        }

    }

    public void StartDialogue(GuidedDialogue guidedDialogue)
    {
        if (guidedDialogue.dialogueInfo.isTriggered) return;

        //animate the UI to pop up
        animator.SetBool("isOpen", true);

        //if got button to be guided, highlight such button.
        if(guidedDialogue.guidedButton != null)
        {
            guidedDialogue.guidedButton.GetComponent<Image>().color = Color.red;

            foreach (Button button in buttonParent.GetComponentsInChildren<Button>())
            {
                if(button != guidedDialogue.guidedButton)
                {
                    button.interactable = false;
                    button.GetComponent<Image>().color = Color.white;
                }
                else
                {
                    button.interactable = true;
                }
            }
        }

        guidedDialogue.dialogueInfo.isTriggered = true;
        name.text = guidedDialogue.dialogueInfo.name;
        sentences.Clear();

        foreach (string sentence in guidedDialogue.dialogueInfo.dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    private void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }

    [ContextMenu("Unlock")]
    public void unlockButtons()
    {
        foreach (Button button in buttonParent.GetComponentsInChildren<Button>())
        {       
            button.GetComponent<Image>().color = Color.white;          
            button.interactable = true;           
        }
    }

    private void resetTrigger(GuidedDialogue guidedDialogue)
    {
        guidedDialogue.dialogueInfo.isTriggered = false;
        guidedDialogue.guidedButton.GetComponent<DialogueTrigger>().dialogue.dialogueInfo.isTriggered = false;
    }

    [ContextMenu("Guide To Map")]
    public void guideToMap()
    {
        resetTrigger(guidedDialogues[0]);
        StartDialogue(guidedDialogues[0]);
    }

    [ContextMenu("Guide to Gacha")]
    public void guideToGacha()
    {
        resetTrigger(guidedDialogues[1]);
        StartDialogue(guidedDialogues[1]);
    }

    [ContextMenu("Guide to Party")]
    public void guideToParty()
    {
        resetTrigger(guidedDialogues[2]);
        StartDialogue(guidedDialogues[2]);
    }

    public void testButton()
    {
        tutorialPhase = TutorialPhase.guideToGacha;
        DatabaseManager.instance.SaveTutorialState(tutorialPhase);
        guideToGacha();
    }
}

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(3,10)]
    public string[] sentences; 
}

[System.Serializable] 
public class GuidedDialogue
{
    public DialogueInfo dialogueInfo;
    public Button guidedButton;
}

[System.Serializable]
public enum TutorialPhase
{
    guideToMap,
    guideToGacha,
    guideToParty,
    guideFinish
}
