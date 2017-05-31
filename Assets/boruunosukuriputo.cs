using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class boruunosukuriputo : NetworkBehaviour
{
    [SyncVar]
    public float sokudox = 0.0f;
    [SyncVar]
    public float sokudoy = 0.0f;

    public float gensui = 10.0f;
    public int renzokuhit = 5;

    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        renzokuhit -= 1;

        if (sokudox > 0.009) { sokudox -= gensui; if (sokudox < 0) { sokudox = 0; } }
        if (sokudox < 0.009) { sokudox += gensui; if (sokudox > 0) { sokudox = 0; } }
        if (sokudoy > 0.009) { sokudoy -= gensui; if (sokudoy < 0) { sokudoy = 0; } }
        if (sokudoy < 0.009) { sokudoy += gensui; if (sokudoy > 0) { sokudoy = 0; } }
        
        transform.Translate(sokudox, sokudoy, 0.0F);
        
        if (transform.position.x < -580 && sokudox <= 0) { sokudox *= -1; }
        if (transform.position.x > 1220 && sokudox >= 0) { sokudox *= -1; }
        if (transform.position.y < -330 && sokudoy <= 0) { sokudoy *= -1; }
        if (transform.position.y > 740  && sokudoy >= 0) { sokudoy *= -1; }
        
        if (sokudox != 0 || sokudoy != 0) { animator.SetFloat("speeed", 1.0f); }
        if (sokudox == 0 && sokudoy == 0) { animator.SetFloat("speeed", 0.0f); }

    }

    public void directhenko(float dx,float dy) {
        if (renzokuhit >= 1) { return; }

        sokudox *= -1;
        sokudoy *= -1;

        sokudox += dx*2;
        sokudoy += dy*2;

        renzokuhit = 10;
    }
}
