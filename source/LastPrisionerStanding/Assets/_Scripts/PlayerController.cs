using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float curSpeed;
    public float speedSmothTime;
    public float combo1cd;
    public float combo2cd;
    public float combo3cd;
    public GameObject[] detectors;
    public bool canUseAbility;
    [HideInInspector]
    public int playerNumber;

    private PlayerHealth playerHealth;
    private AudioSource audioSource;
    private GameObject[] targets;
    private GameObject currentTarget;
    private WeaponDetection weaponDetection;
    private Rigidbody playerRigidBody;
    private Animator playerAnimator;
    private Vector3 forward, right;
    private Vector3 movement;
    private int targetIndex;
    private float horizontalValue;
    private float verticalValue;
    private int attackCount;
    private float comboTimer;
    private bool canMove;
    private bool fightMode;
    public int CharInt;

    void Start() {
        targets = GameObject.FindGameObjectsWithTag("Target");
        //currentTarget = targets[targetIndex];
        weaponDetection = GetComponentInChildren<WeaponDetection>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = GetComponentInChildren<PlayerHealth>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        if (playerNumber == 0){
            playerNumber = 1;
        }
        forward = GameObject.Find("CameraP" + playerNumber).transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        movement = Vector3.zero;
        attackCount = 0;
        canMove = true;
        fightMode = false;
    }
	
	void Update () {
        comboTimer += Time.deltaTime;       

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (!fightMode)
                fightMode = true;
            else
                fightMode = false;            
        }

        if (fightMode)
        {         
            transform.parent.LookAt(currentTarget.transform);
            ChangeTarget();
        }        

        if (Input.GetButtonUp("Attack" + playerNumber) && attackCount == 0)
        {
            playerAnimator.SetTrigger("Combo1");
            Debug.Log(1);
            attackCount++;
            comboTimer = 0;
            canMove = false;
        }
        if (Input.GetButtonUp("Attack" + playerNumber) && attackCount == 1 && comboTimer > combo2cd)
        {
            playerAnimator.SetTrigger("Combo2");
            Debug.Log(2);
            attackCount++;
            comboTimer = 0;
            canMove = false;
        }
        if (Input.GetButtonUp("Attack" + playerNumber) && attackCount == 2 && comboTimer > combo3cd)
        {
            playerAnimator.SetTrigger("Combo3");
            Debug.Log(3);
            attackCount++;
            canMove = false;
        }
        else if(comboTimer > 1.6f && attackCount > 0)
        {
            attackCount = 0;
            comboTimer = 0;
        }

        if (canUseAbility)
        {
            if (Input.GetButtonUp("Ability" + playerNumber))
            {
                playerHealth.UseAbility();
                canUseAbility = false;
            }
        }
    }

    void FixedUpdate()
    {
        horizontalValue = Input.GetAxis("Horizontal" + playerNumber);
        verticalValue = Input.GetAxis("Vertical" + playerNumber);
        if (canMove)
            Move();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "WeaponCollider":
                PlayerHealth Enemy = other.gameObject.GetComponentInParent<PlayerHealth>();

                Enemy.AddMana(5);
                int amount = Random.Range(5, 12);

                float randValue = Random.value;
                if (randValue < playerHealth.critChance)
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

                playerHealth.TakeDamage(amount);
                break;

            case "Heal":
                Destroy(other.gameObject);
                playerHealth.HealMe(45);
                break;
        }
    }

        void EnableDetectors()
    {
        for (int i = 0; i < detectors.Length; i++)
        {
            detectors[i].SetActive(true);
            audioSource.Play();
            TrailRenderer trail = detectors[i].GetComponent<TrailRenderer>();
            if (trail)
            {
                trail.enabled = true;   
            }
        }
    }

    void DisableDetectors()
    {
        for (int i = 0; i < detectors.Length; i++)
        {
            detectors[i].SetActive(false);
            TrailRenderer trail = detectors[i].GetComponent<TrailRenderer>();
            if (trail)
            {
                trail.Clear();
                trail.enabled = false;
            }
        }
    }

    void Move()
    {
        Vector2 inputDir = new Vector2(horizontalValue, verticalValue);
        bool walking = inputDir.magnitude > 0.7 ? true : false;
        Vector3 rightMovement = right * horizontalValue;
        Vector3 upMovement = forward * verticalValue;
        movement = Vector3.Normalize(rightMovement + upMovement);
        movement = movement * curSpeed * Time.deltaTime;

        if (!fightMode)
        {
            if (movement != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(movement);
                rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
                transform.rotation = rotation;
            }
        }

        if (walking)
        {
            playerRigidBody.MovePosition(playerRigidBody.position + movement);
        }

        float animationSpeedPercent = ((walking) ? 1 : 0f) * inputDir.magnitude;
        playerAnimator.SetFloat("Speed", animationSpeedPercent, speedSmothTime, Time.deltaTime);
    }

    void ChangeTarget()
    {
        if (Input.GetKeyUp(KeyCode.Q) && targetIndex > 0)
        {
            targetIndex--;
            currentTarget = targets[targetIndex];
        }
        if (Input.GetKeyUp(KeyCode.E) && targetIndex < targets.Length-1)
        {
            targetIndex++;
            currentTarget = targets[targetIndex];
        }
    }

    public void CanMoveFunction()
    {
        canMove = true;
    }
}
