using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : Ability {
    float dashVelocity;
    PlayerController p;
    Rigidbody2D rb;

    private void Start() {
        dashVelocity = 50;
        p = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Activate() {
        rb.AddForce(p.moveCommand * dashVelocity, ForceMode2D.Impulse);
    }

    public override void BeginCooldown() { }
}
