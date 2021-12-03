using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
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
    [SerializeField]
    private float dmgTimer;
    public float critChance = 10.0f;
    public float waitDisable = 3.0f;
    public bool damageUltimate = false;
    bool armorUltimate = false;

    public bool bossDamage = false;
    public bool bossArmor = false;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
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

    public void TakeDamage(int amount)
    {
        if (dmgTimer < 0.28f)
            return;

        dmgTimer = 0;

        if(bossArmor)
        {
            float damage = amount / 1.30f;
            amount = Mathf.FloorToInt(damage);
        }

        if (armorUltimate)
        {
            float damage = amount / 1.85f;
            amount = Mathf.FloorToInt(damage);
        }

        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
            gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void HealMe(int heal)
    {
        currentHealth += heal;

        if (currentHealth > 100)
            currentHealth = 100;

        healthSlider.value = currentHealth;
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
                    playerController.curSpeed = 12;
                    critChance = 85f;
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

    public void ResetStats()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
        currentMana = 0;
        manaSlider.value = currentMana;
        cooldownTimer = cooldownTime;
        cooldown = false;
        specialCooldownText.text = "";
    }

    void DisableAbility()
    {
        critChance = 10f;
        playerController.curSpeed = 7;
        anim.speed = 1.0f;
        damageUltimate = false;
        armorUltimate = false;
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
