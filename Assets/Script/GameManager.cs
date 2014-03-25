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
	public GameObject playerControl;
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


	private Rigidbody2D playerRigid;

	void Awake(){
		Application.targetFrameRate = 60;
		playerRigid = playerControl.GetComponent<Rigidbody2D>();
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

		enemyUpdateTime = 0;
		bestScore = PlayerPrefs.GetInt("BestScore");



		playerRigid.isKinematic = false;
		playerRigid.AddForce(new Vector2(0,150));

		playerControl.GetComponent<PlayerControl>().SetScore(0);
		currentGameStatus = GameStatus.Start;
	}

	public void GameOver(){

		if(currentGameStatus == GameStatus.Over)
			return;


		playerControl.GetComponent<PlayerControl>().SetTap();
		currentGameStatus = GameStatus.Over;
		startUI.GetComponent<Animator>().SetTrigger("Show");
		playerRigid.isKinematic = true;
		playerRigid.velocity = Vector2.zero;
		playerControl.transform.position = new Vector3(-2.16f,0,0);
		PlayerPrefs.SetInt("BestScore",bestScore);

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
				playerRigid.velocity = Vector3.zero;
				playerRigid.AddForce(new Vector2(0,jumpAddForce));
				playerControl.GetComponent<PlayerControl>().playFly();
			}

		}
	}

}

