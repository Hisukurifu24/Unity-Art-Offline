using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Ability : MonoBehaviour {
    private static AbilityData[] abilities;

    public AbilityData data;

    private float currentCooldownTime;
    private float currentActiveTime;

    enum AbilityState {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready;

    public KeyCode key;

    private void Awake() {
        abilities = Resources.LoadAll<AbilityData>("Abilities");
        data = GetAbilityData(GetType().ToString());
    }

    public abstract void Activate();
    public abstract void BeginCooldown();

    private void Update() {
        switch (state) {
            case AbilityState.ready:
                if (Input.GetKeyDown(key) && !EventSystem.current.IsPointerOverGameObject()) {
                    Activate();
                    state = AbilityState.active;
                    currentActiveTime = data.activeTime;
                }
                break;
            case AbilityState.active:
                if (currentActiveTime > 0) {
                    currentActiveTime -= Time.deltaTime;
                }
                else {
                    BeginCooldown();
                    state = AbilityState.cooldown;
                    currentCooldownTime = data.cooldownTime;
                }
                break;
            case AbilityState.cooldown:
                if (currentCooldownTime > 0) {
                    currentCooldownTime -= Time.deltaTime;
                }
                else {
                    state = AbilityState.ready;
                }
                break;
            default:
                break;
        }
    }

    public static AbilityData GetAbilityData(string name) {
        foreach(AbilityData a in abilities) {
            if(a.name == name) {
                return a;
            }
        }
        Debug.LogError("Ability data not found: " + name);
        return null;
    }
}
