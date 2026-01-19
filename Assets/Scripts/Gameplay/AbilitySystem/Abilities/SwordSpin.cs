using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpin : Ability {
    private GameObject punchPrefab;
    private Vector2 pivot;
    private float rotateSpeed;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/SwordSlash");
    }

    private void Start() {
        rotateSpeed = 1500 * player.GetCurrentStat(Stat.AttackSpeed); 
    }

    public override void Activate() {
        //Spawno il pugno
        GameObject tsuki = Instantiate(punchPrefab, transform);
        tsuki.GetComponentInChildren<DamageDealer>().Set(player.WhatIsEnemy(), data.damageAmount, data.damageType, data.hitSound);

        //Effettuo il pugno
        StartCoroutine(RotateAnimation(tsuki));
    }

    private IEnumerator RotateAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            obj.transform.position = transform.position;
            pivot = transform.position + new Vector3(0, 0.25f, 0);
            obj.transform.RotateAround(pivot, Vector3.back, rotateSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }

    public override void BeginCooldown() { }

}
