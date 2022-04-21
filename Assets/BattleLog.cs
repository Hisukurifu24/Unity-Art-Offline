using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleLog : MonoBehaviour {
    [SerializeField] private Text[] uiText = new Text[5];

    private void Awake() {
    }

    private void Update() {
    }

    public void AddText(string s, Color c) {
        for(int i = 4; i>0; i--) {
            uiText[i].text = uiText[i - 1].text;
            uiText[i].color = uiText[i - 1].color;
        }
        uiText[0].text = s;
        uiText[0].color = c;
    }
}
