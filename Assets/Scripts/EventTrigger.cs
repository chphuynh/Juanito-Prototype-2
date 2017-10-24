using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventTrigger : MonoBehaviour {

    public string eventType;
    
    void Start()
    {
		GetComponent<Renderer>().enabled = false;
	}
}
