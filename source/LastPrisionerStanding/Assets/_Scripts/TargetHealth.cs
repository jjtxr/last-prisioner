using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetHealth : MonoBehaviour {

    
    public Slider healthSlider;
    public Image fillImage;
    public int startingHealth = 100;
    public int currentHealth;
    public int maxAmount;

    private float dmgTimer;
    private DamageTextController dmgTextController;

    void Start()
    {
        dmgTextController = GameObject.Find("GameTrainningController").GetComponent<DamageTextController>();
        currentHealth = startingHealth;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        dmgTimer += Time.deltaTime;
    }

    public void TakeDamage()
    {
        if (dmgTimer < 0.28f)
            return;

        dmgTimer = 0;
        int amount = Random.Range(1, maxAmount);

        currentHealth -= amount;

        dmgTextController.CreateFloatingText(amount,transform);

        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Refill();
        }
    }

    void Refill()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "WeaponCollider":
                TakeDamage();
                other.gameObject.GetComponentInParent<PlayerHealth>().AddMana(5);
                break;
        }
    }
}
