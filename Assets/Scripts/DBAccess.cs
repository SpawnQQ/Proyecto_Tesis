using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite; 
using System.Data; 
using System;

public class DBAccess : MonoBehaviour {

	public GameObject oneDataPref; 

	private SqliteConnection _conexion;
	private SqliteCommand _command;
	private SqliteDataReader _reader;

	public Sprite ratonFace;
	public Sprite ratonaFace;


	private string _query;

	void Awake(){
		ratonFace = GetComponent<Sprite> ();
		ratonaFace = GetComponent<Sprite> ();
	}

	void Start () {
		ShowAllCharacters ();
	}

	void ShowAllCharacters (){
		_conexion = new SqliteConnection ("URI=file:Assets\\DB\\database.db");
		_conexion.Open ();

		_query="SELECT * FROM Personaje";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_reader = _command.ExecuteReader ();

		if(_reader != null){
			while(_reader.Read()){

				GameObject go= Instantiate (oneDataPref) as GameObject;
				go.transform.SetParent (GameObject.Find ("PnlDataDB").transform,false);

				Text _id=go.transform.GetChild(0).gameObject.GetComponent<Text>();
				Text _name=go.transform.GetChild(1).gameObject.GetComponent<Text>();
				//Image _type=go.transform.GetChild(1).gameObject.GetComponent<Image>();

				_id.text = _reader.GetValue (0).ToString ();
				_name.text = _reader.GetValue (1).ToString ();

				if (_reader.GetValue (2).ToString ().Equals ("Raton")) {
					//_type.sprite = ratonFace;
				} else if(_reader.GetValue (2).ToString ().Equals ("Ratona")) {
					//_type.sprite = ratonaFace;
				}
			}
		}

		CloseDB (_conexion,_command,_reader);
	}

	void CloseDB(SqliteConnection _conexion,SqliteCommand _command,SqliteDataReader _reader){
		_reader.Close ();
		_reader = null;
		_command = null;
		_conexion.Close ();
		_conexion = null;
	}
}
