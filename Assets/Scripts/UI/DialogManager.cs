using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Animator animator;

    public Text nameText;
    public Text dialogueText;

    private Queue<string> sentences;

    private void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialog(Dialog dialog)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialog.name;

        sentences.Clear();

        foreach(string s in dialog.senteces)
        {
            sentences.Enqueue(s);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string s = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(s));
    }

    private void EndDialog()
    {
        animator.SetBool("IsOpen", false);
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char c in sentence)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
