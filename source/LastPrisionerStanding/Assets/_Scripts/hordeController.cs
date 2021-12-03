using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hordeController : MonoBehaviour
{
    public int id, hNum;
    public GameObject[] hordes;

    private GameController gameController;

    private void Start()
    {
        hNum = hordes.Length;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update ()
    {
        if (hNum == 0)
            gameController.DestroyWall(id);
	}
}
