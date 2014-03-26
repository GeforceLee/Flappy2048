using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum GameStatus{
		Start,
		Over,
		Ready
	};


	public GameObject enemy;
	public float enemyDurdingTime = 2;
	public GameObject startUI;
	public GameObject bestScoreUI;
	public GameObject currentScoreUI;
	private GameObject playerControl;
	public GameObject playerControlPer;
	private GameObject playerFly;
	public GameObject playerFlyPer;
	private float enemyUpdateTime = 0;
	public GameStatus currentGameStatus = GameStatus.Start;
	public GameObject socreAnima;
	public int jumpAddForce = 200;
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

	private AudioSource audioSource;
	public AudioClip audioPlay;
	public AudioClip audioPoint;

	private Vector3 startPosition = new Vector3(-2.16f,0,0);

	void Awake(){
		Application.targetFrameRate = 60;
	}
	void Start(){
		

		string channel = null;
		string mta_appkey = null;
#if UNITY_IPHONE
		channel = "iOS";
		mta_appkey = "Aqc1101259487";
#elif UNITY_ANDROID
		channel = "Android";
		mta_appkey = "Aqc1101259487";
#endif
		MtaService.SetInstallChannel(channel);
		MtaService.StartStatServiceWithAppKey(mta_appkey);

		audioSource = gameObject.GetComponent<AudioSource>();
		bestScore = PlayerPrefs.GetInt("BestScore");
		currentScoreUI.GetComponent<tk2dTextMesh>().text = "0";
		bestScoreUI.GetComponent<tk2dTextMesh>().text = ""+bestScore;
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

	public void ReadyGame(){
		if(currentGameStatus != GameStatus.Over)
			return;

		audioSource.clip = audioPlay;
		audioSource.Play();
		currentGameStatus = GameStatus.Ready;
		startUI.GetComponent<Animator>().SetTrigger("Hide");
		currentScore = 0;
		bestScoreUI.GetComponent<tk2dTextMesh>().text = ""+bestScore;
		currentScoreUI.GetComponent<tk2dTextMesh>().text = ""+currentScore;

	}

	public void StartGame(){
		if(currentGameStatus == GameStatus.Start)
			return;

		if(playerFly !=null)
			Destroy(playerFly);

		playerControl = Instantiate(playerControlPer,startPosition,Quaternion.identity) as GameObject;
		playerControl.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,150));
		enemyUpdateTime = 0;
		bestScore = PlayerPrefs.GetInt("BestScore");

		playerControl.GetComponent<PlayerControl>().SetScore(0);
		currentGameStatus = GameStatus.Start;
	}

	public void GameOver(){

		if(currentGameStatus == GameStatus.Over)
			return;


		currentGameStatus = GameStatus.Over;
		startUI.GetComponent<Animator>().SetTrigger("Show");

		if(playerControl !=null)
			Destroy(playerControl);

		PlayerPrefs.SetInt("BestScore",bestScore);
		playerFly = Instantiate(playerFlyPer,startPosition,Quaternion.identity) as GameObject;


		GameObject[] enemys =  GameObject.FindGameObjectsWithTag("Enemy");
		foreach(GameObject en in enemys){
			Destroy(en);
		}

		if(enableGameCenter){
			Social.ReportScore(bestScore,gameCenterKey, result => {
			});
		}
	}

	public void showGameCenterScore(){
		if(enableGameCenter){
			Social.ShowLeaderboardUI();
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
		audioSource.clip = audioPoint;
		audioSource.Play();
		Instantiate(socreAnima,socreAnima.transform.position,Quaternion.identity);
		currentScoreUI.GetComponent<tk2dTextMesh>().text = ""+currentScore;
		playerControl.GetComponent<PlayerControl>().SetScore(currentScore);
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
			enemyUpdateTime += Time.deltaTime;
			if(enemyUpdateTime > enemyDurdingTime){
				CreateEmeny();
				enemyUpdateTime = 0;
			}
		}
		addScoreTime += Time.deltaTime;
		if(currentGameStatus == GameStatus.Over)
			return;

		bool jump = false;
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space"))
			jump = true;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) 
			jump = true;
		if(jump){
			if(currentGameStatus == GameStatus.Ready){
				StartGame();
			}else{
				playerControl.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
				playerControl.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,jumpAddForce));
				playerControl.GetComponent<PlayerControl>().playFly();
			}

		}
	}

}

