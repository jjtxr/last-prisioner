using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMob : MonoBehaviour {

    public float distanceToFollow;
    public float distanceToAttack;
    public float damping;
    public float timeBetweenAttacks;
    public float attackDamage;
    public bool isBoss;
    public int bossType;

    private GameObject[] players;
    private Animator mobAnimator;
    private AiHealth aiHealth;
    private UnityEngine.AI.NavMeshAgent nav;
    private GameController gameController;
    private float[] playerDistance;
    private float timer;

    void Start()
    {
        timer = 0;
        players = GameObject.FindGameObjectsWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        if (players == null)
        {
            this.enabled = false;
            Debug.Log("ze");
        }
        aiHealth = GetComponent<AiHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        mobAnimator = GetComponent<Animator>();
        playerDistance = new float[2];
        StartCoroutine(SetDestiny());
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (aiHealth.currentHealth > 0 && !gameController.OnePlayerLeft()) {
            if (!nav.enabled)
                nav.enabled = true;

            for(var i = 0; i < players.Length; i++)
                playerDistance[i] = Vector3.Distance(transform.position, players[i].transform.position);
        }
        else {
            nav.enabled = false;
        }
    }

    void LookAtPlayer(Transform player)
    {
        Quaternion rotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    void Attack(GameObject player)
    {
        if (timer >= timeBetweenAttacks && aiHealth.currentHealth > 0)
        {
            timer = 0f;
            PlayerHealth playerHealth = player.GetComponentInChildren<PlayerHealth>();
            if (playerHealth == null)
                return;
            if (playerHealth.currentHealth > 0)
            {
                playerHealth.TakeDamage((int)attackDamage);
                if (mobAnimator.GetBool("Attack"))
                    return;
                mobAnimator.SetBool("Attack", true);
                mobAnimator.SetBool("Walk", false);
            }
            else
            {
                mobAnimator.SetBool("Attack", false);
                mobAnimator.SetBool("Walk", false);
            }
        }
    }

    IEnumerator SetDestiny()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            for (var i = 0; i < players.Length; i++)
            {
                yield return new WaitForSeconds(0.1f);
                if (playerDistance[i] < distanceToFollow)
                {
                    if (playerDistance[i] < distanceToAttack)
                    {
                        LookAtPlayer(players[i].transform);
                        Attack(players[i]);

                        if (!nav.enabled)
                            yield return null;

                        nav.enabled = true;
                        nav.Stop();
                        nav.enabled = false;                       
                    }
                    else {

                        if (nav.enabled)
                            yield return null;

                        nav.enabled = true;
                        nav.SetDestination(players[i].transform.position);
                        mobAnimator.SetBool("Attack", false);
                        mobAnimator.SetBool("Walk", true);
                    }
                }
                else
                {
                    if (!mobAnimator.GetBool("Walk"))
                        yield return null;

                    mobAnimator.SetBool("Attack", false);
                    mobAnimator.SetBool("Walk", false);
                }
            }          
        }
    }

    public void Disabled()
    {
        StopCoroutine(SetDestiny());
        nav.Stop();
        nav.enabled = false;
        this.enabled = false;
    }
}
