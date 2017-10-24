using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public float speed;
	public GameObject indicador;
	public GameObject indicadorClickDerecho;
	public GameObject indicadorClickIzquierdo;
	public GameObject player;

	string accion;
	string objeto;

	bool moving;
	Vector2 target;
	Vector2 final;
	public Text textoRaton;

	public Animator animacion;
	float secondsCounter;
	float tiempo_texto=3;
	bool hablaRaton;

	bool estaParado=true;
	bool navMesh=false;
	bool movimientoPorVoz = false;
	bool mirar = false;
	bool caminar = false;

	private DBConnector _connector;

	MenuController menuController;

	public Sprite basureroSinTapa;

	void Start () {
		moving = false;
		hablaRaton = false;
		secondsCounter=0;

		Debug.Log("Datos usuario: "+GlobalController.ID+", "+GlobalController.NAME+", "+GlobalController.TYPE);

		if(ExisteObjeto (GlobalController.ID,"Platano")){
			Destroy (GameObject.Find("Platano"),0f);
		}

	}
		
	void Update () {

		if (Input.GetKey (KeyCode.Escape)) {
			SceneManager.LoadScene ("Menu");
		} else if (Input.GetKey (KeyCode.I)) {
			SceneManager.LoadScene ("Inventario");

		} else if (Input.GetKey (KeyCode.F1)) {
			SceneManager.LoadScene ("ShowCharacters");
		} else {
			movimientoRaton ();
			movimientoAccion ();
			movimiento ();
		}

		if (MicrophoneInput.accion != null) {
			Debug.Log (MicrophoneInput.accion);
			if (MicrophoneInput.accion.Equals ("Usar")) {

				if (MicrophoneInput.objetoInventario != null ) {
					
					if(ExisteObjeto (GlobalController.ID,MicrophoneInput.objetoInventario)){
						if (MicrophoneInput.objeto != null) {
							Debug.Log (MicrophoneInput.objeto);
							microfonoAccion ();
						}
					}

				} else {
					MicrophoneInput.objeto = null;
				}
			} else {
				MicrophoneInput.objetoInventario = null;

				if (MicrophoneInput.objeto != null) {
					Debug.Log (MicrophoneInput.objeto);
					microfonoAccion ();
				}
			}
		} else {
			MicrophoneInput.objeto = null;
			MicrophoneInput.objetoInventario = null;
		}
	}

	void accion_objeto(){
		if (accion.Equals ("Abrir")) {
			if (objeto.Equals ("Basurero")) {
				GameObject.Find (objeto).GetComponent<SpriteRenderer> ().sprite = basureroSinTapa;
			} else {
				hablar ("No puedo abrir ese objeto");
			}
		} else if (accion.Equals ("Ir")) {
			if (objeto.Equals ("Basurero")) {

			} else if(objeto.Equals ("Platano")){

			}
		} else if (accion.Equals ("Mirar")) {
			if (objeto.Equals ("Basurero")) {
				hablar ("Es un basurero!");
			} else if(objeto.Equals ("Platano")){
				hablar ("Es un platano!");
			}

		} else if(accion.Equals ("Coger") ){
			if (objeto.Equals ("Platano")) {
				Destroy (GameObject.Find("Platano"),.5f);
				CrearObjeto ("Platano", "Es un platano!", 1, GlobalController.ID);

			} else {
				hablar ("No puedo coger eso");
			} 

		}
	}


	void movimiento(){
		Vector2 originPosition = new Vector2 (player.transform.position.x, player.transform.position.y);

		if (moving == true) {
			player.transform.position = Vector2.MoveTowards (originPosition, target, speed* Time.deltaTime);
			if (Vector2.Distance (player.transform.position, target) < 0.1f) {
				moving = false;
				GlobalController.onTriggerObjeto = false;

				if (navMesh == true && (player.transform.position.x!=GlobalController.pObjetoX && player.transform.position.y!=GlobalController.pObjetoY)) {
					RaycastHit2D hitVerificador = Physics2D.Raycast (new Vector2 (player.transform.position.x, player.transform.position.y), final - new Vector2 (player.transform.position.x, player.transform.position.y), Vector2.Distance (new Vector2 (player.transform.position.x, player.transform.position.y), final));
					if (hitVerificador.collider != null) {
						if (Vector2.Distance (player.transform.position, hitVerificador.point) > Mathf.Sqrt (2)) {
							target = new Vector2 (Acercarse (new Vector2 (player.transform.position.x, player.transform.position.y), hitVerificador.point).x, Acercarse (new Vector2 (player.transform.position.x, player.transform.position.y), hitVerificador.point).y);
							moving = true;
						} else {
							target = new Vector2 (NavMesh (originPosition, final, target).x, NavMesh (originPosition, final, target).y);
							moving = true;
						}
					} else {
						target = new Vector2 (final.x, final.y);
						moving = true;
						navMesh = false;
					}
				} else {
					animacion.SetBool ("caminar", false);
					moving = false;
					estaParado = true;

					if (mirar == true) {
						if (GlobalController.objetoClickeado.name.Equals ("Basurero")) {
							hablar ("Es un basurero!");
						} else if(GlobalController.objetoClickeado.name.Equals ("Platano")){
							hablar ("Una cascara de platano");
						}
						mirar = false;
					} else if (movimientoPorVoz == true) {
						//Aca realizamos la accion sobre un objeto
						accion_objeto ();
					} else if (caminar == true) {
						if (GlobalController.salirE1_E2 == true) {
							Camera.main.transform.position = new Vector3 (1000f, 0f, -10f);
							player.transform.position = new Vector2 (600f, player.transform.position.y);
						} else if(GlobalController.salirE2_E1 == true){
							Camera.main.transform.position = new Vector3 (0f, 0f, -10f);
							player.transform.position = new Vector2 (400f, player.transform.position.y);
							player.transform.rotation = Quaternion.Euler(player.transform.rotation.x,180f,player.transform.rotation.z);
							player.transform.GetChild(0).gameObject.transform.rotation=Quaternion.Euler(0f,0f,0f);
						}
					}

					movimientoPorVoz = false;
					mirar = false;
					caminar = false;

					GlobalController.salirE1_E2=false;
					GlobalController.salirE2_E1=false;

					accion = null;
					objeto = null;
				}
			}
		}
	}
		
	void microfonoAccion(){
		accion = MicrophoneInput.accion;
		objeto = MicrophoneInput.objeto;

		GameObject objetoLLamado=GameObject.Find(MicrophoneInput.objeto);

		target = new Vector2 (objetoLLamado.transform.position.x, objetoLLamado.transform.position.y - (objetoLLamado.GetComponent<SpriteRenderer> ().sprite.pivot.y / 2));

		//this.gameObject.transform.position.y - (this.gameObject.GetComponent<SpriteRenderer> ().sprite.pivot.y / 2)
		MicrophoneInput.accion = null;
		MicrophoneInput.objeto = null;
		movimientoPorVoz = true;
		caminar = false;
		mirar = false;

		Vector2 originPosition = new Vector2 (player.transform.position.x, player.transform.position.y);
		//Vector2 originPosition = new Vector2 (player.transform.position.x, player.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast (originPosition, target - originPosition,Vector2.Distance(originPosition,target));

		animacion.SetBool ("caminar",true);

		rotarObjeto (player,originPosition.x,target.x);

		if (hit.collider != null) {
			if (estaParado == true) {
				StartCoroutine (comenzarCaminar ());
				estaParado = false;
			} else {
				moving = true;
			}

			final = new Vector2 (target.x, target.y);
			navMesh = true;

			target = new Vector2 (Acercarse (originPosition, hit.point).x, Acercarse (originPosition, hit.point).y);
		} else {
			//No esta colisionando con nada.

			if (estaParado == true) {
				StartCoroutine (comenzarCaminar ());
				estaParado = false;
			} else {
				moving = true;
			}
		}
	}

	void movimientoAccion(){
		//Vector2 respaldoPosicion = new Vector2 (indicadorClickDerecho.transform.position.x,indicadorClickDerecho.transform.position.y);
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 originPosition = new Vector2 (player.transform.position.x, player.transform.position.y);

		if (Input.GetMouseButtonDown (1)) {
			mirar = true;
			movimientoPorVoz = false;
			caminar = false;

			indicadorClickDerecho.transform.position = new Vector2 (mousePosition.x,mousePosition.y);
			StartCoroutine (IndicadorClickDerecho());

			if(GlobalController.onTriggerObjeto==true){
				target = new Vector2 (GlobalController.pObjetoX, GlobalController.pObjetoY);

				RaycastHit2D hit = Physics2D.Raycast (originPosition, target - originPosition,Vector2.Distance(originPosition,target));

				animacion.SetBool ("caminar",true);

				rotarObjeto (player,originPosition.x,target.x);

				if (hit.collider != null) {
					if (estaParado == true) {
						StartCoroutine (comenzarCaminar ());
						estaParado = false;
					} else {
						moving = true;
					}
						
					final = new Vector2 (target.x, target.y);
					navMesh = true;

					target = new Vector2 (Acercarse (originPosition, hit.point).x, Acercarse (originPosition, hit.point).y);
				} else {
					//No esta colisionando con nada.

					if (estaParado == true) {
						StartCoroutine (comenzarCaminar ());
						estaParado = false;
					} else {
						moving = true;
					}
				}
			}
		}
	}

	void movimientoRaton (){
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

		//Se puede asignar un objeto "Raton" y asignarlo en vez de asumir que raton es el propietario de este script
		Vector2 originPosition = new Vector2 (player.transform.position.x, player.transform.position.y);
		RaycastHit2D hit = Physics2D.Raycast (originPosition, mousePosition - originPosition,Vector2.Distance(originPosition,mousePosition));

		Debug.DrawLine (originPosition, mousePosition, Color.green);
		indicador.transform.position = new Vector2(mousePosition.x,mousePosition.y);

		if (hit.collider != null) {
			Debug.DrawLine (hit.point,mousePosition , Color.red);
			Debug.DrawLine (originPosition, hit.point, Color.green);
		}

		if (Input.GetMouseButtonDown (0)) {
			mirar = false;
			movimientoPorVoz = false;
			caminar = true;

			//Vector2 respaldoPosicion = new Vector2 (indicadorClickIzquierdo.transform.position.x,indicadorClickIzquierdo.transform.position.y);

			animacion.SetBool ("caminar",true);

			//Al mover, se modifica la posicion z del objeto raton e indicador
			target = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

			rotarObjeto (player,originPosition.x,target.x);

			if (hit.collider != null) {

				if (hit.collider.CompareTag ("Obstaculo") || hit.collider.CompareTag ("Objeto")) {

					//Verificamos si el usuario clickea sobre el objeto, esto nos dira que es inaccesible
					Collider2D checkHitPosition = Physics2D.OverlapPoint (mousePosition);

					if (checkHitPosition != null && hit.collider.CompareTag ("Obstaculo")) {
						//Aca se esta clickeando encima de un obstaculo

						//moving = true;
						if (estaParado == true) {
							StartCoroutine (comenzarCaminar ());
							estaParado = false;
						} else {
							moving = true;
						}

						target = new Vector2 (Acercarse (originPosition, hit.point).x, Acercarse (originPosition, hit.point).y);

						Debug.Log ("Hey!, no llego a ese lugar");
						textoRaton.text = "" + "Hey!, no llego a ese lugar";

						//Aca decimos que comienza a contar el tiempo;
						hablaRaton = true;
					} else if (checkHitPosition != null && checkHitPosition.CompareTag ("Objeto")) {
						//Aca se esta clickeando encima de un objeto

						if (estaParado == true) {
							StartCoroutine (comenzarCaminar ());
							estaParado = false;
						} else {
							moving = true;
						}

						target = new Vector2 (Acercarse (originPosition, hit.point).x, Acercarse (originPosition, hit.point).y);

					} else {
						//Hay un obstaculo

						if (estaParado == true) {
							StartCoroutine (comenzarCaminar ());
							estaParado = false;
						} else {
							moving = true;
						}

						//Aca decimos que hay un obstaculo, por lo que se generara un camino a reccorrer.

						final = new Vector2 (target.x, target.y);
						navMesh = true;

						target = new Vector2 (Acercarse (originPosition, hit.point).x, Acercarse (originPosition, hit.point).y);
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
			}else {
				//No esta colisionando con nada.

				if (estaParado == true) {
					StartCoroutine (comenzarCaminar ());
					estaParado = false;
				} else {
					moving = true;
				}
			}
			indicadorClickIzquierdo.transform.position = new Vector2 (mousePosition.x,mousePosition.y);
			StartCoroutine (IndicadorClickIzquierdo());
		}

		if(hablaRaton==true){
			secondsCounter += Time.deltaTime;
			if(secondsCounter>=tiempo_texto){
				textoRaton.text = "";
				secondsCounter = 0;
				hablaRaton = false;
			}
		}
	}

	void hablar(string texto){
		StartCoroutine (tiempoTexto(texto));
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
			objeto.transform.GetChild(0).gameObject.transform.rotation=Quaternion.Euler(0f,0f,0f);

		}else if(targetPositionX > originPositionX){
			objeto.transform.rotation = Quaternion.Euler(player.transform.rotation.x,0f,player.transform.rotation.z);
			objeto.transform.GetChild(0).gameObject.transform.rotation=Quaternion.Euler(0f,0f,0f);
		}
	}

	IEnumerator comenzarCaminar (){
		moving = false;
		yield return new WaitForSeconds(0.5f);
		moving = true;
	}

	IEnumerator IndicadorClickDerecho(){
		yield return new WaitForSeconds(0.00000000000000000000000001f);
		indicadorClickDerecho.transform.position = new Vector2 (-460f,215f);
	}

	IEnumerator IndicadorClickIzquierdo(){
		yield return new WaitForSeconds(0.00000000000000000000000001f);
		indicadorClickIzquierdo.transform.position = new Vector2 (-470f,215f);
	}

	IEnumerator tiempoTexto (string texto){
		player.transform.GetChild (0).gameObject.GetComponent<Text> ().text = texto;
		yield return new WaitForSeconds(3f);
		player.transform.GetChild (0).gameObject.GetComponent<Text> ().text=null;
	}

	public void CrearObjeto(string _name, string _description, int _lot,int _id_personaje){
		_connector=gameObject.AddComponent<DBConnector> ();
		_connector.OpenDB ("URI=file:Assets\\DB\\database.db");
		_connector.InsertDataObjeto (_name,_description,_lot,_id_personaje);
		_connector.CloseWriteDB ();
	}

	public bool ExisteObjeto(int _id_personaje, string _nombre_objeto){
		bool aux;
		_connector=gameObject.AddComponent<DBConnector> ();
		_connector.OpenDB ("URI=file:Assets\\DB\\database.db");
		aux = _connector.ExisteObjetoNombre (_id_personaje, _nombre_objeto);
		_connector.CloseDB ();
		return aux;

	}
}

//-----------------Esto iria dentro del movimientoRaton, es caminar hasta el objeto dentro del movimiento ya probado

/*
		if(GlobalController.onTriggerObjeto==true){

			animacion.SetBool ("caminar",true);
			target = new Vector2 (GlobalController.pObjetoX, GlobalController.pObjetoY);
			rotarObjeto (player,originPosition.x,target.x);

			RaycastHit2D hitSobreObjeto = Physics2D.Raycast (originPosition, target - originPosition,Vector2.Distance(originPosition,target));

			if (hitSobreObjeto.collider != null) {
				Debug.Log ("Hay un obstaculo");
				if (estaParado == true) {
					StartCoroutine (comenzarCaminar ());
					estaParado = false;
				} else {
					moving = true;
				}
				final = new Vector2 (target.x,target.y);
				navMesh = true;
				target = new Vector2 (Acercarse(originPosition,hit.point).x,Acercarse(originPosition,hit.point).y);
			} else {
				if (estaParado == true) {
					StartCoroutine (comenzarCaminar ());
					estaParado = false;
				} else {
					moving = true;
				}
			}
		}


		else if(GlobalController.onTriggerObjeto==true){
			//Aca clickeamos sobre el objeto en un trigger.

			if (estaParado == true) {
				StartCoroutine (comenzarCaminar ());
				estaParado = false;
			} else {
				moving = true;
			}
			target = new Vector2 (GlobalController.pObjetoX, GlobalController.pObjetoY);

		}
*/