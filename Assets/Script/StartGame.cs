using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	public GameObject sound;
	public bool enableGameCenter = false;
	void Start () {
		tk2dUIToggleButton bt = sound.GetComponent<tk2dUIToggleButton>();

		bool soundEnable = PlayerPrefs.GetInt("sound") == 0?true:false;
		bt.IsOn = soundEnable;
		Social.localUser.Authenticate (ProcessAuthentication);
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

	public void StartFlappy(){
		Debug.Log("StartFlappy");
		Application.LoadLevel("Sence1");
	}

	public void Start2048(){
		Debug.Log("Start2048");
		Application.LoadLevel("2048");
	}
	public void Top(){
		if(enableGameCenter){
			Social.ShowLeaderboardUI();
		}

	}

	public void SoundBtnClick(tk2dUIToggleButton a){


		if(a.IsOn){
			PlayerPrefs.SetInt("sound",0);
		}else{

			PlayerPrefs.SetInt("sound",1);
		}
	}


	public void RateGame(){
		string url =   @"https://userpub.itunes.apple.com/WebObjects/MZUserPublishing.woa/wa/addUserReview?type=Purple+Software&id=845764602&mt=8&o=i";
		Application.OpenURL(url);
	}
	public void MoreGame(){
		string url =   @"https://userpub.itunes.apple.com/WebObjects/MZUserPublishing.woa/wa/addUserReview?type=Purple+Software&id=694610741&mt=8&o=i";
		Application.OpenURL(url);
	}
}
