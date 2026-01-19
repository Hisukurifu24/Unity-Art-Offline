using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : Ability {

    private GameObject punchPrefab;
    private AudioClip hitSound;
    private Vector2 pivot;

    private bool leftPunch;
    private float swingSpeed;

    protected override void Awake() {
        base.Awake();
        punchPrefab = Resources.Load<GameObject>("Abilities/Prefabs/Punch");
        hitSound = Resources.Load<AudioClip>("Sounds/Hit");
    }

    private void Start() {
        //Calcolo cooldown basato sull'ats
        data.cooldownTime = 0.1f / GetComponentInParent<Player>().GetCurrentStat(Stat.AttackSpeed);

        leftPunch = false;
        swingSpeed = 1000;
    }

    public override void Activate() {
        //Calcolo pivot del pugno
        pivot = (Vector2)transform.position + new Vector2(0, 0.25f);

        //Spawno il pugno
        GameObject punch = Instantiate(punchPrefab, transform);
        punch.GetComponentInChildren<DamageDealer>().Set(player.WhatIsEnemy(), data.damageAmount, data.damageType, data.hitSound);

        //Calcolo la rotazione
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Vector2.SignedAngle(Vector2.up, mousePos - pivot);

        //Debug.DrawLine(transform.position, pivot, Color.magenta, 3);
        //Debug.DrawLine(transform.position, mousePos, Color.yellow, 3);

        //Posiziono il pugno per iniziare a ruotare
        punch.transform.RotateAround(pivot, Vector3.forward, leftPunch ? angle - 45 : angle + 45);

        //Effettuo la rotazione
        StartCoroutine(SwingAnimation(punch));

        //Cambio pugno
        leftPunch = !leftPunch;
    }

    public override void BeginCooldown() { }

    private IEnumerator SwingAnimation(GameObject obj) {
        float time = data.activeTime;
        while (time > 0) {
            obj.transform.RotateAround(pivot, leftPunch ? Vector3.back : Vector3.forward, swingSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(obj);
    }
}
