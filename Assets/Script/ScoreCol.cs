using UnityEngine;
using System.Collections;

public class ScoreCol : MonoBehaviour {
	public GameObject gameManagerGO;
	GameManager gameManager;
	// Use this for initialization



	void Start () {
		gameManager = gameManagerGO.GetComponent<GameManager>();
	}
	
	// Update is called once per frame


	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag == "Enemy"){
			
				gameManager.OnScoreCollid();


		}
	}
}
