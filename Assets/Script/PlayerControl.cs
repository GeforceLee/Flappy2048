using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	

	public GameObject scoreText;

	public AudioClip audioHit;
	public AudioClip audioFly;

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
	
	private AudioSource audioSource;
	GameManager gameManager;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		audioSource = gameObject.GetComponent<AudioSource>();
	}
	


	public void SetTap(){
		gameObject.GetComponent<tk2dSprite>().color = color2;
		scoreText.GetComponent<tk2dTextMesh>().text = "Tap";
		scoreText.GetComponent<tk2dTextMesh>().color = new Color(0.39f,0.35f,0.31f);
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

	public void playFly(){
		if(audioSource.isPlaying)
			return;
		audioSource.clip = audioFly;
		audioSource.Play();
	}
	void OnTriggerEnter2D(Collider2D coll){
		bool gameOver = false;
		if(coll.gameObject.name == "Floor"){
			rigidbody2D.velocity = Vector3.zero;
			gameOver = true;
			
		}else if (coll.gameObject.name == "Celiling"){
			rigidbody2D.velocity = Vector3.down;
			gameOver = true;
		}else if(coll.gameObject.tag == "Enemy"){
			gameOver = true;
		}
		if(gameOver){
			audioSource.clip = audioHit;
			audioSource.Play();
			gameManager.GameOver();
		}
	}


}
