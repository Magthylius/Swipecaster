using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LerpFunctions;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue")]
    public RectTransform dialogueRectTr;
    public TMP_Text dialogueName;
    public TMP_Text dialogueText;
    public Image speakerSpeaker;

    [Header("Settings")]
    public float transitionSpeed = 15f;

    [Header("Sentences")]
    private Queue<string> sentences;
    public List<GuidedDialogue> guidedDialogues = new List<GuidedDialogue>();
    public GameObject buttonParent;
    public TutorialPhase tutorialPhase;

    FlexibleRect dialogueFR;

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

        if (DatabaseManager.instance != null)
            tutorialPhase = DatabaseManager.instance.GetTutorialPhase();
        // DatabaseManager.instance.SaveTutorialState(tutorialPhase);

        dialogueFR = new FlexibleRect(dialogueRectTr);
        dialogueFR.MoveTo(dialogueFR.GetBodyOffset(Vector2.down));

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

    private void Update()
    {
        if(Input.GetKeyDown("a"))
        {
            unlockButtons();
        }

        dialogueFR.Step(transitionSpeed * Time.unscaledDeltaTime);
    }

    public void StartDialogue(GuidedDialogue guidedDialogue)
    {
        if (guidedDialogue.dialogueInfo.isTriggered) return;

        //animate the UI to pop up
        //animator.SetBool("isOpen", true);
        
        dialogueFR.StartLerp(dialogueFR.originalPosition);

        //if got button to be guided, highlight such button.
        if (guidedDialogue.guidedButton != null)
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
        dialogueName.text = guidedDialogue.dialogueInfo.dialogue.dialogueName;

        if(guidedDialogue.dialogueInfo.speakerSprite)
            speakerSpeaker.sprite = guidedDialogue.dialogueInfo.speakerSprite;

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
        //animator.SetBool("isOpen", false);
        //dialogueFR.StartLerp(dialogueFR.originalPosition);
        dialogueFR.StartLerp(dialogueFR.GetBodyOffset(Vector2.down));
    }

    private void resetTrigger(GuidedDialogue guidedDialogue)
    {
        guidedDialogue.dialogueInfo.isTriggered = false;

        if(guidedDialogue.guidedButton) 
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
        if(CurrencyManager.instance.PremiumCurrency < 5)
        {
            CurrencyManager.instance.AddCurrency(CurrencyType.PREMIUM_CURRENCY, 5 - CurrencyManager.instance.PremiumCurrency);
        }
        resetTrigger(guidedDialogues[1]);
        StartDialogue(guidedDialogues[1]);
    }

    [ContextMenu("Guide to Party")]
    public void guideToParty()
    {
        if (tutorialPhase == TutorialPhase.guideFinish) return;
        tutorialPhase = TutorialPhase.guideToParty;
        DatabaseManager.instance.SaveTutorialState(tutorialPhase);
        resetTrigger(guidedDialogues[2]);
        StartDialogue(guidedDialogues[2]);
    }

    [ContextMenu("Unlock")]
    public void unlockButtons()
    {
        tutorialPhase = TutorialPhase.guideFinish;
        DatabaseManager.instance.SaveTutorialState(tutorialPhase);
        foreach (Button button in buttonParent.GetComponentsInChildren<Button>())
        {
            button.GetComponent<Image>().color = Color.white;
            button.interactable = true;
        }
    }

}

[System.Serializable]
public class Dialogue
{
    public string dialogueName;

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
