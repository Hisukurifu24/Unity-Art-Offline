using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {
    [SerializeField] private Player p;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private ProgressBar manaBar;
    [SerializeField] private ProgressBar expBar;
    [SerializeField] private Text lvl;
    [SerializeField] private Text exp;

    private void Update() {
        healthBar.SetValues(p.GetCurrentStats().hp, p.GetMaxHp());
        manaBar.SetValues(p.GetCurrentStats().mana, p.GetMaxMana());
        expBar.SetValues(p.Experience, p.GetMaxExp());
        lvl.text = "Level: " + p.Level;
        exp.text = "Exp: " + p.Experience + "/" + p.GetMaxExp();
    }
}
