using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour {

	public static GlobalController globalController;

	void Awake(){
		if(globalController==null){
			globalController = this;
			DontDestroyOnLoad (gameObject);
		}else if(globalController!=this){
			Destroy (gameObject);
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
