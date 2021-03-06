using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameTe;
    public Text dialTe;

    private Queue<string> sentences;
    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting converstaions with" + dialogue.name);

        nameTe.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count ==0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialTe.text = sentence;
    }

    void EndDialogue()
    {
        Debug.Log("End of conversation");
    }
}
