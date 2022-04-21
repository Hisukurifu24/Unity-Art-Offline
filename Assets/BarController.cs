using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour {
    [SerializeField] private BattleManager bm;
    [SerializeField] private Text nameP1;
    [SerializeField] private Text nameP2;
    [SerializeField] private TMPro.TextMeshProUGUI time;
    [SerializeField] private ProgressBar healthBarP1;
    [SerializeField] private ProgressBar manaBarP1;
    [SerializeField] private ProgressBar healthBarP2;
    [SerializeField] private ProgressBar manaBarP2;
    //[SerializeField] private ProgressBar baseAttackCD;
    //[SerializeField] private ProgressBar spell1CD;
    //[SerializeField] private ProgressBar spell2CD;
    //[SerializeField] private ProgressBar spell3CD;
    //[SerializeField] private ProgressBar spell4CD;

    private Player p1;
    private Player p2;

    private void Start() {
        p1 = bm.player;
        p2 = bm.enemy;

        healthBarP1.SetTargetValue(p1.GetMaxHp());
        manaBarP1.SetTargetValue(p1.GetMaxMana());
        healthBarP2.SetTargetValue(p2.GetMaxHp());
        manaBarP2.SetTargetValue(p2.GetMaxMana());
        //baseAttackCD.SetTargetValue(p1.AttackSpeed);
        //spell1CD.SetTargetValue(p1.GetCooldown(1));
        //spell2CD.SetTargetValue(p1.GetCooldown(2));
        //spell3CD.SetTargetValue(p1.GetCooldown(3));
        //spell4CD.SetTargetValue(p1.GetCooldown(4));
        nameP1.text = "(" + p1.Level + ") " + p1.name;
        nameP2.text = p2.name + " (" + p2.Level + ")";
    }

    private void Update() {
        healthBarP1.SetValue(p1.GetCurrentStats().hp);
        manaBarP1.SetValue(p1.GetCurrentStats().mana);
        healthBarP2.SetValue(p2.GetCurrentStats().hp);
        manaBarP2.SetValue(p2.GetCurrentStats().mana);
        //baseAttackCD.SetValue(p1.GetCurrentCooldown(0));
        //spell1CD.SetValue(p1.GetCurrentCooldown(1));
        //spell2CD.SetValue(p1.GetCurrentCooldown(2));
        //spell3CD.SetValue(p1.GetCurrentCooldown(3));
        //spell4CD.SetValue(p1.GetCurrentCooldown(4));
        time.text = "Time: " + bm.GetTurnTimeLeft().ToString("F2") + "s";
    }
}
