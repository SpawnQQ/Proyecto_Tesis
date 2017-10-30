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

	public static float pObjetoX;
	public static float pObjetoY;

	public static bool onTriggerObjeto=false;

	public static bool salirE1_E2=false;
	public static bool salirE2_E1=false;

	public static GameObject objetoClickeado=null;

	public static GameObject escenario;

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
