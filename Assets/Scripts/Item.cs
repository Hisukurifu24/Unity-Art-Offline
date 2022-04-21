using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Name", menuName = "Item", order = 1)]
public class Item : ScriptableObject {
    public Sprite icon;
    public string itemName;
    [TextArea] public string description;

    

    public Rarity rarity;
    public Stats stats;
}

public enum Rarity{
    Common = 1,
    Uncommon = 4,
    Rare = 32,
    Epic = 256,
    Legendary = 2048,
    Relic = 16384,
    Artifact = 1048676,
}
/*
 * Common       1/1         0
 * Uncommon     1/4         2
 * Rare         1/32        5
 * Epic         1/256       8
 * Legendary    1/2048      11
 * Relic        1/16384     14
 * Artifact     1/1048676   20
 */
