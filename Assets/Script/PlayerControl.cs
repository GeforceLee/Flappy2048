using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public int groundAddForce = 100;
	public int jumpAddForce = 200;
	public GameObject scoreText;

	Color color2 = new Color(0xee/255.0f,0xe4/255.0f,0xda/255.0f);
	Color color4 = new Color(0xed/255.0f,0xe0/255.0f,0xc8/255.0f);
	Color color8 = new Color(0xf2/255.0f,0xb1/255.0f,0x79/255.0f);
	Color color16 = new Color(0xf5/255.0f,0x95/255.0f,0x63/255.0f);
	Color color32 = new Color(0xf6/255.0f,0x7c/255.0f,0x5f/255.0f);
	Color color64 = new Color(0xf6/255.0f,0x5e/255.0f,0x3b/255.0f);
	Color color128 = new Color(0xed/255.0f,0xcf/255.0f,0x72/255.0f);
	Color color256 = new Color(0xed/255.0f,0xcc/255.0f,0x61/255.0f);
	Color color512 = new Color(0xed/255.0f,0xc8/255.0f,0x50/255.0f);
	Color color1024 = new Color(0xed/255.0f,0xc5/255.0f,0x3f/255.0f);
	Color color2048 = new Color(0xed/255.0f,0xc2/255.0f,0x2e/255.0f);
	Color colorOther = new Color(0xed/255.0f,0xc2/255.0f,0x1f/255.0f);
	

	GameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SetScore(int score){
		Color t;
		Color textColor = new Color(1,1,1);
		if(score <= 2){
			t = color2;
			textColor = new Color(0.39f,0.35f,0.31f);
		}else if(score <= 4){
			t = color4;
			textColor = new Color(0.39f,0.35f,0.31f);
		}else if(score <= 8){
			t = color8;
		}else if(score <= 16){
			t = color16;
		}else if(score <= 32){
			t = color32;
		}else if(score <= 64){
			t = color64;
		}else if(score <= 128){
			t = color128;
		}else if(score <= 256){
			t = color256;
		}else if(score <= 512){
			t = color512;
		}else if(score <= 1024){
			t = color1024;
		}else if(score <= 2048){
			t = color2048;
		}else{
			t = colorOther;
		}
		gameObject.GetComponent<tk2dSprite>().color = t;
		string str = ""+score;
		scoreText.GetComponent<tk2dTextMesh>().text = str;
		scoreText.GetComponent<tk2dTextMesh>().color = textColor;


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
			jump = true;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) 
			jump = true;
		if(jump){
			rigidbody2D.velocity = Vector3.zero;
			Debug.Log("Jump");
			rigidbody2D.AddForce(new Vector2(0,jumpAddForce));
		}
	}
}
