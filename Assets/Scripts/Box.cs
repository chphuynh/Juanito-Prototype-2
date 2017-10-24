using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {

	public string boxNumber;

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}

	void Update()
	{
		//Debug.Log(transform.position);
		// switch(boxNumber) {
		// 	case "1":
		// 		if(transform.position.x == 6.5f && transform.position.y == 2.5f){
		// 			Debug.Log(true);
		// 			gameObject.SetActive(false);
		// 		}
		// 		break;
		// 	case "2":
		// 		if(transform.position == new Vector3(-6.5f, -0.5f, -1f)) gameObject.SetActive(false);
		// 		break;
		// 	default:
		// 		break;
		// }

	}
	
}
