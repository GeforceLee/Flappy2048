using UnityEngine;
using System.Collections;

public class BackBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Click(){
		GameObject man =  GameObject.FindGameObjectWithTag("Manager2048") as GameObject;
		GameManager2048 m2 = man.GetComponent<GameManager2048>();
		m2.BackToStart();
	}


	public void StartGame(){
		GameObject man =  GameObject.FindGameObjectWithTag("Manager2048") as GameObject;
		GameManager2048 m2 = man.GetComponent<GameManager2048>();
		m2.StartGame();
	}

	public void ShowTop(){
		GameObject man =  GameObject.FindGameObjectWithTag("Manager2048") as GameObject;
		GameManager2048 m2 = man.GetComponent<GameManager2048>();
		m2.showGameCenterScore();
	}
}
