using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Dialog dialog;
    public Reward reward;

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        TriggerDialog();
        collision.GetComponent<Player>().AddItem(reward.item);
        collision.GetComponent<Player>().AddGold(reward.gold);
        collision.GetComponent<Player>().AddExp(reward.exp);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        FindObjectOfType<DialogManager>().EndDialog();
    }
}
