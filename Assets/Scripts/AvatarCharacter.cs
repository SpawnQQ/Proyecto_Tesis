using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite; 
using System.Data; 
using System;

public class AvatarCharacter : MonoBehaviour {

	public Sprite raton;
	public Sprite ratona;
	public Image panel;
	public Dropdown inputGenero;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (inputGenero.captionText.text.Equals ("Raton")) {
			panel.sprite = raton;
		} else {
			panel.sprite = ratona;
		}
	}
}
