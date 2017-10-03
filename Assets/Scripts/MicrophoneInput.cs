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
		keywords.Add ("go", () => {
			GoCalled();		
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

	void GoCalled(){
		print ("Holi");
	}
	void Update(){    
	
	}    
}