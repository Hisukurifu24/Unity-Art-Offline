using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    [SerializeField] GameObject aura;
    [SerializeField] GameObject shield;

    [SerializeField] private List<Item> inventory;
    //[SerializeField] private Spell[] spellbook;


    [SerializeField] private Stats basic;
    [SerializeField] private Stats current;
    [SerializeField] private Stats bonus;

    [SerializeField] private float experience = 0;
    [SerializeField] private int level = 1;

    private float hpReg = 1f;
    private float manaReg = 10f;

    public float Experience => experience;
    public int Level => level;

    public bool isAnimating;
    private bool powerUp;
    private bool defPosition;


    private void Awake() {
        //spellbook = new Spell[5];
        //LearnSpell<BaseAttack>();
    }

    private void Start() {
        current = basic;

        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());
    }

    private void Update() {
        aura.SetActive(powerUp);
        shield.SetActive(defPosition);
    }

    private IEnumerator RegenHealth() {
        while (true) {
            yield return new WaitForSeconds(1);
            Heal(hpReg);
        }
    }
    private IEnumerator RegenMana() {
        while (true) {
            yield return new WaitForSeconds(1);
            RestoreMana(manaReg);
        }
    }

    public void TakeDamage(float amount, Damage type) {
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        float damage = 0;
        switch (type) {
            case Damage.Physical:
                damage = amount - (defPosition ? current.armor * 2 : current.armor);
                break;
            case Damage.Magic:
                damage = amount - (defPosition ? current.mr * 2 : current.mr);
                break;
            case Damage.True:
                damage = amount;
                break;
        }
        damage = Mathf.Clamp(damage, amount / 3, float.MaxValue); //Massima riduzione danno: 66%
        current.hp -= damage;
        current.hp = Mathf.Clamp(current.hp, 0, GetMaxHp());
        if (current.hp == 0) {
            //Die
            Debug.Log(name + " died");
        }
        defPosition = false;
    }
    public void Heal(float amount) {
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        current.hp += amount;
        current.hp = Mathf.Clamp(current.hp, 0, GetMaxHp());
    }

    public bool UseMana(float amount) {
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        if (amount > current.mana) {
            return false;
        }
        current.mana -= amount;
        return true;
    }
    public void RestoreMana(float amount) {
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        current.mana += amount;
        current.mana = Mathf.Clamp(current.mana, 0, GetMaxMana());
    }

    //Old Spell System
    //private void LearnSpell<T>() where T : Spell {
    //    UpdateSpellbook(gameObject.AddComponent<T>());
    //}

    //private void UpdateSpellbook(Spell s) {
    //    if (s.GetType() == typeof(BaseAttack)) {
    //        spellbook[0] = s;
    //    }
    //    else {
    //        bool freeSlotFound = false;
    //        for (int i = 1; i < 5; i++) {
    //            if (spellbook[i] == null) {
    //                spellbook[i] = s;
    //                freeSlotFound = true;
    //                break;
    //            }
    //        }
    //        if (!freeSlotFound) {
    //            //Learn New
    //            Debug.LogError("No free slot found");
    //        }
    //    }
    //}
    //public List<SpellInfo> GetSpellbookInfo() {
    //    List<SpellInfo> temp = new List<SpellInfo>();
    //    foreach (Spell s in spellbook) {
    //        if (s) {
    //            temp.Add(s.info);
    //        }
    //    }
    //    return temp;
    //}

    //public bool Cast(int spellIndex) {
    //    if (spellIndex < spellbook.Length && spellbook[spellIndex]) {
    //        //Debug.Log(name + " usa: " + spellbook[spellIndex].info.spellName + "!");
    //        return spellbook[spellIndex].Activate();
    //    }
    //    else {
    //        Debug.LogError("Nessuna Spell allo slot " + spellIndex);
    //        return false;
    //    }
    //}

    //public void SetEnemy(Player enemy) {
    //    foreach (Spell s in spellbook) {
    //        if (s) {
    //            s.enemy = enemy;
    //        }
    //    }
    //}

    //public float GetCurrentCooldown(int spellIndex) {
    //    return spellbook[spellIndex] ? spellbook[spellIndex].currentCD : 0;
    //}

    //public float GetCooldown(int spellIndex) {
    //    return spellbook[spellIndex] ? spellbook[spellIndex].info.cooldown : 0;
    //}

    //public bool IsOnCooldown(int spellIndex) {
    //    return spellbook[spellIndex].onCooldown;
    //}

    //public bool IsAnimating() {
    //    foreach (Spell s in spellbook) {
    //        if (s && s.isAnimating)
    //            return true;
    //    }
    //    return false;
    //}

    //////public void AddItem(Item i) {
    //////    inventory.Add(i);
    //////    hp += i.hp;
    //////    maxHp += i.hp;
    //////    mana += i.mana;
    //////    maxMana += i.mana;
    //////    attackDamage += i.attackDamage;
    //////    abilityPower += i.abilityPower;
    //////    armor += i.armor;
    //////    magicResistance += i.magicResistance;
    //////    movementSpeed += i.movementSpeed;
    //////    critChance += i.critChance;
    //////    critChance = Mathf.Clamp(critChance, 0, 100);
    //////}

    public float GetMaxHp() {
        return basic.hp + bonus.hp;
    }
    public float GetMaxMana() {
        return basic.mana + bonus.mana;
    }

    public Stats GetBaseStats() {
        return basic;
    }
    public Stats GetCurrentStats() {
        return current;
    }
    public Stats GetBonusStats() {
        return bonus;
    }

    public bool Attack(Player enemy) {
        if (!UseMana(50)) {
            return false;
        }
        FindObjectOfType<BattleManager>().AddLog(name + " sta attaccando!", Color.red);

        StartCoroutine(AttackAnimation(enemy));
        defPosition = false;

        float miss = Random.Range(0, 1f);
        if (miss <= 1 / (enemy.current.ms - current.ms * current.ats / 1000)) {
            //miss
            FindObjectOfType<BattleManager>().AddLog(name + " ha mancato l'attacco!!", Color.white);
            powerUp = false;
            return true; //L'attacco ha funzionato (ma miss)
        }

        bool crit = false;
        float r = Random.Range(0, 1f);
        if (r <= current.crit / 100) {
            FindObjectOfType<BattleManager>().AddLog("Colpo critico!!", new Color(0.75f, 0, 0));
            crit = true;
        }
        if (IsBuildAd()) {
            enemy.TakeDamage(powerUp ? current.ad * 2 : current.ad, crit ? Damage.True : Damage.Physical);
        }
        else {
            enemy.TakeDamage(powerUp ? current.ap * 2 : current.ap, crit ? Damage.True : Damage.Magic);
        }

        powerUp = false;
        return true;
    }
    public bool PowerUp() {
        if (!UseMana(100)) {
            return false;
        }
        StartCoroutine(PowerDefendAnimation());
        FindObjectOfType<BattleManager>().AddLog(name + " si è caricato!", Color.blue);

        defPosition = false;

        powerUp = true;
        return true;
    }
    public bool Defend() {
        StartCoroutine(PowerDefendAnimation());
        FindObjectOfType<BattleManager>().AddLog(name + " in posizione di difesa!", Color.white);

        powerUp = false;

        defPosition = true;
        return true;
    }

    private bool IsBuildAd() {
        int ad = 0, ap = 0;
        foreach (Item i in inventory) {
            if (i.stats.ad > 0) {
                ad++;
            }
            if (i.stats.ap > 0) {
                ap++;
            }
        }
        return ad >= ap;
    }

    private IEnumerator AttackAnimation(Player enemy) {
        isAnimating = true;
        Vector3 currentPosition = transform.position; //Save current position

        bool goRight = GetComponent<PlayerController>();
        Vector3 moveCommand = goRight ? Vector3.right : Vector3.left;
        float speed = Mathf.Clamp(current.ms * current.ats, 500, float.MaxValue);
        Vector3 movement = speed * Time.deltaTime * moveCommand;

        float targetX = transform.position.x + (enemy.transform.position.x - transform.position.x) / 2;

        while (goRight ? transform.position.x <= targetX : transform.position.x >= targetX) {
            yield return new WaitForFixedUpdate();
            transform.Translate(movement);

        }
        movement *= -1;
        while (goRight ? transform.position.x >= currentPosition.x : transform.position.x <= currentPosition.x) {
            yield return new WaitForFixedUpdate();
            transform.Translate(movement);
        }

        transform.position = currentPosition; //Reset to original position
        isAnimating = false;
    }
    private IEnumerator PowerDefendAnimation() {
        isAnimating = true;
        yield return new WaitForSeconds(.5f);
        isAnimating = false;
    }

    private void Die() {
    }
}

[System.Serializable]
public struct Stats {
    public float hp;        //500
    public float mana;      //200
    public float ad;        //60
    public float ap;        //0
    public float crit;      //0
    public float armor;     //30
    public float mr;        //30
    public float ats;       //0.5
    public float ms;        //325
    public float cdr;       //0
}

public enum Damage {
    Physical,
    Magic,
    True
}