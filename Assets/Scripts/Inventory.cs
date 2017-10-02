using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mono.Data.Sqlite; 
using System.Data; 
using System;


public class Inventory : MonoBehaviour {

	private SqliteConnection _conexion;
	private SqliteCommand _command;
	private SqliteDataReader _reader;

	private string _query;

	// Use this for initialization
	void Start () {
		ObjetosPersonaje ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.I) || Input.GetKey (KeyCode.Escape)) {
			SceneManager.LoadScene ("GameScene");

		}
	}

	void ObjetosPersonaje(){
		_conexion = new SqliteConnection ("URI=file:Assets\\DB\\database.db");
		_conexion.Open ();

		_query="SELECT * FROM Objeto";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_reader = _command.ExecuteReader ();

		if(_reader != null){
			while(_reader.Read()){

				/*GameObject go= Instantiate (oneDataPref) as GameObject;
				go.transform.SetParent (GameObject.Find ("PnlDataDB").transform,false);

				Text _id=go.transform.GetChild(0).gameObject.GetComponent<Text>();
				Text _name=go.transform.GetChild(1).gameObject.GetComponent<Text>();
				Image _type=go.transform.GetChild(2).gameObject.GetComponent<Image>();

				_id.text = _reader.GetValue (0).ToString ();
				_name.text = _reader.GetValue (1).ToString ();

				if (_reader.GetValue (2).ToString ().Equals ("Raton")) {
					_type.sprite = ratonFace;
				} else if(_reader.GetValue (2).ToString ().Equals ("Ratona")) {
					_type.sprite = ratonaFace;
				}
				*/
				if (_reader.GetInt32 (4)==GlobalController.ID) {
					Debug.Log (_reader.GetValue(0).ToString()+", "+_reader.GetValue(1).ToString()+", "+_reader.GetValue(2).ToString()+", "+_reader.GetValue(3).ToString()+", "+_reader.GetValue(4).ToString()+", ");	
				} 
			}
		}

		_reader.Close ();
		_reader = null;
		_command = null;
		_conexion.Close ();
		_conexion = null;
	}
}
