using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour {

	public InputField inputNombre;
	public Dropdown inputGenero;
	private DBConnector _connector;


	public void MenuManager(string pNameScene){
		SceneManager.LoadScene (pNameScene);
	}

	public void CrearPersonaje(){

		int _id;

		_connector=gameObject.AddComponent<DBConnector> ();
		_connector.OpenDB ("URI=file:Assets\\DB\\database.db");

		//Validar los datos del input
		if(inputNombre.text==""){
			Debug.Log ("Error!, no ha ingresado ningun nombre para su personaje");


		}else{
			//Verificamos que no exista otro nombre en la base de datos
			if(_connector.ValidateNamePersonaje(inputNombre.text)){
				_connector.CloseDB ();
				Debug.Log ("Error!, ha ingreado un nombre ya existente");
			}else{
				_connector.InsertDataPersonaje (inputNombre.text,inputGenero.captionText.text);
				_connector.CloseDB ();

				//Aca accedemos a la DB nuevamente, paa extraer ID de un personaje
				_connector.OpenDB ("URI=file:Assets\\DB\\database.db");
				_id=_connector.GetIdPersonaje (inputNombre.text);
				_connector.CloseDB ();

				//Asignamos el objeto queso al crear un personaje
				CrearObjeto("Queso","Queso apestoso",1,_id);

				GlobalController.NewGame (_id,inputNombre.text,inputGenero.captionText.text);

				//Guardamos los datos en la base de datos y entramos al juego
				SceneManager.LoadScene ("GameScene");	
			}
		}

	}

	public void CrearObjeto(string _name, string _description, int _lot,int _id_personaje){
		_connector=gameObject.AddComponent<DBConnector> ();
		_connector.OpenDB ("URI=file:Assets\\DB\\database.db");
		_connector.InsertDataObjeto (_name,_description,_lot,_id_personaje);
		_connector.CloseWriteDB ();
	}

	public void BorrarPersonajes(){
		_connector=gameObject.AddComponent<DBConnector> ();
		_connector.OpenDB ("URI=file:Assets\\DB\\database.db");
		_connector.ResetDataPersonaje ();
		_connector.CloseWriteDB ();

	}

	void Start () {

	}

	public void Exit(){

	}
}
