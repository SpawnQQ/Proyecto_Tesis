using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public float speed;
	public GameObject indicador;
	Vector2 target;
	// Use this for initialization
	void Start () {
		target = transform.position;
	}

	// Update is called once per frame
	void Update () {

		//Movimiento clickeando el raton
		if (Input.GetMouseButtonDown (0)) {
			//Buscamos el click respecto a la escena, target seria la posicion final
			target = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hitSuelo=Physics2D.Raycast(target,Vector2.zero);
			Debug.Log ("Posicion destino: "+target);
			Debug.Log ("Ray en 2D: "+hitSuelo.collider);

		}

		//Movemos el objeto hacia el punto de destino, cada Update se movera hasta llegar a destino
		transform.position = Vector2.MoveTowards (transform.position,target,speed*Time.deltaTime);
		indicador.transform.position = target;
		Debug.DrawLine (transform.position, target, Color.green);
	}
}
