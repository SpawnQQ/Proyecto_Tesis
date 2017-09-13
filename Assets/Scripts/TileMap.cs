using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

	public TileType[] tileTypes;

	int[,] tiles;

	int mapSizeX = 10;
	int mapSizeY = 10;

	void Start(){
	
		tiles = new int[mapSizeX, mapSizeY];

		//Creacon del suelo por todo el mapeado
		for(int x=0; x<mapSizeX;x++){
			for(int y=0; y<mapSizeY;y++){
				tiles [x, y] = 0;
			}
		}

		//Creacion de obstaculos
		tiles [4, 4] = 1;
		tiles [5, 4] = 1;
		tiles [6, 4] = 1;
		tiles [7, 4] = 1;
		tiles [8, 4] = 1;

		tiles [4, 5] = 1;
		tiles [4, 6] = 1;
		tiles [8, 5] = 1;
		tiles [8, 6] = 1;

		GenerateMapVisual ();
	}

	//Dibujamos el terreno
	void GenerateMapVisual(){
		for(int x=0; x<mapSizeX;x++){
			for(int y=0; y<mapSizeY;y++){
				TileType tt=tileTypes[tiles[x,y]];

				Instantiate (tt.titleVisualPrefab, new Vector3(x,y,0),Quaternion.identity);
			}
		}

	}
}
