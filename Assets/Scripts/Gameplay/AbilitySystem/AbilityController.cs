using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Random;

public class AbilityController : MonoBehaviour {
    [SerializeField] GameObject abilityBase;
    [SerializeField] GameObject abilityMain1;
    [SerializeField] GameObject abilityMain2;
    [SerializeField] GameObject abilityUltimate;
    [SerializeField] GameObject abilitySecondary1;
    [SerializeField] GameObject abilitySecondary2;

    private bool isPlayerBusy;
    private GameObject ability;

    private KeyCode abilityBaseKey = KeyCode.Mouse0;
    private KeyCode abilityMain1Key = KeyCode.Q;
    private KeyCode abilityMain2Key = KeyCode.E;
    private KeyCode abilityUltimateKey = KeyCode.R;
    private KeyCode abilitySecondary1Key = KeyCode.LeftShift;
    private KeyCode abilitySecondary2Key = KeyCode.Space;

    private void Update() {
        isPlayerBusy = false;
        foreach (Ability a in GetComponents<Ability>()) {
            if (a.GetState() == AbilityState.active) {
                isPlayerBusy = true;
            }
        }

        bool buttonPressed = false;

        if (Input.GetKeyDown(abilityBaseKey)) {
            ability = abilityBase;
            buttonPressed = true;
        }
        if (Input.GetKeyDown(abilityMain1Key)) {
            ability = abilityMain1;
            buttonPressed = true;
        }
        if (Input.GetKeyDown(abilityMain2Key)) {
            ability = abilityMain2;
            buttonPressed = true;
        }
        if (Input.GetKeyDown(abilityUltimateKey)) {
            ability = abilityUltimate;
            buttonPressed = true;
        }
        if (Input.GetKeyDown(abilitySecondary1Key)) {
            ability = abilitySecondary1;
            buttonPressed = true;
        }
        if (Input.GetKeyDown(abilitySecondary2Key)) {
            ability = abilitySecondary2;
            buttonPressed = true;
        }

        if (buttonPressed && ability.GetComponent<Ability>().GetState() == AbilityState.ready) {
            if (!isPlayerBusy ) {
                Instantiate(ability, transform);
            }
        }
    }
}
