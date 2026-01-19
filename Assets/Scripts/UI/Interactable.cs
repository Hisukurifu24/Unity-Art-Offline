using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Dialog dialog;
    public bool battle;
    public string enemyName;
    public Reward reward;

    private bool interacted = false;
    private Player player;

    public void TriggerDialog()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Entered");
        if (collision.tag == "Player" && !interacted)
        {
            interacted = true;
            player = collision.GetComponent<Player>();
            FindObjectOfType<DialogManager>().StartDialog(dialog);
            FindObjectOfType<DialogManager>().onDialogEnded.AddListener(GiveReward);
        }
    }

    private void GiveReward()
    {
        FindObjectOfType<DialogManager>().onDialogEnded.RemoveListener(GiveReward);
        foreach (Item i in reward.items)
        {
            player.AddItem(i);
        }
        player.AddGold(reward.gold);
        player.AddExp(reward.exp);
    }

    /*
    public void TriggerBattle() {
        if (battle) {
            GameManager.instance.StartBattle(enemyName);
        }
    }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<DialogManager>().Interrupt();
        }
    }
}
