using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	enum GameStatus{
		Start,
		Over
	};


	public GameObject enemy;

	public float enemyDurdingTime = 2;
	private float enemyUpdateTime = 0;
	private GameStatus currentGameStatus = GameStatus.Over;

	public Vector3[,] allPositions = new Vector3[3,2]{ 
		{new Vector3(3.6f,-2.16f,0),new Vector3(3.6f,2.16f,0)},
		{new Vector3(3.6f,-2.16f,0),new Vector3(3.6f,-0.72f,0)},
		{new Vector3(3.6f,2.16f,0),new Vector3(3.6f,0.72f,0)}
	};


	void Start(){
		StartGame();
	}

	void StartGame(){

		currentGameStatus = GameStatus.Start;
		enemyUpdateTime = 0;

	}

	void CreateEmeny(){
		Debug.Log("Create Emeny");
		int randomIndex = Random.Range(0,3);
		Vector3 postion1 = allPositions[randomIndex,0];
		Vector3 postion2 = allPositions[randomIndex,1];

		Instantiate(enemy,postion1, Quaternion.identity);
		Instantiate(enemy,postion2, Quaternion.identity);
	}

	void Update(){

		if(currentGameStatus == GameStatus.Start){
//			Debug.Log(enemyUpdateTime);
			enemyUpdateTime += Time.deltaTime;
			if(enemyUpdateTime > enemyDurdingTime){
				CreateEmeny();
				enemyUpdateTime = 0;
			}
		}
	}
}

