using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour {

	public float speed;
	public GameObject indicador;
	public GameObject player;

	bool moving;
	Vector2 target;
	public Text textoRaton;

	float secondsCounter;
	float tiempo_texto=3;
	bool hablaRaton;

	private DBConnector _connector;

	void Start () {
		moving = false;
		hablaRaton = false;
		secondsCounter=0;

		//Creamos personaje
		_connector=gameObject.AddComponent<DBConnector> ();
		_connector.OpenDB ("URI=file:Assets\\DB\\database.db");
		//_connector.InsertDataPersonaje ("nicolas",'M');
		_connector.SelectDataPersonaje ();
		_connector.CloseDB ();
	}
		
	void Update () {
		
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

		//Se puede asignar un objeto "Raton" y asignarlo en vez de asumir que raton es el propietario de este script
		Vector2 originPosition = new Vector2 (player.transform.position.x, player.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast (originPosition, mousePosition - originPosition,Vector2.Distance(originPosition,mousePosition));

		Debug.DrawLine (originPosition, mousePosition, Color.green);
		indicador.transform.position = mousePosition;
	
		//Debug.Log (hit.collider.tag);
		if (hit.collider != null) {
			Debug.DrawLine (hit.point,mousePosition , Color.red);
			Debug.DrawLine (originPosition, hit.point, Color.gray);

		}

		if (Input.GetMouseButtonDown (0)) {

			//Sacando un angulo
			//Debug.Log (Mathf.Atan2 (mousePosition.y - originPosition.y,mousePosition.x - originPosition.x)*(180/Mathf.PI));

			target = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			if (hit.collider != null) {
				if (hit.collider.CompareTag ("Obstaculo")) {
					
					//Verificamos si el usuario clickea sobre el objeto, esto nos dira que es inaccesible
					Collider2D checkHitPosition = Physics2D.OverlapPoint (mousePosition);

					if (checkHitPosition != null) {
						Debug.Log (checkHitPosition.tag);
						//NavMesh2D (originPosition,mousePosition,hit.point);
						moving = false;
						Debug.Log ("Hey!, no llego a ese lugar");
						textoRaton.text = "" + "Hey!, no llego a ese lugar";

						//Aca decimos que comienza a contar el tiempo;
						hablaRaton = true;
					} else {
						Debug.Log ("Hay un obstaculo");
						moving = false;
						//Reemplazar moving=false, por la funcion "NavMesh2D"
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
