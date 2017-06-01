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
    public float gizmox1 = 0;
    public float gizmoy1 = 0;

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
    }
    
    public struct CircleLineCollisionResult
    {
        public bool Collision;
        public Vector2 Point;
        public Vector2 Normal;
        public float Distance;
    }

    public static bool CircleLineCollide(Vector2 center, float radius, Vector2 lineStart, Vector2 lineEnd, ref CircleLineCollisionResult result)
    {
        Vector2 AC = center - lineStart;
        Vector2 AB = lineEnd - lineStart;
        float ab2 = AB.sqrMagnitude;
        if (ab2 <= 0f) {
            return false;
        }
        float acab = Vector2.Dot(AC, AB);
        float t = acab / ab2;
        if (t < 0.0f) {
            t = 0.0f; //点Hが点Aの場所になる
        } else if (t > 1.0f) {
            t = 1.0f; //点Hが点Bの場所になる
        }
        result.Point = lineStart + t * AB;
        result.Normal = center - result.Point;
        float h2 = result.Normal.sqrMagnitude;
        float r2 = radius * radius;
        if (h2 > r2) {
            result.Collision = false;
        }
        else {
            result.Normal.Normalize();
            result.Collision = true;
            result.Distance = (radius - (center - result.Point).magnitude);
        }
        return result.Collision;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Vector3 to = transform.position;
        Vector3 from = new Vector3 (gizmox1,gizmoy1);
        Gizmos.DrawLine(from, to);

        Gizmos.color = Color.blue;
         to = transform.position;
         from = goru[0].transform.position;
        Gizmos.DrawLine(from, to);

        Gizmos.color = Color.red;
        to = transform.position;
        from = goru[1].transform.position;
        Gizmos.DrawLine(from, to);
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

        bool frag = false;
        if (transform.position.x < -580 && sokudox <= 0) { sokudox *= -1 * Random.Range(1.01f, 0.99f); frag = true; }
        if (transform.position.x > 1220 && sokudox >= 0) { sokudox *= -1 * Random.Range(1.01f, 0.99f); frag = true; }
        if (transform.position.y < -330 && sokudoy <= 0) { sokudoy *= -1 * Random.Range(1.01f, 0.99f); frag = true; }
        if (transform.position.y > 740 && sokudoy >= 0) { sokudoy *= -1 * Random.Range(1.01f, 0.99f); frag = true; }

        if (frag) {
            int tinpo = Random.Range(0, 4);
            if (tinpo == 3) {
                float temp = sokudox;
                sokudox = sokudoy;
                sokudoy = temp;
                            }
            if (tinpo == 2) {
                if (sokudox == 0) { sokudox = sokudoy * Random.Range(0.5f,1.0f); } else if(sokudoy == 0) { sokudoy = sokudox * Random.Range(0.5f, 1.0f); }
            }
        }

        if (sokudox != 0 || sokudoy != 0) { animator.SetFloat("speeed", 1.0f); }
        if (sokudox == 0 && sokudoy == 0) { animator.SetFloat("speeed", 0.0f); }

        if (isServer) {


            //ザ・ニュー接触判定
            CircleLineCollisionResult result = new CircleLineCollisionResult();
            CircleLineCollide(goru[0].transform.position,180, new Vector2(gizmox1, gizmoy1), transform.position, ref result);

            if (result.Collision == true ) {
                sukoa += Mathf.Abs(sokudox) + Mathf.Abs(sokudoy);
            }
            
            CircleLineCollide(goru[1].transform.position, 180, new Vector2(gizmox1, gizmoy1), transform.position, ref result);

            if (result.Collision == true) {
                sukob += Mathf.Abs(sokudox) + Mathf.Abs(sokudoy);
            }

            /*ゴールとボールの距離の判定(サーバーだけで計算する)
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
            */
            
        }
        if (transform.position.x != gizmox1 || transform.position.y != gizmoy1 ) {
            gizmox1 = transform.position.x;
            gizmoy1 = transform.position.y;
            ; }
        text.text = "あお：" + sukoa.ToString() + "てん\nあか：" + sukob.ToString() + "てん";
    }
    public void directhenko(float dx,float dy) {
        if (renzokuhit >= 1) { return; }

        //接触判定を満たした際，XY移動量に-1を掛けてバウンスさせる
        sokudox *= -1* Random.Range(1.01f,0.99f);
        sokudoy *= -1 * Random.Range(1.01f, 0.99f);

        //接触判定を満たした際，たまにXY移動量を入れ替える
        int tinpo = Random.Range(0, 4);
        if (tinpo==3){ float temp = sokudox; sokudox = sokudoy; sokudoy = temp; }

        sokudox += dx*2;
        sokudoy += dy*2;

        renzokuhit = 10;
    }
    public bool sessyokuhantei(Vector2 unko) {

        CircleLineCollisionResult result = new CircleLineCollisionResult();
        CircleLineCollide(unko, 100, new Vector2(gizmox1, gizmoy1), transform.position, ref result);

        //text.text = result.Distance.ToString();
        return result.Collision;
    }

}
