using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public SpellInfo info;
    public Player owner;
    public Player enemy;
    public float currentCD;
    public bool onCooldown;
    public bool isAnimating;

    private void Awake() {
        owner = GetComponent<Player>();
        info = GameManager.instance.GetSpellInfo(GetType().ToString());
    }


    virtual public bool Activate() {
        //General Code
        if (onCooldown || !owner.UseMana(info.cost)) {
            return false;
        }
        StartCooldown();
        isAnimating = true;
        StartCoroutine(Animation());
        return true;
    }

    virtual protected IEnumerator Animation() {
        //General Code
        Vector3 currentPosition = owner.transform.position;
        bool goRight = owner.transform.position.x - enemy.transform.position.x <= 0;
        Vector3 moveCommand = goRight ? Vector3.right : Vector3.left;
        Vector3 movement = moveCommand * 500 * Time.deltaTime;
        float targetX = owner.transform.position.x + (enemy.transform.position.x - owner.transform.position.x) / 2;
        while (goRight ? owner.transform.position.x <= targetX : owner.transform.position.x >= targetX) {
            yield return new WaitForFixedUpdate();
            transform.Translate(movement);

        }
        movement *= -1;
        while (goRight ? owner.transform.position.x >= currentPosition.x : owner.transform.position.x <= currentPosition.x) {
            yield return new WaitForFixedUpdate();
            transform.Translate(movement);
        }
        owner.transform.position = currentPosition;
        isAnimating = false;
    }

    protected void StartCooldown() {
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown() {
        onCooldown = true;
        currentCD = info.cooldown;
        while (true) {
            yield return new WaitForSeconds(0.01f);
            currentCD -= 0.01f;
            if (currentCD <= 0) {
                onCooldown = false;
                break;
            }
        }
    }


}
