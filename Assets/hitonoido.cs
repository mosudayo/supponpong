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
		}

		/*
			この位置で接触判定をするのであればクライアント側で行う必要がある。
			元々のコードではCmd関数の中でクラスのメンバ変数(dx.dy)を参照していたが
			Cmduntitti()が呼び出されるのはServer上のhitonoidoインスタンスなので
			キー入力によるdx,dyの値は同期処理も行っていないため0のままである。
			そのためボールは移動処理されなかった。
			今回はdx,dyを同期するのではなく引数で渡す形とした。
		*/

		//ボールとの距離の計算
		Vector3 Apos = objA.transform.position;
		Vector3 Bpos = transform.position;
		distance = (Apos - Bpos).sqrMagnitude;
		//text.text = distance.ToString();
		if ( distance < 4000 ) {
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
