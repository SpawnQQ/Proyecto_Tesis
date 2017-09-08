using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public float speed;
	public GameObject indicador;
	Vector2 target;
	Vector2 inicio;
	Vector2 newPosition;
	bool moving;


	// Use this for initialization
	void Start () {
		target=transform.position;
		moving = false;
	}

	// Update is called once per frame
	void Update () {

//		//Movimiento clickeando el raton
//		if (Input.GetMouseButtonDown (0)) {
//			inicio = target;
//			//Buscamos el click respecto a la escena, target seria la posicion final
//			target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//			RaycastHit2D hitSuelo=Physics2D.Raycast(target,inicio);
//			Debug.Log ("Posicion destino: "+target+" Inicio: "+inicio);
//			Debug.Log ("Ray en 2D: "+hitSuelo.collider.tag);
//
//		}
//
//		//Movemos el objeto hacia el punto de destino, cada Update se movera hasta llegar a destino
//		transform.position = Vector2.MoveTowards (transform.position,target,speed*Time.deltaTime);
//		indicador.transform.position = target;
//		Debug.DrawLine (transform.position, target, Color.green);

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit;

			inicio = target;
			target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			hit = Physics2D.Raycast(target,inicio-target,Vector2.Distance(transform.position,target));

			if (hit.collider!=null) {
				//Asignamos nueva posicion
				newPosition = hit.point;
				indicador.transform.position = hit.point;
				if (hit.collider.CompareTag ("Objeto")) {
					moving = false;
					indicador.transform.position = hit.point;
					Debug.Log ("Hay un obstaculo");
				} else {
					moving = true;
				}
			} else {
				moving = true;
			}
		}

		if(moving==true){
			transform.position = Vector2.MoveTowards (transform.position,target,speed*Time.deltaTime);
			Debug.DrawLine (transform.position, target, Color.green);
			if(Vector2.Distance(transform.position,newPosition)<0.1f){
				moving = false;
			}
		}

	}
}
