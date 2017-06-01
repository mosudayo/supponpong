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

        GetComponent<SpriteRenderer>().material.color = Color.green;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 to = transform.position;
        Vector3 from = objA.transform.position;
        Gizmos.DrawLine(from, to);
        
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
        }
        else if (t > 1.0f) {
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
            result.Distance = (radius - (center - result.Point).magnitude);
            result.Collision = true;
        }
        return result.Collision;
    }

    // Update is called once per frame
    void Update() {
		/*
			void Update()はホストもクライアントも両方呼ばれる。
			なのでキー入力による移動はオブジェクトオーナーのみが行う必要がある。
			そのためキー入力とTransformの反映をisLocalPlayerでくくってやるといい。
		*/
				
		// キー入力による移動は
		if ( isLocalPlayer ) {

			dx = Input.GetAxis("Horizontal");
			if ( dx < 0 ) { dx = -1F * speed; }
			if ( dx > 0 ) { dx = 1F * speed; }

			dy = Input.GetAxis("Vertical");
			if ( dy < 0 ) { dy = -1F * speed; }
			if ( dy > 0 ) { dy = 1F * speed; }

			if ( dy != 0 && dx != 0 ) { dx *= 0.8F; dy *= 0.8F; }

			//これはアニメーターの制御です
			if ( dx != 0 || dy != 0 ) { animator.SetFloat("speeed", 1.0f); }
			if ( dx == 0 && dy == 0 ) { animator.SetFloat("speeed", 0.0f); }

			transform.Translate(dx, dy, 0.0F);

            //Fire1ボタンを押しているかどうかで色を変える
            if (Input.GetButton("Fire1")) {
                GetComponent<SpriteRenderer>().material.color = Color.red;
            }
            else {
                    GetComponent<SpriteRenderer>().material.color = Color.green;
            }
        }
        else { GetComponent<SpriteRenderer>().material.color = Color.white;       }

        /*
			この位置で接触判定をするのであればクライアント側で行う必要がある。
			元々のコードではCmd関数の中でクラスのメンバ変数(dx.dy)を参照していたが
			Cmduntitti()が呼び出されるのはServer上のhitonoidoインスタンスなので
			キー入力によるdx,dyの値は同期処理も行っていないため0のままである。
			そのためボールは移動処理されなかった。
			今回はdx,dyを同期するのではなく引数で渡す形とした。
		*/

        // 接触判定をボールに投げる

        //早すぎる場合は線分で計算する
        bool unti = objA.GetComponent<boruunosukuriputo>().sessyokuhantei(transform.position);
        //unti = false;
        

        //ボールとの距離の計算
        Vector3 Apos = objA.transform.position;
		Vector3 Bpos = transform.position;
		distance = (Apos - Bpos).sqrMagnitude;
		//text.text = distance.ToString();
        
        if ( unti==true || distance < 4000 ) {
            //Fire1ボタンを押している場合，適当な数字を足して適当な数字をかけて制御不可能な速度にする
            if (Input.GetButton("Fire1")) { dx += 10; dy += 10; dx *=Random.Range(3,6) ;dy *= Random.Range(3, 6); }
            
            Cmduntitti( dx, dy ); // 関数の引数としてdx,dyを渡す
		}

    }

	[Command]
	void Cmduntitti( float powerX, float powerY )
	{
		/*
			コマンドの中は基本的にSyncVarなどを行っていない場合メンバ変数は使えない
			関数の引数などを使うようにする。
		*/
		objA.GetComponent<boruunosukuriputo>().directhenko(powerX, powerY);
	}

}
