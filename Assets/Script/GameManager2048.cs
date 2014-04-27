using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class GameManager2048 : MonoBehaviour {

	private int currentScore = 0;
	private int hightestScore = 0;

	public GameObject tile;

	private Game _game;

	public GameObject scoreText;
	public GameObject hScoreText;

	public GameObject bgObject;

	public GameObject UI;

	Vector3 firstPostion;
	float tileBetween = 1.44f;

	public MyDirection lastDirection;

	public bool enableGameCenter = false;
	
	public float staticAngle = 40.0f;
	
	public AudioClip audio2_4;
	public AudioClip audio8_16_32_64;
	public AudioClip audio128_256_512;
	public AudioClip audio1024_2048;
	public AudioClip audioGame_Over;
	public AudioClip audioOff;


	public GameObject scorePer;

	float prevTime = 0.0f;
	public float moveDuring = 0.2f;

	[HideInInspector]
	public bool sound;



	// Use this for initialization
	void Start () {
		Social.localUser.Authenticate (ProcessAuthentication);
		sound = PlayerPrefs.GetInt("sound") == 0?true:false;
	}

	void ProcessAuthentication (bool success) {
		if (success) {
//			Debug.Log ("Authenticated, checking achievements");
			enableGameCenter = true;
			// Request loaded achievements, and register a callback for processing them

		}
//		else
//			Debug.Log ("Failed to authenticate");
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
	}
	void Awake(){

		
		bgObject =  GameObject.Find("BG1") as GameObject;
		

		float x = bgObject.transform.position.x-2.16f;
		
		float y = bgObject.transform.position.y+2.16f;
		
		firstPostion = new Vector3(x,y,0f);
//		StartGame();
		LoadGame();
//		showUI();
	}

	public void StartGame(){
		currentScore = 0;
		GameObject[] allTile = GameObject.FindGameObjectsWithTag("Tile");
		for(int i = 0;i<allTile.Length;i++){
			Destroy(allTile[i]);
		}
		int tempHightestScore = PlayerPrefs.GetInt("HightestScore");
		hightestScore = tempHightestScore;
		hScoreText =  GameObject.Find("HightestScore1") as GameObject;
		hScoreText.GetComponent<tk2dTextMesh>().text = tempHightestScore.ToString();
		scoreText = GameObject.Find("Score1") as GameObject;
		scoreText.GetComponent<tk2dTextMesh>().text = "0";
		_game = gameObject.GetComponent<Game>();
		_game.setup(4);
		hideUI();
		prevTime = moveDuring;

	}

	public void BackToStart(){
		GameObject[] ads = GameObject.FindGameObjectsWithTag("AD");
		foreach(GameObject ad in ads){
			ad.SetActive(false);
		}
		Application.LoadLevel("StartScene");
	}
	public void LoadGame(){
		string save  = Load();
		Debug.Log("load:"+save);
		if(save == null){
			StartGame();
		}else{
			string deStr =  PlayerPrefs.GetString("Score");
			string dStr = StringEncryption.DecryptDES(deStr,"-#sd~cn3");
			if(deStr != dStr){
				currentScore = int.Parse(dStr);
			}else{
				currentScore = 0;
			}
			int tempHightestScore = PlayerPrefs.GetInt("HightestScore");
			hightestScore = tempHightestScore;
			hScoreText =  GameObject.Find("HightestScore1") as GameObject;
			hScoreText.GetComponent<tk2dTextMesh>().text = tempHightestScore.ToString();
			scoreText = GameObject.Find("Score1") as GameObject;
			scoreText.GetComponent<tk2dTextMesh>().text = currentScore.ToString();
			_game = gameObject.GetComponent<Game>();
			_game.load(4,save,currentScore);
			hideUI();
			prevTime = moveDuring;
		}
	}
	public Vector3 getTilePosition(int x,int y,int z=0){
		Vector3 result = new Vector3(firstPostion.x+x*tileBetween,firstPostion.y- y*tileBetween,z);
		
		return result;
	}


	void option(bool mov){
		if(!mov){
			// have not mov  play audio
			audio.clip = audioOff;
			if(sound){
				audio.Play();
			}

			return;
		}
		GameObject[] gameobjects =  GameObject.FindGameObjectsWithTag("TileText");

		int maxValue = 0;
		GamePostion vector = _game.getVector(lastDirection);
		Hashtable traversals = _game.buildTraversals(vector);
		List<int> xList = (List<int>)traversals["x"];
		List<int> yList = (List<int>)traversals["y"];
		for(int ii=0;ii<4;ii++){
			int i = xList[ii];
			for(int jj=0;jj<4;jj++){
				int j = yList[jj];

				Tile t = _game.grid.cells[i,j];
				if(t != null){
					GameObject newTile;
					if(t.mergedFrom != null){
						int x1 = (int)t.mergedFrom[0].previousPosition["x"];
						int y1 = (int)t.mergedFrom[0].previousPosition["y"];
						GameObject perTile1 = GameObject.Find("Tile"+x1+y1);
						perTile1.GetComponent<TIleScript>().move(getTilePosition(i,j,1),t.mergedFrom[0].value);
						perTile1.name = "Tile";
						Destroy(perTile1,0.1f);
						int x2 = (int)t.mergedFrom[1].previousPosition["x"];
						int y2 = (int)t.mergedFrom[1].previousPosition["y"];
						GameObject perTile2 = GameObject.Find("Tile"+x2+y2);
						perTile2.GetComponent<TIleScript>().move(getTilePosition(i,j,0),t.value);
						perTile2.name = "Tile"+i+j;;

						if(t.value>maxValue)
							maxValue = t.value;
					}else{
						if(t.previousPosition != null){
							int x = (int)t.previousPosition["x"];
							int y = (int)t.previousPosition["y"];
							newTile = GameObject.Find("Tile"+x+y);
							newTile.GetComponent<TIleScript>().move(getTilePosition(i,j),t.value);
							newTile.name = "Tile"+i+j;
//							Debug.Log("move  form change yuan x:"+x+" y:"+y +"  value:"+t.value +"  xian:"+"Tile"+i+j);
						}else{
							{
								newTile = Instantiate(tile,getTilePosition(i,j),Quaternion.identity) as GameObject;
								newTile.GetComponent<TIleScript>().setCurrentValue(t.value);
								newTile.name = "Tile"+i+j;
//								Debug.Log("new   x:"+i+" y:"+j +" value:"+t.value);
							}
						}
					}
				}

			}
		}

		if(sound){
			if(maxValue == 0){
				
			}else if(maxValue == 2 || maxValue == 4 ){
				audio.clip = audio2_4;
				audio.Play();
			}else if(maxValue == 8||maxValue == 16 || maxValue == 32|| maxValue == 64){
				audio.clip = audio8_16_32_64;
				audio.Play();
			}else if(maxValue == 128||maxValue == 256 || maxValue == 512){
				audio.clip = audio128_256_512;
				audio.Play();
			}else {
				audio.clip = audio1024_2048;
				audio.Play();
			}
		}

		
		int addScore = _game.score - currentScore;
		if (addScore > 0) {
			GameObject go = Instantiate(scorePer,scorePer.transform.position,Quaternion.identity) as GameObject;
			go.GetComponent<tk2dTextMesh>().text = "+"+addScore;
		}
		currentScore = _game.score;

		PlayerPrefs.SetString("Score",StringEncryption.EncryptDES(currentScore.ToString(),"-#sd~cn3"));

		if (currentScore > hightestScore) {

			PlayerPrefs.SetInt("HightestScore",currentScore);
			hightestScore = currentScore;
			hScoreText =  GameObject.Find("HightestScore1") as GameObject;
			hScoreText.GetComponent<tk2dTextMesh>().text = hightestScore.ToString();
			if(enableGameCenter){
				Social.ReportScore(hightestScore,"com.royalgame.flappy2048.2048", result => {
//					if (result)
//						Debug.Log ("Successfully reported achievement progress");
//					else
//						Debug.Log ("Failed to report achievement");
				});
			}

		}
		scoreText = GameObject.Find("Score1") as GameObject;
		scoreText.GetComponent<tk2dTextMesh>().text = currentScore.ToString();


		if(_game.over){
			showUI();
			if(sound){
				StartCoroutine(playGameOverAudio());
			}

		}else{
			string s = _game.grid.GameSave();
			Save(s);
		}
	}

	IEnumerator playGameOverAudio(){
		yield return new WaitForSeconds(0.5f);
		audio.clip = audioGame_Over;
		audio.Play();
	}

	public void showUI(){
		UI = GameObject.Find("StartUI1") as GameObject;
		UI.GetComponent<Animator>().SetTrigger("Show");
	}
	public void hideUI(){
		UI = GameObject.Find("StartUI1") as GameObject;
		UI.GetComponent<Animator>().SetTrigger("Hide");
	}

	public void showGameCenterScore(){
		if(enableGameCenter){
			Social.ShowLeaderboardUI();
		}
	}

	void OnSwipe( SwipeGesture gesture ) 
	{
		if(_game.over){ return; }
		FingerGestures.SwipeDirection direction = gesture.Direction;
		bool isMoved = false;
		if(Vector2.Angle(gesture.Move,Vector2.right) < staticAngle){
			lastDirection = MyDirection.DirectionRight;
			isMoved = true;
		}else if(Vector2.Angle(gesture.Move,Vector2.up) < staticAngle){
			lastDirection = MyDirection.DirectionUp;
			isMoved = true;
		}else if(Vector2.Angle(gesture.Move,Vector2.right) >  180 - staticAngle){
			lastDirection = MyDirection.DirectionLeft;
			isMoved = true;
		}else if(Vector2.Angle(gesture.Move,Vector2.up) > 180 - staticAngle){
			lastDirection = MyDirection.DirectionDown;
			isMoved = true;
		}
		if(isMoved){
			_game.move(lastDirection);
		}
	}

	void FixedUpdate(){
		if(_game.over ) { return; }
		bool isMoved = false;

		if(Input.GetKeyDown("right")){
			lastDirection = MyDirection.DirectionRight;
			isMoved = true;
		}else if(Input.GetKeyDown("up")){
			lastDirection = MyDirection.DirectionUp;
			isMoved = true;
		}else if(Input.GetKeyDown("left")){
			lastDirection = MyDirection.DirectionLeft;
			isMoved = true;
		}else if(Input.GetKeyDown("down")){
			lastDirection = MyDirection.DirectionDown;
			isMoved = true;
		}
		prevTime+= Time.deltaTime;
		if(isMoved){
			Debug.Log(prevTime);
			if(prevTime >= moveDuring){
				_game.move(lastDirection);
				prevTime = 0;
			}


		}


		if ( Application.platform == RuntimePlatform.Android &&(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home) ))
		{
//			OnApplicationQuit();
			Application.Quit();
		}

	}



	public void Save(string gs){
		string path = Application.persistentDataPath+"//data";

		StreamWriter sw;
		FileInfo t = new FileInfo(path);
		
		//		if(!t.Exists)
		//		{
		//			//如果此文件不存在则创建
		sw = t.CreateText();
		//		}
		//		else
		//		{
		//			//如果此文件存在则打开
		//
		//			sw = t.AppendText();
		//		}
		//以行的形式写入信息
		string egs = StringEncryption.EncryptDES(gs,"-#sd~cn3");
		sw.WriteLine(egs);
		//关闭流
		sw.Close();
		//销毁流
		sw.Dispose();


	}

	public string Load(){
		//使用流的形式读取
		string res = null;
		StreamReader sr =null;
		try{
			string path = Application.persistentDataPath+"//data";
			Debug.Log(path);
			sr = File.OpenText(path);
		}catch(Exception e)
		{
			//路径与名称未找到文件则直接返回空
			return null;
		}
		string line = sr.ReadLine();
		if(line == null){
			return null;
		}else{
			string degs = StringEncryption.DecryptDES(line,"-#sd~cn3");
			if(line != degs){
				res = degs;
			}
		}
		//关闭流
		sr.Close();
		//销毁流
		sr.Dispose();
		//将数组链表容器返回
		return res;
	}


}
