﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public float speed;
	public GameObject indicador;
	public GameObject player;
	public GameObject b;

	bool moving;
	Vector2 target;
	public Text textoRaton;

	float secondsCounter;
	float tiempo_texto=3;
	bool hablaRaton;
	 
	private DBConnector _connector;

	MenuController menuController;

	void Start () {
		moving = false;
		hablaRaton = false;
		secondsCounter=0;

		Debug.Log("Datos usuario: "+GlobalController.ID+", "+GlobalController.NAME+", "+GlobalController.TYPE);
	}
		
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			SceneManager.LoadScene ("Menu");
		} else {
			movimientoRaton ();
		}
	}
		
	void movimientoRaton (){
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

		//Se puede asignar un objeto "Raton" y asignarlo en vez de asumir que raton es el propietario de este script
		Vector2 originPosition = new Vector2 (player.transform.position.x, player.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast (originPosition, mousePosition - originPosition,Vector2.Distance(originPosition,mousePosition));

		Debug.DrawLine (originPosition, mousePosition, Color.green);
		indicador.transform.position = new Vector2(mousePosition.x,mousePosition.y);

		//Debug.Log (hit.collider.tag);
		if (hit.collider != null) {
			Debug.DrawLine (hit.point,mousePosition , Color.red);
			Debug.DrawLine (originPosition, hit.point, Color.green);

		}

		if (Input.GetMouseButtonDown (0)) {

			//Al mover, se modifica la posicion z del objeto raton e indicador
			target = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

			rotarObjeto (player,originPosition.x,target.x);

			if (hit.collider != null) {

				if (hit.collider.CompareTag ("Obstaculo") || hit.collider.CompareTag ("Objeto")) {

					//Verificamos si el usuario clickea sobre el objeto, esto nos dira que es inaccesible
					Collider2D checkHitPosition = Physics2D.OverlapPoint (mousePosition);

					if (checkHitPosition!=null && hit.collider.CompareTag ("Obstaculo")) {

						moving = true;
						target = new Vector2 (Acercarse(originPosition,hit.point).x,Acercarse(originPosition,hit.point).y);

						Debug.Log ("Hey!, no llego a ese lugar");
						textoRaton.text = "" + "Hey!, no llego a ese lugar";

						//Aca decimos que comienza a contar el tiempo;
						hablaRaton = true;
					} else if(checkHitPosition!=null && hit.collider.CompareTag ("Objeto")){

						//Aca interactuamos con el objeto al clickearlo
						moving = true;
						target = new Vector2 (Acercarse(originPosition,hit.point).x,Acercarse(originPosition,hit.point).y);

					}else{
						//Debug.Log ("Hay un obstaculo");
						moving = true;
						target = new Vector2 (Acercarse(originPosition,hit.point).x,Acercarse(originPosition,hit.point).y);
					}

				} else {
					moving = true;
				}
			} else {
				moving = true;
			}
		}

		if(hablaRaton==true){
			secondsCounter += Time.deltaTime;
			if(secondsCounter>=tiempo_texto){
				textoRaton.text = "";
				secondsCounter = 0;
				hablaRaton = false;
			}
		}

		if(moving==true){
			player.transform.position = Vector2.MoveTowards (originPosition,target,speed*Time.deltaTime);
			if(Vector2.Distance(player.transform.position,target)<0.1f){
				moving = false;
			}
		}
	}

	Vector2 Acercarse(Vector2 origin, Vector2 hitPoint){
		Vector2 destiny;
		float x, y;
		if (origin.x > hitPoint.x) {
			x = hitPoint.x + 1;
		} else {
			x = hitPoint.x - 1;
		}

		if (origin.y > hitPoint.y) {
			y = hitPoint.y + 1;
		} else {
			y = hitPoint.y - 1;
		}

		destiny = new Vector2 (x,y);
		return destiny;
	}

	void rotarObjeto(GameObject objeto, float originPositionX, float targetPositionX){
		if( targetPositionX < originPositionX){
			objeto.transform.rotation = Quaternion.Euler(player.transform.rotation.x,180f,player.transform.rotation.z);
		}else if(targetPositionX > originPositionX){
			objeto.transform.rotation = Quaternion.Euler(player.transform.rotation.x,0f,player.transform.rotation.z);
		}
	}

	//Pegado a la pared?
	void NavMesh2D(Vector2 originPosition,Vector2 mousePosition, Vector2 hitPoint){
		//RaycastHit2D hit = Physics2D.Raycast (originPosition, mousePosition - originPosition,Vector2.Distance(originPosition,mousePosition));
//		float angulo=Mathf.Atan2 (mousePosition.y - originPosition.y,mousePosition.x - originPosition.x)*(180/Mathf.PI);
//		for(int i=1;i<5;i++){
//			if (((angulo <= 45 && angulo >= 0) || (angulo >= -45 && angulo <= 0)) || ((angulo >= 135 && angulo <= 180) || (angulo <= -135 && angulo >= -180))) {
//				//Derecha e izquierda
//				Debug.DrawLine (originPosition,new Vector2( hit.point.x,hit.point.y-i), Color.magenta);
//				Debug.DrawLine (originPosition,new Vector2( hit.point.x,hit.point.y+i), Color.cyan);
//			} else {
//				if ((angulo > 45 && angulo < 135) || (angulo < -45 && angulo > -135)) {
//					//Arriba y abajo
//					Debug.DrawLine (originPosition,new Vector2( hit.point.x-i,hit.point.y), Color.magenta);
//					Debug.DrawLine (originPosition,new Vector2( hit.point.x+i,hit.point.y), Color.cyan);
//				}
//			}
//
//		}
	}
}
