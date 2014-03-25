using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameStatus{
		Start,
		Over
	};


	public GameObject enemy;
	public float enemyDurdingTime = 2;
	public GameObject startUI;
	public GameObject bestScoreUI;
	public GameObject currentScoreUI;
	public GameObject playerControl;
	private float enemyUpdateTime = 0;
	public GameStatus currentGameStatus = GameStatus.Start;
	public GameObject socreAnima;

	private float addScoreTime = 0;


	private int currentScore = 0;
	private int bestScore = 0;

	private string gameCenterKey = "com.royalgame.flappy2048.bestscore";
	private bool enableGameCenter = false;
	public Vector3[,] allPositions = new Vector3[3,2]{ 
		{new Vector3(3.6f,-2.16f,0),new Vector3(3.6f,2.16f,0)},
		{new Vector3(3.6f,-2.16f,0),new Vector3(3.6f,-0.72f,0)},
		{new Vector3(3.6f,2.16f,0),new Vector3(3.6f,0.72f,0)}
	};


	void Start(){
		Social.localUser.Authenticate (success => {
			if (success) {
				Debug.Log ("Authentication successful");
				string userInfo = "Username: " + Social.localUser.userName + 
					"\nUser ID: " + Social.localUser.id + 
						"\nIsUnderage: " + Social.localUser.underage;
				Debug.Log (userInfo);
				enableGameCenter = true;
			}
			else
				Debug.Log ("Authentication failed");
		});
		GameOver();
	}

	public void StartGame(){
		if(currentGameStatus == GameStatus.Start)
			return;


		enemyUpdateTime = 0;
		bestScore = PlayerPrefs.GetInt("BestScore");
		currentScore = 0;
		bestScoreUI.GetComponent<tk2dTextMesh>().text = ""+bestScore;
		currentScoreUI.GetComponent<tk2dTextMesh>().text = ""+currentScore;
		startUI.GetComponent<Animator>().SetTrigger("Hide");
//		playerControl.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//		playerControl.transform.position = new Vector3(-2.16f,0,0);
//		playerControl.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,100));
		playerControl.GetComponent<Rigidbody2D>().isKinematic = false;
		GameObject[] enemys =  GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject en in enemys){
			Destroy(en);
		}
		currentGameStatus = GameStatus.Start;
	}

	public void GameOver(){

		if(currentGameStatus == GameStatus.Over)
			return;
		playerControl.GetComponent<Rigidbody2D>().isKinematic = true;
		currentGameStatus = GameStatus.Over;
		startUI.GetComponent<Animator>().SetTrigger("Show");

		PlayerPrefs.SetInt("BestScore",bestScore);
		if(enableGameCenter){
//			Social.ReportScore(bestScore,gameCenterKey, result => {
//			});
		}
	}

	public void OnScoreCollid(){
		if(currentGameStatus == GameStatus.Start  && addScoreTime > 0.5){
			addScoreTime = 0;
			AddScore();
		}
	}


	void AddScore(){
		Debug.Log("add score");
		currentScore++;
		Instantiate(socreAnima,socreAnima.transform.position,Quaternion.identity);
		currentScoreUI.GetComponent<tk2dTextMesh>().text = ""+currentScore;
		if(currentScore> bestScore){
			bestScore = currentScore;
			bestScoreUI.GetComponent<tk2dTextMesh>().text = ""+bestScore;
		}
	}

	void CreateEmeny(){
		
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
		addScoreTime += Time.deltaTime;
	}
}

