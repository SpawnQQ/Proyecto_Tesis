﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuPrincipal : MonoBehaviour {

	public InputField inputNombre;
	public Dropdown inputGenero;
	private DBConnector _connector;

	public void MenuManager(string pNameScene){
		SceneManager.LoadScene (pNameScene);
	}

	public void CrearPersonaje(){

		_connector=gameObject.AddComponent<DBConnector> ();
		_connector.OpenDB ("URI=file:Assets\\DB\\database.db");

		char genero;

		if (inputGenero.captionText.text == "Raton") {
			genero = 'M';
		} else {
			genero= 'F' ;
		}

		//Validar los datos del input
		if(inputNombre.text==""){
			Debug.Log ("Error!, no ha ingresado ningun nombre para su personaje");
		}else{
			//Verificamos que no exista otro nombre en la base de datos
			if(_connector.ValidateNamePersonaje(inputNombre.text)){
				_connector.CloseDB ();
				Debug.Log ("Error!, ha ingreado un nombre ya existente");
			}else{
				_connector.InsertDataPersonaje (inputNombre.text,genero);
				_connector.CloseDB ();

				//Guardamos los datos en la base de datos y entramos al juego
				SceneManager.LoadScene ("GameScene");	
			}
		}
			
	}

	public void Exit(){
		
	}
}