using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialDialogues : MonoBehaviour
{
    public static TutorialDialogues Instance { get; private set; }

    [Header("Animator")]
    public Animator dialoguePop;

    [Header("UI")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI instructorDialogueText;
    private Queue<string> sentences;

    private void Awake()
    {
        #region Check null
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.sentences = this.sentences;
            Destroy(gameObject);
        }
        #endregion
    }

    private void Start()
    {
        sentences = new Queue<string>();
        dialoguePop.SetBool("IsOpenDialogue", false);
    }

    public void StartInstructorPlayer(TutorialDialoguesData dialogue)
    {
        dialoguePop.SetBool("IsOpenDialogue", true);
        playerNameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentences();
    }

    private void DisplayNextSentences()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //Prevent Player to start a next dialogue while current dialogue is playing (but there only one dialogue) 
        StopAllCoroutines();
        StartCoroutine(TypeSentences(sentence));
    }

    IEnumerator TypeSentences(string sentence)
    {
        instructorDialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            instructorDialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        dialoguePop.SetBool("IsOpenDialogue", false);
    }
}
