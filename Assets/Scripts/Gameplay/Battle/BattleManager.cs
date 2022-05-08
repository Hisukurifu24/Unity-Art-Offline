using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {
    public GameObject menu1;
    public GameObject menu2;

    public Player player;
    public Player enemy;

    private int turn = 0;

    private bool bossBattle;

    public BattleLog battleLog;

    private readonly float defaultTime = 5f;
    private float turnTimeLeft;
    private bool timePaused;

    private enum Phase {
        Fight,
        Animation,
        EndTurn,
        Win,
        Lose
    }
    private Phase currentPhase;

    private enum Action {
        Attack,
        PowerUp,
        Defend
    }
    private Action lastPlayerAction;
    private Action lastEnemyAction;

    private void Awake() {
        GameObject p1 = GameManager.instance.GetEntity("Character");
        p1.name = "Character";
        GameObject p2 = GameManager.instance.GetEntity(PlayerPrefs.GetString("enemyName"));
        p2.name = GameManager.instance.GetEntity(PlayerPrefs.GetString("enemyName")).name;
        player = Instantiate(p1).GetComponent<Player>();
        enemy = Instantiate(p2).GetComponent<Player>();
    }

    private void Start() {
        /** Old Spell System
        Text[] spellsText = menu2.transform.Find("Spells").GetComponentsInChildren<Text>();
        List<SpellInfo> playerSpells = player.GetSpellbookInfo();
        for (int i = 0; i < spellsText.Length; i++) {
            if (i + 1 < playerSpells.Count && playerSpells[i + 1] != null) {
                spellsText[i].text = playerSpells[i + 1].spellName;
            }
            else {
                spellsText[i].text = "NON DISPONIBILE";
            }
        }

        player.SetEnemy(enemy);
        enemy.SetEnemy(player);
        */
        turnTimeLeft = defaultTime;
    }

    private void Update() {
        switch (currentPhase) {
            case Phase.Fight:
                if (turn == 0) {
                    //Wait for player input
                }
                else {
                    // Enemy turn
                    /** Old Spell System
                    List<SpellInfo> enemySpells = enemy.GetSpellbookInfo();
                    float max = 0;
                    int i = 0;
                    foreach (SpellInfo s in enemySpells) {
                        if (s.cost > max && s.cost <= enemy.Mana && !enemy.IsOnCooldown(enemySpells.IndexOf(s))) {
                            max = s.cost;
                            i = enemySpells.IndexOf(s);
                        }
                    }
                    if (!enemy.Cast(i)) {
                        //Something went wrong, lets try again
                        Debug.LogError("Something went wrong here, chief");
                        break;
                    }
                    */
                    bool ok = false;
                    if (lastEnemyAction == Action.PowerUp) {
                        ok = enemy.Attack(player);
                        lastEnemyAction = Action.Attack;
                    }
                    else {
                        if (enemy.GetCurrentStats().mana >= 150) {
                            ok = enemy.PowerUp();
                            lastEnemyAction = Action.PowerUp;
                        }
                        else if (player.GetCurrentStats().hp < enemy.GetCurrentStats().hp) {
                            if (lastPlayerAction == Action.PowerUp) {
                                ok = enemy.Defend();
                                lastEnemyAction = Action.Defend;
                            }
                            else {
                                ok = enemy.Attack(player);
                                lastEnemyAction = Action.Attack;
                            }
                        }
                        else {
                            ok = enemy.Defend();
                            lastEnemyAction = Action.Defend;
                        }
                    }
                    if (ok) {
                        currentPhase = Phase.Animation;
                    }
                } 
                break;
            case Phase.Animation:
                PauseTime();
                if (!player.isAnimating && !enemy.isAnimating) {
                    currentPhase = Phase.EndTurn;
                }
                break;
            case Phase.EndTurn:
                if (player.GetCurrentStats().hp <= 0) {
                    currentPhase = Phase.Lose;
                    break;
                }
                if (enemy.GetCurrentStats().hp <= 0) {
                    currentPhase = Phase.Win;
                    break;
                }

                turn = (turn + 1) % 2;
                menu2.SetActive(turn == 0);

                float playerSpeed = player.GetCurrentStats().ms * player.GetCurrentStats().ats;
                float enemySpeed = enemy.GetCurrentStats().ms * enemy.GetCurrentStats().ats;
                float timeReduction = turn == 0 ? (enemySpeed - playerSpeed) / 100 : (playerSpeed - enemySpeed) / 100;
                timeReduction = Mathf.Clamp(timeReduction, 0, defaultTime-0.25f);
                turnTimeLeft = defaultTime - timeReduction;

                ResumeTime();
                currentPhase = Phase.Fight;
                // Altri effetti?
                break;
            case Phase.Win:
                battleLog.AddText(player.name + " vince!", Color.black);
                SceneManager.LoadScene("Floor" + GameManager.ReachedFloor);
                break;
            case Phase.Lose:
                battleLog.AddText(player.name + " perde!", Color.black);
                SceneManager.LoadScene("GameOver");
                break;
            default:
                Debug.LogError("Idk");
                break;
        }
    }

    private IEnumerator TurnTime() {
        while (true) {
            yield return new WaitForSeconds(0.01f);
            if (!timePaused) {
                turnTimeLeft -= 0.01f;
            }
            if (turnTimeLeft <= 0) {
                battleLog.AddText((turn == 0 ? player.name : enemy.name) + " è stato troppo lento!!!", Color.yellow);
                currentPhase = Phase.EndTurn;
            }
        }
    }
    private void PauseTime() {
        timePaused = true;
    }
    private void ResumeTime() {
        timePaused = false;
    }

    private void StartFight() {
        currentPhase = Phase.Fight;

        float playerSpeed = player.GetCurrentStats().ats * player.GetCurrentStats().ms;
        float enemySpeed = enemy.GetCurrentStats().ats * enemy.GetCurrentStats().ms;

        turn = playerSpeed > enemySpeed ? 0 : 1;
        menu2.SetActive(turn == 0 ? true : false);

        StartCoroutine(TurnTime());
    }

    private IEnumerator TryToRun() {
        float playerSpeed = player.GetCurrentStats().ats * player.GetCurrentStats().ms;
        float enemySpeed = enemy.GetCurrentStats().ats * enemy.GetCurrentStats().ms;
        if (bossBattle) {
            battleLog.AddText("Non puoi scappare da questa battaglia!", Color.black);
            yield return new WaitForSeconds(1);
            StartFight();
        }
        if (playerSpeed > enemySpeed) {
            battleLog.AddText(player.name + " sta scappando a gambe levate!", Color.black);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Floor" + GameManager.ReachedFloor);
        }
        else if (enemySpeed > playerSpeed) {
            battleLog.AddText(player.name + " sta provando a fuggire!", Color.black);
            yield return new WaitForSeconds(1);
            battleLog.AddText(enemy.name + ": dove pensavi di andare?", Color.black);
            yield return new WaitForSeconds(1);
            StartFight();
        }
        else {
            battleLog.AddText(player.name + " sta provando a fuggire!", Color.black);
            yield return new WaitForSeconds(1);
            float r = Random.Range(0, 1f);
            if (r < .5f) {
                battleLog.AddText("E ce la fa!", Color.black);
                yield return new WaitForSeconds(1);
                SceneManager.LoadScene("Floor" + GameManager.ReachedFloor);
            }
            else {
                battleLog.AddText("Ma inciampa!!", Color.black);
                yield return new WaitForSeconds(1);
                StartFight();
            }
        }
    }

    public void AddLog(string text, Color c) {
        battleLog.AddText(text, c);
    }

    public float GetTurnTimeLeft() {
        return turnTimeLeft;
    }

    public void Menu1(bool run) {
        menu1.SetActive(false);
        if (!run) {
            StartFight();
        }
        else {
            //Try to run
            StartCoroutine(TryToRun());
        }
    }

    public void Menu2(int choice) {
        /*
        if (!player.Cast(choice)) {
            //BadActivation
            return;
        }
        */
        bool ok = false;
        switch (choice) {
            case 0:
                ok = player.Attack(enemy);
                lastPlayerAction = Action.Attack;
                break;
            case 1:
                ok = player.PowerUp();
                lastPlayerAction = Action.PowerUp;
                break;
            case 2:
                ok = player.Defend();
                lastPlayerAction = Action.Defend;
                break;
            default:
                break;
        }
        if (!ok) {
            battleLog.AddText(player.name + " non ha abbastanza mana!", Color.cyan);
            return;
        }
        currentPhase = Phase.Animation;
        menu2.SetActive(false);
    }
}
