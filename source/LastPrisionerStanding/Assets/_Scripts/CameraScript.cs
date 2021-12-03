using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject target;
    public float distanceX;
    public float distanceY;
    public float distanceZ;
    public float camSpeed;

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(distanceX, distanceY, distanceZ), camSpeed * Time.fixedDeltaTime);
            
        }
    }
}
