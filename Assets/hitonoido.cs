using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class hitonoido : NetworkBehaviour {

    public float speed;
    private Animator animator;

    public GameObject objA;
    public GameObject objB;

    public float dx;
    public float dy;

    public float ux;
    public float uy;

    public float distance;

    public Text text;
    // Use this for initialization
    void Start () {
        speed = 20.0F;
        objA = GameObject.FindGameObjectWithTag("unti");
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

            //これはアニメーターの制御です
            if (dx != 0 || dy != 0) { animator.SetFloat("speeed", 1.0f); }
            if (dx == 0 && dy == 0) { animator.SetFloat("speeed", 0.0f); }

            transform.Translate(dx, dy, 0.0F);

            //ボールとの距離の計算
            Vector3 Apos = objA.transform.position;
            Vector3 Bpos = transform.position;
            distance = (Apos - Bpos).sqrMagnitude;
            //text.text = distance.ToString();
            if (distance < 4000) {
                Cmduntitti();
            }

    }

    [Command]
    void Cmduntitti()
    {
        ux = dx;
        uy = dy;
        objA.GetComponent<boruunosukuriputo>().directhenko(ux, uy);
    }

}
