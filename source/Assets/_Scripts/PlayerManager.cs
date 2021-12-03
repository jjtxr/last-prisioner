using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerManager {

    public GameObject playerCharacter;
    public Transform spawnPoint;
    public CameraScript playerCamera;
    public Text playersCharacterName;
    public Slider healthSlider;
    public Slider manaSlider;
    public Image ablityImage;
    public Text abilityCooldown;
    public Image damageImage;
    [HideInInspector] public int playerNumber;
    [HideInInspector] public string playerText;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public int nWins;
    [HideInInspector] public int CharInt;

    private PlayerController playerController;
    private PlayerHealth playerHealth;

    public void Setup()
    {
        playerController = instance.GetComponentInChildren<PlayerController>();
        playerHealth = instance.GetComponentInChildren<PlayerHealth>();

        playerText = "PLAYER " + playerNumber;

        playerController.playerNumber = playerNumber;
        playerController.CharInt = CharInt;

        playerCamera.target = instance;

        playerHealth.SetUIElements(healthSlider, manaSlider, ablityImage, abilityCooldown, damageImage);
    }

    public void DisableControl()
    {
        playerController.enabled = false;
        playerHealth.enabled = false;
    }

    public void EnableControl()
    {
        playerController.enabled = true;
        playerHealth.enabled = true;
    }

    public void Reset()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}
