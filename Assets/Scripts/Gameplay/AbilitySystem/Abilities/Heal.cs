using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Heal : Ability {

    private GameObject prefab;
    private int healPC = 10;

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        prefab = Resources.Load<GameObject>("Abilities/Prefabs/Meditation");
    }

    public override void Activate() {
        player.Heal(player.GetMaxHp() / 100 * healPC);

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
