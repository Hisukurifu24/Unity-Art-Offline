using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Dialog dialog;
    public bool battle;
    public string enemyName;
    public Reward reward;

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            TriggerDialog();
            if (!battle) {
                collision.GetComponent<Player>().AddItem(reward.item);
                collision.GetComponent<Player>().AddGold(reward.gold);
                collision.GetComponent<Player>().AddExp(reward.exp);
            }
        }
    }

    public void TriggerBattle() {
        if (battle) {
            GameManager.instance.StartBattle(enemyName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            FindObjectOfType<DialogManager>().Interrupt();
        }
    }
}
