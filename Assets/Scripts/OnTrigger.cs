using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag=="Objeto"){
			Debug.Log ("Entro a: "+col.gameObject.name);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag=="Objeto"){
			Debug.Log ("Salio de: "+col.gameObject.name);
		}
	} 
}
