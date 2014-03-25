using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public int deadTime = 3;

	private Vector3 targetPosition;
	private float updateTime  = 0;

	private Vector3 startPosition;
	// Use this for initialization
	void Start () {
		startPosition = gameObject.transform.position;
		targetPosition = new Vector3(-3.6f,startPosition.y,startPosition.z);
		Destroy(gameObject,deadTime);
	}
	
	// Update is called once per frame
	void Update () {
		updateTime += Time.deltaTime;

		gameObject.transform.position = Vector3.Lerp(startPosition,targetPosition,updateTime/deadTime);
	}
}
