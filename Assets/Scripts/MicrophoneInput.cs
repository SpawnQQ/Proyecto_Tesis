using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio; 
using UnityEngine.Windows.Speech;
using System.Linq;

public class MicrophoneInput : MonoBehaviour {

	public static string accion=null;
	public static string objeto=null;
	public static string objetoInventario = null;

	KeywordRecognizer keywordRecognizer;
	Dictionary<string,System.Action> keywords=new Dictionary<string,System.Action>();

	void Start() {        
		keywords.Add ("Abrir", () => {
			accion="Abrir";		
		});

		keywords.Add ("Coger", () => {
			accion="Coger";		
		});

		keywords.Add ("Mirar", () => {
			accion="Mirar";		
		});

		keywords.Add ("Ir", () => {
			accion="Ir";		
		});

		keywords.Add ("Usar", () => {
			accion="Usar";		
		});

		keywords.Add ("Basurero", () => {
			objeto="Basurero";		
		});

		keywords.Add ("Platano", () => {
			objeto="Platano";		
		});

		keywords.Add ("Queso", () => {
			objetoInventario="Queso";		
		});

		keywords.Add ("Zapato", () => {
			objetoInventario="Zapato";		
		});

		keywordRecognizer = new KeywordRecognizer (keywords.Keys.ToArray ());
		keywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
		keywordRecognizer.Start ();
	}
 	
	void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args){
		System.Action keywordAction;

		if(keywords.TryGetValue(args.text, out keywordAction)){
			keywordAction.Invoke ();
		}
	}

	void Update(){
		
	}    
}