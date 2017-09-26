using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GlobalController : MonoBehaviour {

	public static GlobalController globalController;

	public void BtnLoadGame(){
		Debug.Log (EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text);
		SceneManager.LoadScene ("GameScene");
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
