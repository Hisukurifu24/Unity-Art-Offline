using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Shield : Ability {

    private GameObject prefab;
    private int resPC = 60;
    private Stats buff;

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        prefab = Resources.Load<GameObject>("Abilities/Prefabs/Shield");
        buff = new Stats();
    }

    public override void Activate() {
        buff.Set(Stat.Armor, player.GetCurrentStat(Stat.Armor) / 100 * resPC);
        buff.Set(Stat.MagicResistance, player.GetCurrentStat(Stat.MagicResistance) / 100 * resPC);

        player.Buff(buff, data.activeTime);

        //Spawno il prefab
        GameObject p = Instantiate(prefab, transform);

        //Effettuo la meditazione
        StartCoroutine(Animation(p));
    }

    public override void BeginCooldown() {
    }

    private IEnumerator Animation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }
}
