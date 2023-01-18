using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI {
public class ColourTheme : MonoBehaviour {
    [Header("Player Infomation")]
    public Statistics.Information playerInfo;

    [Header("Solid Elements")]
    public Image cornerImageLeft1;
    public Image cornerImageLeft2;
    public Image cornerImageRight1;
    public Image cornerImageRight2;
    public Image healthBorder;
    public Image staminaBorder;
    public Image energyBorder;
    public Image classBorder;
    public Image ability1Border;
    public Image ability2Border;
    public Image ability3Border;
    public Image ability4Border;
    public Image ability5Border;
    public Image ability1HeaderLeft;
    public Image ability1HeaderRight;
    public TextMeshProUGUI ability1HeaderText;
    public Image ability1HeaderBack;
    public Image ability2HeaderLeft;
    public Image ability2HeaderRight;
    public TextMeshProUGUI ability2HeaderText;
    public Image ability2HeaderBack;
    public Image ability3HeaderLeft;
    public Image ability3HeaderRight;
    public TextMeshProUGUI ability3HeaderText;
    public Image ability3HeaderBack;
    public Image ability4HeaderLeft;
    public Image ability4HeaderRight;
    public TextMeshProUGUI ability4HeaderText;
    public Image ability4HeaderBack;
    public Image ability5HeaderLeft;
    public Image ability5HeaderRight;
    public TextMeshProUGUI ability5HeaderText;
    public Image ability5HeaderBack;

    [Header("Rotating Borders")]
    public Image classBorder1;
    public Image classBorder2;
    public Image ability1Border1;
    public Image ability1Border2;
    public Image ability2Border1;
    public Image ability2Border2;
    public Image ability3Border1;
    public Image ability3Border2;
    public Image ability4Border1;
    public Image ability4Border2;
    public Image ability5Border1;
    public Image ability5Border2;

    private Managers.KineticManager classManager;

    private void Awake() {
        classManager = Managers.KineticManager.GetInstance();
    }

    private void Start() {
        Statistics.KineticClass playerClass = classManager.GetClass(playerInfo.entityClass);
        Color primary = playerClass.primary;
        Color secondary = playerClass.secondary;

        classBorder1.color = secondary;
        ability1Border1.color = secondary;
        ability2Border1.color = secondary;
        ability3Border1.color = secondary;
        ability4Border1.color = secondary;
        ability5Border1.color = secondary;
        classBorder2.color = primary;
        ability1Border2.color = primary;
        ability2Border2.color = primary;
        ability3Border2.color = primary;
        ability4Border2.color = primary;
        ability5Border2.color = primary;
        ability1HeaderLeft.color = primary;
        ability1HeaderRight.color = primary;
        ability1HeaderText.color = primary;
        ability1HeaderBack.color = primary;
        ability2HeaderLeft.color = primary;
        ability2HeaderRight.color = primary;
        ability2HeaderText.color = primary;
        ability2HeaderBack.color = primary;
        ability3HeaderLeft.color = primary;
        ability3HeaderRight.color = primary;
        ability3HeaderText.color = primary;
        ability3HeaderBack.color = primary;
        ability4HeaderLeft.color = primary;
        ability4HeaderRight.color = primary;
        ability4HeaderText.color = primary;
        ability4HeaderBack.color = primary;
        ability5HeaderLeft.color = primary;
        ability5HeaderRight.color = primary;
        ability5HeaderText.color = primary;
        ability5HeaderBack.color = primary;

        cornerImageLeft1.color = primary;
        cornerImageLeft2.color = primary;
        cornerImageRight1.color = primary;
        cornerImageRight2.color = primary;
        healthBorder.color = primary;
        staminaBorder.color = primary;
        energyBorder.color = primary;
        classBorder.color = primary;
        ability1Border.color = primary;
        ability2Border.color = primary;
        ability3Border.color = primary;
        ability4Border.color = primary;
        ability5Border.color = primary;
    }
}
}