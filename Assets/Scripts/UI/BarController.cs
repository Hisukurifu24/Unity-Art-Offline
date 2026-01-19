using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {
    [SerializeField] private Player p;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar manaBar;
    [SerializeField] private ProgressBar expBar;
    [SerializeField] private Text lvl;
    [SerializeField] private Text exp;
    [SerializeField] private Text gold;

    private void Start() {
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update() {
        healthBar.SetValues(p.GetCurrentStat(Stat.Health), p.GetMaxHp());
        manaBar.SetValues(p.GetCurrentStat(Stat.Mana), p.GetMaxMana());
        expBar.SetValues(p.GetExp(), p.GetMaxExp());
        lvl.text = "Level: " + p.GetLevel();
        exp.text = "Exp: " + p.GetExp() + "/" + p.GetMaxExp();
        gold.text = "Gold: " + p.GetGold();
    }
}
