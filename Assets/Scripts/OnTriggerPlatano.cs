using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerPlatano : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag=="IndicadorClick"){
			GlobalController.pObjetoX = this.gameObject.transform.position.x;
			GlobalController.pObjetoY = (this.gameObject.transform.position.y - (this.gameObject.GetComponent<SpriteRenderer> ().sprite.pivot.y / 2));
			GlobalController.onTriggerObjeto = true;
			GlobalController.objetoClickeado = this.gameObject;
		}else if(col.gameObject.tag=="IndicadorStay"){
			Debug.Log ("Activar: "+this.gameObject.name);
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag=="IndicadorStay"){
			Debug.Log ("Desactivar: "+this.gameObject.name);
		}
	}
}
