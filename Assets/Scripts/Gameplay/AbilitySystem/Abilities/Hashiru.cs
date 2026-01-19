using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hashiru : Ability {
    PlayerController p;

    private void Start() {
        p = GetComponent<PlayerController>();
    }

    public override void Activate() {
        p.speed *= 3;
    }

    public override void BeginCooldown() {
        p.speed /= 3;
    }
}
