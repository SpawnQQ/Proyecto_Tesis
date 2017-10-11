using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public float speed;
	public GameObject indicador;
	public GameObject player;

	bool moving;
	Vector2 target;
	Vector2 final;
	public Text textoRaton;

	public Animator animacion;

//	public AnimationClip agacharse_raton;
//	public AnimationClip pararse_raton;
//	public AnimationClip caminar_raton;
//	public AnimationClip rascandose_raton;
//
//	private Animation animacion;
//
	float secondsCounter;
	float tiempo_texto=3;
	bool hablaRaton;

	bool estaParado=true;
	bool navMesh=false;
	 
	private DBConnector _connector;

	MenuController menuController;

	void Start () {
		moving = false;
		hablaRaton = false;
		secondsCounter=0;

		Debug.Log("Datos usuario: "+GlobalController.ID+", "+GlobalController.NAME+", "+GlobalController.TYPE);

//		animacion = this.gameObject.AddComponent<Animation> ();
//		animacion.AddClip (agacharse_raton,"agacharse");
//		animacion.AddClip (pararse_raton,"pararse");
//		animacion.AddClip (caminar_raton,"caminar");
//		animacion.AddClip (rascandose_raton,"rascandose");

	}
		
	void Update () {

		if (Input.GetKey (KeyCode.Escape)) {
			SceneManager.LoadScene ("Menu");
		} else if (Input.GetKey (KeyCode.I)) {
			SceneManager.LoadScene ("Inventario");

		}else {
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

			animacion.SetBool ("caminar",true);

		//	Debug.Log(animacion.GetClip("agacharse-raton"));

			//Al mover, se modifica la posicion z del objeto raton e indicador
			target = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

			rotarObjeto (player,originPosition.x,target.x);

			if (hit.collider != null) {

				if (hit.collider.CompareTag ("Obstaculo") || hit.collider.CompareTag ("Objeto")) {

					//Verificamos si el usuario clickea sobre el objeto, esto nos dira que es inaccesible
					Collider2D checkHitPosition = Physics2D.OverlapPoint (mousePosition);

					if (checkHitPosition!=null && hit.collider.CompareTag ("Obstaculo")) {
						//Aca se esta clickeando encima de un obstaculo

						//moving = true;
						if (estaParado == true) {
							StartCoroutine (comenzarCaminar ());
							estaParado = false;
						} else {
							moving = true;
						}

						target = new Vector2 (Acercarse(originPosition,hit.point).x,Acercarse(originPosition,hit.point).y);

						Debug.Log ("Hey!, no llego a ese lugar");
						textoRaton.text = "" + "Hey!, no llego a ese lugar";

						//Aca decimos que comienza a contar el tiempo;
						hablaRaton = true;
					} else if(checkHitPosition!=null && hit.collider.CompareTag ("Objeto")){
						//Aca se esta clickeando encima de un objeto

						//Aca interactuamos con el objeto al clickearlo
						//moving = true;

						//moving = true;
						if (estaParado == true) {
							StartCoroutine (comenzarCaminar ());
							estaParado = false;
						} else {
							moving = true;
						}

						target = new Vector2 (Acercarse(originPosition,hit.point).x,Acercarse(originPosition,hit.point).y);

					}else{
						Debug.Log ("Hay un obstaculo");
						//moving = true;
						//moving = true;
						if (estaParado == true) {
							StartCoroutine (comenzarCaminar ());
							estaParado = false;
						} else {
							moving = true;
						}

						//Aca decimos que hay un obstaculo, por lo que se generara un camino a reccorrer.

						final = new Vector2 (target.x,target.y);
						navMesh = true;

						target = new Vector2 (Acercarse(originPosition,hit.point).x,Acercarse(originPosition,hit.point).y);
					}

				} else {
					//Colisiona con algo, pero no es ni obstaculo ni objeto

					//moving = true;
					if (estaParado == true) {
						StartCoroutine (comenzarCaminar ());
						estaParado = false;
					} else {
						moving = true;
					}

				}
			} else {
				//No esta colisionando con nada.

				//moving = true;
				if (estaParado == true) {
					StartCoroutine (comenzarCaminar ());
					estaParado = false;
				} else {
					moving = true;
				}
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

		if (moving == true) {
			player.transform.position = Vector2.MoveTowards (originPosition, target, speed * Time.deltaTime);
			if (Vector2.Distance (player.transform.position, target) < 0.1f) {
				animacion.SetBool ("caminar", false);
				//StartCoroutine (terminarCaminar ());
				moving = false;
				estaParado = true;

				if (navMesh == true) {
					//NavMesh (originPosition, final, target);
				}
				navMesh = false;
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

	string direccionX( Vector2 origin, Vector2 final){
		if (origin.x <= final.x) {
			return "Derecha";
		} else {
			return "Izquierda";
		}
	}

	string direccionY( Vector2 origin, Vector2 final){
		if (origin.y <= final.y) {
			return "Arriba";
		} else {
			return "Abajo";
		}
	}


	Vector2 NavMesh(Vector2 origin, Vector2 final, Vector2 hit){
		string dirX=direccionX (origin,final) ,dirY=direccionY (origin,final);
		float horizontal=hit.x, vertical=hit.y;

		if (dirX.Equals ("Derecha")) {
			if (dirY.Equals ("Arriba")) {
				
				//Derecha Arriba-------------------------------------------------------------------
				if (Physics2D.OverlapPoint (new Vector2(horizontal+2,vertical))!=null) {
					do{
						vertical++;

					//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal+2,vertical))!=null);
					horizontal=horizontal+2;
				} else{

					//Colisionamos hacia arriba, por lo que avanzamos a la derecha
					do{
						horizontal++;

						//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal,vertical+2))!=null);
					vertical=vertical+2;
				}

			} else {
				//Derecha Abajo---------------------------------------------------------------------
				//Preguntamos si a la derecha colisionamos
				if (Physics2D.OverlapPoint (new Vector2(horizontal+2,vertical))!=null) {
					do{
						vertical--;

						//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal+2,vertical))!=null);
					horizontal=horizontal+2;
				} else{

					//Colisionamos hacia arriba, por lo que avanzamos a la derecha
					do{
						horizontal++;

						//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal,vertical-2))!=null);
					vertical=vertical-2;
				}
			}
		} else {
			//Izquierda
			if (dirY.Equals ("Arriba")) {

				//Izquierda Arriba----------------------------------------------------------------
				if (Physics2D.OverlapPoint (new Vector2(horizontal-2,vertical))!=null) {
					do{
						vertical++;

						//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal-2,vertical))!=null);
					horizontal=horizontal-2;
				} else{

					//Colisionamos hacia arriba, por lo que avanzamos a la derecha
					do{
						horizontal--;

						//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal,vertical+2))!=null);
					vertical=vertical+2;
				}

			} else {
				//Izquierda Abajo-----------------------------------------------------------------
				if (Physics2D.OverlapPoint (new Vector2(horizontal-2,vertical))!=null) {
					do{
						vertical--;

						//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal-2,vertical))!=null);
					horizontal=horizontal-2;
				} else{

					//Colisionamos hacia arriba, por lo que avanzamos a la derecha
					do{
						horizontal--;

						//Mientras colisionemos a la derecha, seguimos avanzando hacia arriba
					}while(Physics2D.OverlapPoint (new Vector2(horizontal,vertical-2))!=null);
					vertical=vertical-2;
				}

			}
		}

		Vector2 vector=new Vector2 (horizontal,vertical);
		return vector;
	}

	void rotarObjeto(GameObject objeto, float originPositionX, float targetPositionX){
		if( targetPositionX < originPositionX){
			objeto.transform.rotation = Quaternion.Euler(player.transform.rotation.x,180f,player.transform.rotation.z);
		}else if(targetPositionX > originPositionX){
			objeto.transform.rotation = Quaternion.Euler(player.transform.rotation.x,0f,player.transform.rotation.z);
		}
	}

	IEnumerator comenzarCaminar (){
		moving = false;
		yield return new WaitForSeconds(0.5f);
		moving = true;
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
