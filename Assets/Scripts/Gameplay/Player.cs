using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    [SerializeField] private List<Item> inventory;
    [SerializeField] private List<Item> bag;

    [SerializeField] private PlayerStat baseStats;

    private Stats basic;
    private Stats current;
    private Stats bonus;

    [SerializeField] private float experience = 0;
    [SerializeField] private int level = 1;
    [SerializeField] private int skillPoints = 0;
    [SerializeField] private int gold = 0;
    [SerializeField] private string whatIsEnemy;

    private float hpReg = 1f;
    private float manaReg = 10f;

    private bool isAvailable = true;

    private Animator animator;

    private AudioSource audioSource;
    private AudioClip[] hit = new AudioClip[2];
    private AudioClip death;



    private void Awake() {
        //spellbook = new Spell[5];
        //LearnSpell<BaseAttack>();
        basic = new Stats(baseStats);
        current = new Stats();
        bonus = new Stats();
        current = basic;

        inventory = new List<Item>();
        bag = new List<Item>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        hit[0] = Resources.Load<AudioClip>("Sounds/GruntVoice01");
        hit[1] = Resources.Load<AudioClip>("Sounds/GruntVoice02");
        death = Resources.Load<AudioClip>("Sounds/DeathVoice");
    }

    private void Start() {
        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());
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

    private void Die() {
        audioSource.clip = death;
        audioSource.Play();
        animator.SetTrigger("Dead");
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, 2);
    }
    private IEnumerator Hit() {
        audioSource.clip = hit[UnityEngine.Random.Range(0, 2)];
        audioSource.Play();
        GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().color = Color.white;

    }
    public void TakeDamage(float amount, Damage type) {
        StartCoroutine(Hit());
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        float damage = 0;
        switch (type) {
            case Damage.Physical:
                //damage = amount - (defPosition ? current.Get(Stat.Armor) * 2 : current.Get(Stat.Armor));
                damage = amount - current.Get(Stat.Armor);
                break;
            case Damage.Magic:
                //damage = amount - (defPosition ? current.Get(Stat.MagicResistance) * 2 : current.Get(Stat.MagicResistance));
                damage = amount - current.Get(Stat.MagicResistance);
                break;
            case Damage.True:
                damage = amount;
                break;
        }
        damage = Mathf.Clamp(damage, amount / 3, float.MaxValue); //Massima riduzione danno: 66%
        current.Decrease(Stat.Health, damage);
        current.Set(Stat.Health, Mathf.Clamp(current.Get(Stat.Health), 0, GetMaxHp()));
        if (current.Get(Stat.Health) == 0) {
            Debug.Log(name + " died");
            Die();
        }
        //defPosition = false;
    }
    public void Heal(float amount) {
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        current.Increase(Stat.Health, amount);
        current.Set(Stat.Health, Mathf.Clamp(current.Get(Stat.Health), 0, GetMaxHp()));
    }

    public bool UseMana(float amount) {
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        if (amount > current.Get(Stat.Mana)) {
            return false;
        }
        current.Decrease(Stat.Mana, amount);
        return true;
    }
    public void RestoreMana(float amount) {
        amount = Mathf.Clamp(amount, 0, float.MaxValue);
        current.Increase(Stat.Mana, amount);
        current.Set(Stat.Mana, Mathf.Clamp(current.Get(Stat.Mana), 0, GetMaxMana()));
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
        return (basic + bonus).Get(Stat.Health);
    }
    public float GetMaxMana() {
        return (basic + bonus).Get(Stat.Mana);
    }
    public float GetExp() {
        return experience;
    }
    public float GetMaxExp() {
        return level * 100;
    }
    public int GetLevel() {
        return level;
    }
    public int GetGold() {
        return gold;
    }

    public float GetBaseStat(Stat stat) {
        return basic.Get(stat);
    }
    public float GetCurrentStat(Stat stat) {
        return current.Get(stat);
    }
    public float GetBonusStat(Stat stat) {
        return bonus.Get(stat);
    }

    public void Buff(Stats buff, float time) {
        bonus += buff;
        StartCoroutine(RemoveBuff(buff, time));
    }
    private IEnumerator RemoveBuff(Stats buff, float time) {
        yield return new WaitForSeconds(time);
        bonus -= buff;
    }

    /* Old Combat Systen
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
            FindObjectOfType<BattleManager>().AddLog(name + " ha mancato l'attacco!!", Color.grey);
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
        FindObjectOfType<BattleManager>().AddLog(name + " si Ë caricato!", Color.blue);

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

    private IEnumerator AttackAnimation(Player enemy) {
        isAnimating = true;
        Vector3 currentPosition = transform.position; //Save current position

        bool goRight = gameObject.tag == "Player";
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
    */

    private void UpdateBonusStats() {
        bonus = new Stats();
        foreach (Item i in inventory) {
            bonus += i.stats;
        }
        current += bonus;
    }

    private IEnumerator AddingGold(int amount) {
        float speed = 2 / (float)amount;
        while (amount > 0) {
            gold++;
            amount--;
            yield return new WaitForSeconds(speed);
        }
        gold = Mathf.Clamp(gold, 0, int.MaxValue);
    }
    public void AddGold(int amount) {
        amount = Mathf.Clamp(amount, 0, int.MaxValue);
        StartCoroutine(AddingGold(amount));
    }
    public bool SpendGold(int amount) {
        amount = Mathf.Clamp(amount, 0, int.MaxValue);
        if (amount > gold) {
            return false;
        }
        gold -= amount;
        return true;
    }

    private IEnumerator AddingExp(float amount) {
        bool leveledUp = false;

        float speed = 1 / amount;
        while (amount > 0) {
            experience++;
            amount--;
            if (experience >= GetMaxExp()) {
                experience -= GetMaxExp();
                leveledUp = true;
                LevelUp();
            }
            yield return new WaitForSeconds(speed);
        }

        if (leveledUp) {
            FindObjectOfType<DialogManager>().PromptMessage("Sei salito al livello " + level + "!" +
                "\nHai " + skillPoints + " Skill Points a disposizione!");
        }
    }
    public void AddExp(int amount) {
        amount = Mathf.Clamp(amount, 0, int.MaxValue);
        //FindObjectOfType<DialogManager>().PromptMessage("Hai guadagnato " + amount + " punti esperienza!");
        StartCoroutine(AddingExp(amount));
    }
    public void LevelUp() {
        level++;
        skillPoints += 2;
    }

    public Item GetInventoryItem(int i) {
        return i < inventory.Count ? inventory[i] : null;
    }
    public Item GetBagItem(int i) {
        return i < bag.Count ? bag[i] : null;
    }
    private bool IsBuildAd() {
        int ad = 0, ap = 0;
        foreach (Item i in inventory) {
            if (i.stats.Get(Stat.AttackDamage) > 0) {
                ad++;
            }
            if (i.stats.Get(Stat.AbilityPower) > 0) {
                ap++;
            }
        }
        return ad >= ap;
    }
    public void AddItem(Item i) {
        if (i == null) {
            return;
        }
        if (inventory.Count < 6) {
            inventory.Add(i);
            UpdateBonusStats();
            FindObjectOfType<DialogManager>().PromptMessage("Hai ottenuto " + i.itemName + "!\n» stato messo nell'inventario.");
        }
        else if (bag.Count < 100) {
            bag.Add(i);
            FindObjectOfType<DialogManager>().PromptMessage("Hai ottenuto " + i.itemName + "!\n» stato messo nella borsa.");
        }
        else {
            FindObjectOfType<DialogManager>().PromptMessage("Inventario e borsa pieni!");
        }
    }
    public void Swap(int invIndex, int bagIndex) {
        Item i = invIndex < inventory.Count ? inventory[invIndex] : null;
        Item b = bagIndex < bag.Count ? bag[bagIndex] : null;
        inventory.Remove(i);
        bag.Remove(b);
        if (b)
            inventory.Add(b);
        if (i)
            bag.Add(i);
    }

    public string WhatIsEnemy() {
        return whatIsEnemy;
    }

    public bool IsAvailable() {
        return isAvailable;
    }
    public void SetAvailable() {
        isAvailable = true;
    }
    public void SetUnavailable() {
        isAvailable = false;
    }
}


[System.Serializable]
public enum Stat {
    Health,
    Mana,
    AttackDamage,
    AbilityPower,
    CriticalChance,
    Armor,
    MagicResistance,
    AttackSpeed,
    MovementSpeed,
    Strength,
    Dexterity,
    Intelligence
}

[System.Serializable]
public class Stats {
    private Dictionary<Stat, float> values = new Dictionary<Stat, float>();

    public Stats() {
        foreach (Stat stat in System.Enum.GetValues(typeof(Stat))) {
            values[stat] = 0;
        }
    }

    public Stats(PlayerStat baseStats) {
        values[Stat.Health] = baseStats.Health;
        values[Stat.Mana] = baseStats.Mana;
        values[Stat.AttackDamage] = baseStats.AttackDamage;
        values[Stat.AbilityPower] = baseStats.AbilityPower;
        values[Stat.CriticalChance] = baseStats.CriticalChance;
        values[Stat.Armor] = baseStats.Armor;
        values[Stat.MagicResistance] = baseStats.MagicResistance;
        values[Stat.AttackSpeed] = baseStats.AttackSpeed;
        values[Stat.MovementSpeed] = baseStats.MovementSpeed;
        values[Stat.Strength] = baseStats.Strength;
        values[Stat.Dexterity] = baseStats.Dexterity;
        values[Stat.Intelligence] = baseStats.Intelligence;
    }

    public static Stats operator +(Stats stat1, Stats stat2) {
        Stats result = new Stats();
        foreach (Stat stat in System.Enum.GetValues(typeof(Stat))) {
            result.Set(stat, stat1.Get(stat) + stat2.Get(stat));
        }
        return result;
    }

    public static Stats operator -(Stats stat1, Stats stat2) {
        Stats result = new Stats();
        foreach (Stat stat in System.Enum.GetValues(typeof(Stat))) {
            result.Set(stat, stat1.Get(stat) - stat2.Get(stat));
        }
        return result;
    }

    public float Get(Stat stat) {
        return values[stat];
    }

    public void Set(Stat stat, float value) {
        values[stat] = value;
    }

    public void Increase(Stat stat, float amount) {
        values[stat] += amount;
    }

    public void Decrease(Stat stat, float amount) {
        values[stat] -= amount;
    }
}

/*
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
}
*/

public enum Damage {
    Physical,
    Magic,
    True
}