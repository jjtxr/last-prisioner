using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public int currentMana;
    public Slider healthSlider;
    public Slider manaSlider;
    public Image damageImage;
    public Image specialImage;
    public Text specialCooldownText;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float cooldownTime;

    Color disColor;
    Animator anim;
    PlayerController playerController;
    bool isDead;
    bool damaged;
    bool cooldown;
    private float cooldownTimer;
    private float dmgTimer;
    private float critChance = 10.0f;
    private DamageTextController dmgTextController;
    public float waitDisable = 3.0f;
    bool damageUltimate = false;
    bool armorUltimate = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        dmgTextController = GameObject.FindGameObjectWithTag("GameController").GetComponent<DamageTextController>();
        if (healthSlider == null) {
            healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
            manaSlider = GameObject.FindGameObjectWithTag("ManaSlider").GetComponent<Slider>();
            specialImage = GameObject.FindGameObjectWithTag("Ability").GetComponent<Image>();
            damageImage = GameObject.FindGameObjectWithTag("DamageImage").GetComponent<Image>();
            specialCooldownText = GameObject.FindGameObjectWithTag("Ability").GetComponentInChildren<Text>();
        }
        currentHealth = startingHealth;
        currentMana = 0;
        disColor = specialImage.color;
        cooldownTimer = cooldownTime;
    }

    void Update()
    {
        dmgTimer += Time.deltaTime;
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        if (cooldown)
        {
            cooldownTimer -= Time.deltaTime;
            specialCooldownText.text = cooldownTimer.ToString("0");

            if (cooldownTimer <= 0)
            {
                cooldownTimer = cooldownTime;
                specialCooldownText.text = "";
                cooldown = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "WeaponCollider":
                var Enemy = other.gameObject.GetComponentInParent<PlayerHealth>();

                Enemy.AddMana(5);
                int amount = Random.Range(1, 5);

                float randValue = Random.value;
                if (randValue < critChance)
                    amount = amount + Random.Range(1, 5);

                if (Enemy.damageUltimate)
                {
                    float damage = amount * 1.85f;
                    amount = Mathf.FloorToInt(damage);
                }

                TakeDamage(amount);
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        if (dmgTimer < 0.28f)
            return;

        dmgTimer = 0;

        if (armorUltimate)
        {
            float damage = amount / 1.85f;
            amount = Mathf.FloorToInt(damage);
        }

        currentHealth -= amount;
        dmgTextController.CreateFloatingText(amount, transform);
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            gameObject.SetActive(false);
    }

    public void AddMana(int amount)
    {
        currentMana += amount;

        manaSlider.value = currentMana;

        if (currentMana >= 100)
        {
            currentMana = 100;
            Color actColor = disColor;
            actColor.a = 255;
            specialImage.color = actColor;
            playerController.canUseAbility = true;
        }
    }

    public void UseAbility()
    {
        if (!cooldown)
        {
            currentMana = 0;
            manaSlider.value = currentMana;
            specialImage.color = disColor;
            cooldown = true;

            Debug.Log("playerNum:" + playerController.CharInt);
            switch (playerController.CharInt)
            {
                // SPEARMANO
                case 0:
                    anim.speed = 2.0f;
                    break;
                // SWORDSMANO
                case 1:
                    damageUltimate = true;
                    break;
                // NIGHTMANO
                case 2:
                    armorUltimate = true;
                    break;
            }

            Invoke("DisableAbility", waitDisable);
        }
    }

    void Death()
    {
        isDead = true;

       // anim.SetTrigger("Die");

        playerController.enabled = false;
    }

    public void SetUIElements(Slider health, Slider mana, Image ability, Text cooldown, Image damageImg)
    {
        healthSlider = health;
        manaSlider = mana;
        specialImage = ability;
        specialCooldownText = cooldown;
        damageImage = damageImg;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
