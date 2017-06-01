using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class boruunosukuriputo : NetworkBehaviour
{
    [SyncVar]
    public float sokudox = 0.0f;
    [SyncVar]
    public float sokudoy = 0.0f;
    //スコア集計用
    [SyncVar]
    public float sukoa = 0;
    [SyncVar]
    public float sukob = 0;

    //スコア表示用
    public Text text;

    public float gensui = 10.0f;
    public int renzokuhit = 5;

    public float distance;
    
    //ゴール用のゲームオブジェクトを（複数持つ可能性があるので）配列で作成
    public GameObject[] goru;

    private Animator animator;

    // 距離計算用
    float gizmox1 = 0;
    float gizmoy1 = 0;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 to = new Vector3(gizmox1, gizmoy1,0);
        Vector3 from = transform.position;
        Gizmos.DrawLine(from, to);
        gizmox1 = to.x;
        gizmoy1 = to.y;
    }
        // Update is called once per frame
        void Update()
    {
        renzokuhit -= 1;

        if (sokudox > 0.009) { sokudox -= gensui; if (sokudox < 0) { sokudox = 0; } }
        if (sokudox < 0.009) { sokudox += gensui; if (sokudox > 0) { sokudox = 0; } }
        if (sokudoy > 0.009) { sokudoy -= gensui; if (sokudoy < 0) { sokudoy = 0; } }
        if (sokudoy < 0.009) { sokudoy += gensui; if (sokudoy > 0) { sokudoy = 0; } }

        transform.Translate(sokudox, sokudoy, 0.0F);

        if (transform.position.x < -580 && sokudox <= 0) { sokudox *= -1; }
        if (transform.position.x > 1220 && sokudox >= 0) { sokudox *= -1; }
        if (transform.position.y < -330 && sokudoy <= 0) { sokudoy *= -1; }
        if (transform.position.y > 740 && sokudoy >= 0) { sokudoy *= -1; }

        if (sokudox != 0 || sokudoy != 0) { animator.SetFloat("speeed", 1.0f); }
        if (sokudox == 0 && sokudoy == 0) { animator.SetFloat("speeed", 0.0f); }

        if (isServer) {

            OnDrawGizmos();

            //ゴールとボールの距離の判定(サーバーだけで計算する)
            Vector3 Apos = goru[0].transform.position;
            Vector3 Bpos = transform.position;
            //bool fragu = false;
            distance = (Apos - Bpos).sqrMagnitude;
            //text.text = distance.ToString();
            if (distance < 11000) {
                sukoa += Mathf.Abs(sokudox) + Mathf.Abs(sokudoy);
              //  fragu = true;
            }

            Apos = goru[1].transform.position;
            distance = (Apos - Bpos).sqrMagnitude;
            if (distance < 11000) {
                sukob += Mathf.Abs(sokudox) + Mathf.Abs(sokudoy);
                // fragu = true;                
            }

        }
        text.text = "あお：" + sukoa.ToString() + "てん\nあか：" + sukob.ToString() + "てん";

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
