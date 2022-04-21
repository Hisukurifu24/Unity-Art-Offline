using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttack : Spell {
    private void Start() {
        //info.cooldown = owner.AttackSpeed;
    }

    public override bool Activate() {
        if (!base.Activate()) {
            return false;
        }

        //enemy.TakeDamage(owner.AttackDamage, Damage.Physical);

        return true;
    }
}
