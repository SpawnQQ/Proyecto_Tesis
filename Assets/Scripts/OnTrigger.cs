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
		Debug.Log (col.gameObject);

		if(col.gameObject.tag=="Objeto"){
			this.gameObject.GetComponent<SpriteRenderer> ().sortingLayerName = "Player";
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag=="Objeto"){
			this.gameObject.GetComponent<SpriteRenderer> ().sortingLayerName = "PrimerPlano";
		}
	} 
}
