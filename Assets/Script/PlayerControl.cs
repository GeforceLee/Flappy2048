using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public int groundAddForce = 200;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.name == "Floor"){
			rigidbody2D.velocity = Vector3.zero;
//			Debug.Log("Floor");
			rigidbody2D.AddForce(new Vector2(0,groundAddForce));
		}else if (coll.gameObject.name == "Celiling"){
			rigidbody2D.velocity = Vector3.down;
		}else if(coll.gameObject.name == "Enemy"){
//			Debug.Log("si le");
		}
	}


	void FixedUpdate(){
		bool jump = false;
		if (Input.GetMouseButtonUp(0))
			jump = true;

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) 
			jump = true;
		if(jump){
			rigidbody2D.velocity = Vector3.zero;
			rigidbody2D.AddForce(new Vector2(0,groundAddForce));
		}
			

	}

}
