using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTrainningController : MonoBehaviour {

    public CameraScript camScript;
    public Transform spawnPositon;
    public Image saImage;

    public GameObject[] characters;
    public Sprite[] specialAttacks;

	void Start () {

        GameObject characterInst = Instantiate(characters[LoadCharacter()],spawnPositon.position,Quaternion.identity);
        saImage.sprite = specialAttacks[LoadCharacter()];
        camScript.target = characterInst;
	}

    int LoadCharacter()
    {
        int cs = 0;
        if (PlayerPrefs.HasKey("CS"))
        {
            cs = PlayerPrefs.GetInt("CS");
            PlayerPrefs.DeleteKey("CS");
        }

        return cs;
    }
}
