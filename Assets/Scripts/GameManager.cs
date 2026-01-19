using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static int ReachedFloor = 1;

    private Item[] allItems;
    private SpellInfo[] allSpells;

    [SerializeField] private GameObject[] entities;


    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource battleMusic;

    #region SINGLETON
    public static GameManager instance = null;
    private void Awake() {
        if (!instance) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
            return;
        }

        #endregion
        //Awake
        allItems = Resources.LoadAll<Item>("Items");
        allSpells = Resources.LoadAll<SpellInfo>("Spells");
    }

    public Item GetItem(string name) {
        foreach (Item i in allItems) {
            if (i.itemName == name) {
                return i;
            }
        }
        Debug.LogError("Oggetto " + name + " non trovato");
        return null;
    }

    public Item GetRandomItem(Rarity min = Rarity.Common, Rarity max = Rarity.Artifact) {
        foreach (Item i in allItems) {
            float r = Random.Range(0, 1f);
            if (r <= 1/((float)i.rarity)) {
                return i;
            }
        }
        return null;
    }

    public SpellInfo GetSpellInfo(string name) {
        foreach (SpellInfo i in allSpells) {
            if (i.spellName == name) {
                return i;
            }
        }
        return new SpellInfo();
    }

    public GameObject GetEntity(string name) {
        foreach (GameObject g in entities) {
            if (g.name == name) {
                return g;
            }
        }
        return null;
    }

    private void Start() {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void StartBattle(string enemyName) {
        PlayerPrefs.SetString("enemyName", enemyName);
        bgm.Stop();
        battleMusic.Play();
        SceneLoader.instance.LoadLevel("Battle");
    }
}
