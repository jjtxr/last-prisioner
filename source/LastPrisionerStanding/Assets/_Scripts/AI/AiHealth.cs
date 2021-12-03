using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int amount;

    CapsuleCollider capsuleCollider;
    AiMob aiMob;
    bool isDead;
    bool isSinking;

    hordeController hControl;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        aiMob = GetComponent<AiMob>();
        currentHealth = startingHealth;
        hControl = GetComponentInParent<hordeController>();
    }

    void Update()
    {
        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
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
                if (randValue < Enemy.critChance)
                    amount = amount + Random.Range(1, 5);

                if (Enemy.damageUltimate)
                {
                    float damage = amount * 1.85f;
                    amount = Mathf.FloorToInt(damage);
                }

                if (Enemy.bossDamage)
                {
                    float damage = amount * 1.30f;
                    amount = Mathf.FloorToInt(damage);
                }

                TakeDamage(amount, Enemy);
                break;
        }
    }

    public void TakeDamage(int amount, PlayerHealth guilty)
    {
        if (isDead)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            if (aiMob.isBoss)
            {
                if (aiMob.bossType == 0)
                {
                    guilty.bossDamage = true;
                }
                else
                {
                    guilty.bossArmor = true;
                }
            }
            if (aiMob != null)
            {
                aiMob.Disabled();
            }
            Death();
        }
    }

    void Death()
    {
        isDead = true;
        if (!aiMob.isBoss)
        {
            hControl.hNum--;
        }
        StartSinking();

        if (capsuleCollider != null)
            capsuleCollider.isTrigger = true;
        else
            GetComponentInChildren<MeshCollider>().isTrigger = true;
    }

    public void StartSinking()
    {
        UnityEngine.AI.NavMeshAgent navMesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMesh != null)
            navMesh.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        isSinking = true;
        Destroy(gameObject, 2f);
    }
}
