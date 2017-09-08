using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	public float speed;
	public GameObject indicador;
	public GameObject player;
	public LayerMask notToHit;

	bool moving;
	Vector2 target;

	void Start () {
		moving = false;
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
		}

		if (Input.GetMouseButtonDown (0)) {
			target = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			if (hit.collider != null) {
				if (hit.collider.CompareTag ("Obstaculo")) {
					moving = false;
					Debug.Log ("Hay un obstaculo");
				} else {
					moving = true;
				}
			} else {
				moving = true;
			}
		}

		if(moving==true){
			player.transform.position = Vector2.MoveTowards (originPosition,target,speed*Time.deltaTime);
			if(Vector2.Distance(player.transform.position,target)<0.1f){
				moving = false;
			}
		}
	}
}
