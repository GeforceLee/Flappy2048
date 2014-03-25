using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public int groundAddForce = 110;
	public int jumpAddForce = 200;

	GameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}





	void OnTriggerEnter2D(Collider2D coll){
		bool gameOver = false;
		if(coll.gameObject.name == "Floor"){
			rigidbody2D.velocity = Vector3.zero;
			gameOver = true;
			rigidbody2D.AddForce(new Vector2(0,groundAddForce));
		}else if (coll.gameObject.name == "Celiling"){
			rigidbody2D.velocity = Vector3.down;
			gameOver = true;
		}else if(coll.gameObject.tag == "Enemy"){
			gameOver = true;
		}
		if(gameOver)
			gameManager.GameOver();
	}


	void FixedUpdate(){
		if(gameManager.currentGameStatus == GameManager.GameStatus.Over)
			return;
		bool jump = false;
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
			Debug.Log("Jump");
			jump = true;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) 
			jump = true;
		if(jump){
			rigidbody2D.velocity = Vector3.zero;
			rigidbody2D.AddForce(new Vector2(0,jumpAddForce));
		}
	}
}
