using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    private Animator animator;
    [SerializeField] ToggleGroup inventoryGroup;
    [SerializeField] ToggleGroup bagGroup;
    [SerializeField] Player player;

    private int currentPage = 0;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        /*
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        player.AddItem(GameManager.instance.GetRandomItem());
        */
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            OpenClose();
        }
        UpdateUI();
    }

    private void UpdateUI() {
        Toggle[] invToggles = inventoryGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < 6; i++) {
            Item item = player.GetInventoryItem(i);
            Image icon = invToggles[i].transform.Find("Icon").GetComponent<Image>();
            Text name = invToggles[i].GetComponentInChildren<Text>();
            if (item != null) {
                icon.enabled = true;
                icon.sprite = item.icon;
                name.text = item.itemName;
            }
            else {
                icon.enabled = false;
                name.text = "Slot Vuoto";
            }
        }

        Toggle[] bagToggles = bagGroup.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < 10; i++) {
            Item item = player.GetBagItem(i + 10 * currentPage);
            Image icon = bagToggles[i].transform.Find("Icon").GetComponent<Image>();
            Text name = bagToggles[i].GetComponentInChildren<Text>();
            if (item != null) {
                icon.enabled = true;
                icon.sprite = item.icon;
                name.text = item.itemName;
            }
            else {
                icon.enabled = false;
                name.text = "Slot Vuoto";
            }
        }
        bagGroup.transform.Find("SwitchPage").GetComponentInChildren<Text>().text = "Page: " + (currentPage + 1) + "/10";
    }

    public void OpenClose() {
        animator.SetBool("IsOpen", !animator.GetBool("IsOpen"));
    }

    public void NextPage() {
        currentPage = (currentPage + 1) % 10;
    }
    public void PreviousPage() {
        currentPage--;
        if (currentPage < 0) {
            currentPage = 9;
        }
    }

    public void Swap() {
        int inv = inventoryGroup.GetFirstActiveToggle().transform.GetSiblingIndex();
        int bag = bagGroup.GetFirstActiveToggle().transform.GetSiblingIndex() + 10 * currentPage;

        player.Swap(inv, bag);
    }
}
