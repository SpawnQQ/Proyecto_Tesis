using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio; 
using UnityEngine.Windows.Speech;
using System.Linq;

public class MicrophoneInput : MonoBehaviour {
	
	KeywordRecognizer keywordRecognizer;
	Dictionary<string,System.Action> keywords=new Dictionary<string,System.Action>();

	void Start() {        
		keywords.Add ("Abrir", () => {
			Accion("Abrir");		
		});

		keywords.Add ("Coger", () => {
			Accion("Coger");		
		});

		keywords.Add ("Mirar", () => {
			Accion("Mirar");		
		});

		keywords.Add ("Ir", () => {
			Accion("Ir");		
		});

		keywords.Add ("Ir", () => {
			Accion("Ir");		
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

	void Accion(string accion){
		if (accion.Equals ("Abrir")) {
				
		} else if (accion.Equals ("Coger")) {
		
		} else if (accion.Equals ("Mirar")) {
		
		} else if(accion.Equals("Ir")){
		
		}
	}

	void Objeto(string objeto){
		if(objeto.Equals("Basurero")){
			
		}
	}

	void Update(){    
	
	}    
}