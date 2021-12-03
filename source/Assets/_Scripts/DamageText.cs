using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    public Animator anim;
    public Text damageText;

    void Start () {
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        damageText = anim.GetComponent<Text>();
        Destroy(gameObject, clipInfo[0].clip.length);
    }
	
	public void SetText (int amount) {
        damageText.text = amount.ToString();
	}
}
