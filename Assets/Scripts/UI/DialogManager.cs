using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    public Animator animator;

    public Text nameText;
    public Text dialogueText;

    private Queue<string> sentences;
    private Queue<Dialog> dialogs;

    private void Awake() {
        sentences = new Queue<string>();
        dialogs = new Queue<Dialog>();
    }

    public void PromptMessage(string message) {
        if (animator.GetBool("IsOpen")) {
            //Se c'è già un dialogo in corso, metto in coda
            Dialog d = new Dialog();
            d.name = "Message";
            d.senteces[0] = message;
            dialogs.Enqueue(d);
            return;
        }
        animator.SetBool("IsOpen", true);

        nameText.text = "Message";

        sentences.Clear();
        sentences.Enqueue(message);
        DisplayNextSentence();
    }

    public void StartDialog(Dialog dialog) {
        if (animator.GetBool("IsOpen")) {
            //Se c'è già un dialogo in corso, metto in coda
            dialogs.Enqueue(dialog);
            return;
        }
        animator.SetBool("IsOpen", true);

        nameText.text = dialog.name;

        sentences.Clear();

        foreach (string s in dialog.senteces) {
            sentences.Enqueue(s);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialog();
            return;
        }

        string s = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(s));
    }

    public void EndDialog() {
        animator.SetBool("IsOpen", false);
        if (dialogs.Count == 0) {
            return;
        }
        StartDialog(dialogs.Dequeue());
    }

    private IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach (char c in sentence) {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
