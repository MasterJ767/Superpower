using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Statistics {
public class Health : MonoBehaviour {
    [Header("Values")]
    public float maxHealth = 1000;
    [HideInInspector] public float currentHealth;
    public float regenPerSecond = 4f;
    public float regenDelay = 5f;

    [Header("UI")]
    public bool hasHealthBar;
    public Slider slider;
    public Image healthFill;
    public TextMeshProUGUI healthText;
    public Gradient healthColour;

    [HideInInspector] public bool isDead;
    [HideInInspector] public float timeSinceHit;
    [HideInInspector] public bool isRewinding;

    private void Start() {
        SetHealthMax();
    }

     private void Update() {
        if (!isRewinding) {
            timeSinceHit += Time.deltaTime;
            if (currentHealth < maxHealth && timeSinceHit > regenDelay)
            {
                Heal(regenPerSecond * Time.deltaTime);
            }
        }
    }

    private void SetHealthMax() {
        if (hasHealthBar)
        {
            slider.maxValue = maxHealth;
        }
        currentHealth = maxHealth;
        SetHealthSlider();
    }

    public void SetHealthSlider() {
        if (hasHealthBar)
        {
            slider.value = currentHealth;
            healthText.text = Mathf.Floor(currentHealth) + "";
            healthFill.color = healthColour.Evaluate(slider.normalizedValue);
        }
    }

    public void Damage(float value) {
        if (Mathf.Abs(value) > 0 && !isRewinding) {
            timeSinceHit = 0;
            currentHealth = Mathf.Max(0, currentHealth - value);
            SetHealthSlider();
            if (currentHealth <= 0 && !isDead)
            {
                Death();
            }
        }
    }

    public float DamageQuery(float value) {
        return Mathf.Max(0, currentHealth - value);
    }

    public void Heal(float value) {
        if (Mathf.Abs(value) > 0 && !isRewinding) {
            currentHealth = Mathf.Min(maxHealth, currentHealth + value);
            SetHealthSlider();
        }
    }

    private void Death() {
        isDead = true;
        
        // Run on death things here
    }
}
}