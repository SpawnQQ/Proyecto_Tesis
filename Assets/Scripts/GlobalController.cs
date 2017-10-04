using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalController : MonoBehaviour {

	public static GlobalController globalController;

	public static int ID;
	public static string NAME;
	public static string TYPE;

	public void BtnLoadGame(){
		//Debug.Log ("Datos: "+EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text+", "+EventSystem.current.currentSelectedGameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text+", "+EventSystem.current.currentSelectedGameObject.transform.GetChild(2).gameObject.GetComponent<Image>().sprite.name);

		ID =int.Parse (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).gameObject.GetComponent<Text> ().text);
		NAME = EventSystem.current.currentSelectedGameObject.transform.GetChild (1).gameObject.GetComponent<Text> ().text;
		TYPE = EventSystem.current.currentSelectedGameObject.transform.GetChild (2).gameObject.GetComponent<Image> ().sprite.name;

		SceneManager.LoadScene ("GameScene");
	}

	public static void NewGame(int _id,string _name, string _type){
		//Debug.Log ("Datos creados: "+_id+", "+_name+", "+_type);
		ID=_id;
		NAME = _name;
		TYPE = _type;
	}

	public static void NavMesh(GameObject raton, Vector2 origin, Vector2 final, Vector2 hit){

		string dirX,dirY;

		if (origin.x > final.x) {
			if(origin.y > final.y){
				//Izquierda - Abajo
				dirX="Izquierda";
				dirY="Abajo";
			}else if(origin.y < final.y){
				//Izquierda - Arriba
				dirX="Izquierda";
				dirY="Arriba";
			}else if(origin.y==final.y){
				//Izquierda - Random
				dirX="Izquierda";
				dirY="Arriba";
			}
		} else if(origin.x < final.x){
			if(origin.y > final.y){
				//Derecha - Abajo
				dirX="Derecha";
				dirY="Abajo";
			}else if(origin.y < final.y){
				//Derecha - Arriba
				dirX="Derecha";
				dirY="Arriba";
			}else if(origin.y==final.y){
				//Derecha - Random
				dirX="Derecha";
				dirY="Arriba";
			}
		}else if(origin.x == final.x){
			if(origin.y > final.y){
				//Random - Abajo
				dirX="Derecha";
				dirY="Abajo";
			}else if(origin.y < final.y){
				//Random - Arriba
				dirX="Derecha";
				dirY="Arriba";
			}else if(origin.y==final.y){
				//Random - Random
				//Nunca sucedera!
			}
		}	
	}

	void Awake(){
		if(globalController==null){
			globalController = this;
			DontDestroyOnLoad (gameObject);
		}else if(globalController!=this){
			Destroy (gameObject);
		}

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
