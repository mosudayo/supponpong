using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class yokattane : MonoBehaviour {

    float x = 0;
    float y = 0;

	// Use this for initialization
	void Start () {
        x = Random.Range(-10, 10);
        y = Random.Range(-10, 10);
        Destroy(gameObject,1.0f) ;
    }
	
	// Update is called once per frame
	void Update () {

        transform.Translate(x, y, 0.0F);
    }
}
