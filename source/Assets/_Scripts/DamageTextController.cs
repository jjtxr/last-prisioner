using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextController : MonoBehaviour {

    public DamageText damageText;
    public GameObject canvas;

	void Start () {
        canvas = GameObject.Find("GameCanvas");
	}
	
	public void CreateFloatingText (int amount, Transform location) {
        DamageText instance = Instantiate(damageText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location.position);
        
        instance.transform.SetParent(canvas.transform);
        instance.transform.position = screenPosition;
        instance.SetText(amount);
	}
}
