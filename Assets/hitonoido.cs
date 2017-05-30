using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hitonoido : MonoBehaviour {

    public float speed;
    private Animator animator;

    public GameObject objA;
    public GameObject objB;

    public float dx;
    public float dy;

    public Text text;
    // Use this for initialization
    void Start () {
        speed = 20.0F;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update() {
        
         dx = Input.GetAxis("Horizontal");
        if (dx < 0) { dx = -1F * speed; }
        if (dx > 0) { dx = 1F * speed; }

         dy = Input.GetAxis("Vertical");
        if (dy < 0) { dy = -1F * speed; }
        if (dy > 0) { dy = 1F * speed; }

        if (dy != 0 && dx != 0) { dx *= 0.8F; dy *= 0.8F; }
        if (dx != 0 || dy != 0) { animator.SetFloat("speeed", 1.0f); }
        if (dx == 0 && dy == 0) { animator.SetFloat("speeed", 0.0f); }

        transform.Translate(dx, dy, 0.0F);

        Vector3 Apos = objA.transform.position;
        Vector3 Bpos = transform.position;
        float distance = (Apos - Bpos).sqrMagnitude;
        text.text = distance.ToString();
        if (distance < 1000) {
         objA.GetComponent<boruunosukuriputo>().directhenko(dx,dy);
        }

    }
}
