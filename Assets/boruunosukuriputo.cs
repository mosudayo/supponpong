using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boruunosukuriputo : MonoBehaviour {
    public float sokudox = 0.0f;
    public float sokudoy = 0.0f;
    public float gensui = 10.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        if (sokudox > 0.009) { sokudox -= gensui; if (sokudox < 0) { sokudox = 0; } }
        if (sokudox < 0.009) { sokudox += gensui; if (sokudox > 0) { sokudox = 0; } }
        if (sokudoy > 0.009) { sokudoy -= gensui; if (sokudoy < 0) { sokudoy = 0; } }
        if (sokudoy < 0.009) { sokudoy += gensui; if (sokudoy > 0) { sokudoy = 0; } }
        
        transform.Translate(sokudox, sokudoy, 0.0F);
        
    }

    public void directhenko(float dx,float dy) {
        sokudox += dx;
        sokudoy += dy;
    }
}
