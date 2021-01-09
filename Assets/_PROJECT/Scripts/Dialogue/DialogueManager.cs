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
    public static DialogueManager instance;

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
        StartDialogue(guidedDialogues[0]);
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

            foreach (Button button in guidedDialogue.guidedButton.transform.parent.GetComponentsInChildren<Button>())
            {
                if(button != guidedDialogue.guidedButton)
                {
                    button.interactable = false;
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

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        Debug.Log("End of conversations");
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