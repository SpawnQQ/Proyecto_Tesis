using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S1E1_E2 : MonoBehaviour {

	public GameObject indicador;
	public Sprite imagenIndicador;
	public Sprite imagenSalida;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag=="IndicadorClickIzquierdo"){
			GlobalController.salirE1_E2 = true;
		}else if(col.gameObject.tag=="IndicadorStay"){
			indicador.GetComponent<SpriteRenderer> ().sprite = imagenSalida;
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag=="IndicadorStay"){
			indicador.GetComponent<SpriteRenderer> ().sprite = imagenIndicador;
		}
	}

}
