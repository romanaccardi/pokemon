using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public const int DIALOGUE_TIME = 1; // TODO
    public Queue<string> dialogueQueue;
    public Text dialogueText;

    private bool waiting = false;

    void Awake()
    {
        instance = this;

        dialogueQueue = new Queue<string>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!waiting)
        {
            StartCoroutine(cycleDialogue());
        }

    }

    public void addToQueue(string message)
    {
        dialogueQueue.Enqueue(message);
    }

    IEnumerator cycleDialogue()
    {
        if( dialogueQueue.Count > 0 )
        {
            waiting = true;
            string newMessage = dialogueQueue.Dequeue();

            dialogueText.text = newMessage;

            yield return new WaitForSeconds(DIALOGUE_TIME);
            dialogueText.text = "";
            waiting = false;
        }


    }

    public void pauseDialogue()
    {
        waiting = true;
    }

    public void startDialogue()
    {
        waiting = false;
    }

    public bool isEmpty()
    {
        return dialogueText.text == "" && dialogueQueue.Count == 0;
    }


}
