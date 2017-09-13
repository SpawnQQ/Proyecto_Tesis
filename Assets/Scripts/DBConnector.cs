using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite; 
using System.Data; 
using System;

public class DBConectors : MonoBehaviour {

	private SqliteConnection _conexion;
	private SqliteCommand _command;
	private SqliteDataReader _reader;

	private string _query;

	public void OpenDB(string _dbName){
		_conexion = new SqliteConnection (_dbName);
		_conexion.Open ();
	}


}
