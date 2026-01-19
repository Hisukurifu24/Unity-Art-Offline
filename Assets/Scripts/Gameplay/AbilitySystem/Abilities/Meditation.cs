using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Meditation : Ability {

    private GameObject prefab;
    private float healPS = 20;
    private int resPC = 50;
    private Stats buff;
    private PlayerController pc;

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        pc = GetComponentInParent<PlayerController>();
        prefab = Resources.Load<GameObject>("Abilities/Prefabs/Meditation");
        buff = new Stats();
    }

    public override void Activate() {
        buff.Set(Stat.Armor, player.GetCurrentStat(Stat.Armor) / 100 * resPC);
        buff.Set(Stat.MagicResistance, player.GetCurrentStat(Stat.MagicResistance) / 100 * resPC);

        pc.BlockPlayer();
        pc.moveCommand = Vector2.zero;
        player.Buff(buff, data.activeTime);

        //Spawno il prefab
        GameObject p = Instantiate(prefab, transform);

        //Effettuo la meditazione
        StartCoroutine(Animation(p));
        StartCoroutine(Heal());
    }

    public override void BeginCooldown() {
        pc.UnlockPlayer();
    }

    private IEnumerator Animation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }

    private IEnumerator Heal() {
        float time = data.activeTime;
        while (time > 0) {
            player.Heal(healPS);
            time -= 1;
            yield return new WaitForSeconds(1);
        }
    }
}
