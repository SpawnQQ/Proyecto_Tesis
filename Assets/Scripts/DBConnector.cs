using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite; 
using System.Data; 
using System;

public class DBConnector : MonoBehaviour {

	private SqliteConnection _conexion;
	private SqliteCommand _command;
	private SqliteDataReader _reader;

	private string _query;

	public void OpenDB(string _dbName){
		_conexion = new SqliteConnection (_dbName);
		_conexion.Open ();
	}

	public void SelectDataPersonaje(){
		_query="SELECT * FROM Personaje";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_reader = _command.ExecuteReader ();

		if(_reader != null){
			while(_reader.Read()){
				print (_reader.GetValue(0).ToString()+": "+_reader.GetValue(1).ToString()+", "+_reader.GetValue(2).ToString());
			}
		}
	}

	public int GetIdPersonaje(string _name){
		_query = "SELECT * FROM Personaje";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_reader = _command.ExecuteReader ();

		if (_reader != null) {
			while (_reader.Read ()) {
				if(_name.Equals(_reader.GetValue(1).ToString())){
					return _reader.GetInt32 (0);
				}
			}
		}

		//No encontro registro
		return 0;
	}

	public bool ValidateNamePersonaje(string _name){
		
		_query = "SELECT * FROM Personaje";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_reader = _command.ExecuteReader ();
		if (_reader != null) {
			while (_reader.Read ()) {
				if(_reader.GetValue(1).ToString().Equals(_name)){
					return true;
				}
			}
		}

		//No encontro registro con el nombre especifico
		return false;
	}
	public void InsertDataPersonaje(string _name,string _type){
		_query = "INSERT INTO Personaje (name,type) VALUES('"+_name+"', '"+_type+"')";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_command.ExecuteNonQuery ();
	}

	public void ResetDataPersonaje(){
		_query="DELETE FROM Personaje";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_command.ExecuteNonQuery ();
		_query="DELETE FROM sqlite_sequence WHERE name='Personaje'";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_command.ExecuteNonQuery ();
	}

	public void SelectDataObjeto(int _id_personaje){
		_query = "SELECT * FROM Objeto WHERE id_personaje=" + _id_personaje;
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_reader = _command.ExecuteReader ();

		if(_reader != null){
			while(_reader.Read()){
				print (_reader.GetValue(0).ToString()+": "+_reader.GetValue(1).ToString()+", "+_reader.GetValue(2).ToString()+", "+_reader.GetValue(3).ToString()+", "+_reader.GetValue(4).ToString());
			}
		}
	}

	public bool ExisteObjetoNombre(int _id_personaje, string _name){
		_query = "SELECT * FROM Objeto";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_reader = _command.ExecuteReader ();

		if(_reader != null){
			while(_reader.Read()){
				if(_reader.GetInt32 (4)==_id_personaje && _reader.GetValue(1).ToString().Equals(_name)){
					return true;
				}
			}
		}
		return false;
	}

	public void InsertDataObjeto(string _name, string _description,int lot,int _id_personaje){
		_query = "INSERT INTO Objeto (name,description,lot,id_personaje) VALUES('" + _name + "', '" + _description + "', '" + lot + "', '" + _id_personaje + "')";
		_command = _conexion.CreateCommand ();
		_command.CommandText = _query;
		_command.ExecuteNonQuery ();
	}

	public void CloseDB (){
		_reader.Close ();
		_reader = null;
		_command = null;
		_conexion.Close ();
		_conexion = null;
	}

	public void CloseWriteDB(){
		_command = null;
		_conexion.Close ();
		_conexion = null;
	}
}
