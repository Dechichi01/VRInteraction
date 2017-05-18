using UnityEngine;
using System.Collections;

public class sceneManager : MonoBehaviour {

	public SceneFadeInOut scriptFade;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(GameObject.Find("First Person Controller") != null)
			{
				string cenaNome = Application.loadedLevelName;
				string[] newString = cenaNome.Split('_');
				Application.LoadLevel(newString[0]);
			}
		}

	}

	Vector3 parseVector3(string sourceString) {
		
		string outString;
		Vector3 outVector3;
		string[] splitString = new string[10];
		
		// Trim extranious parenthesis
		
		outString = sourceString.Substring(1, sourceString.Length - 2);
		
		// Split delimted values into an array
		
		splitString = outString.Split("," [0]);
		
		// Build new Vector3 from array elements
		
		outVector3.x = float.Parse(splitString[0]);
		outVector3.y = float.Parse(splitString[1]);
		outVector3.z = float.Parse(splitString[2]);
		
		return outVector3;
		
	}
	
	string parseScene(string sourceString) {
		
		string[] toSplit = new string[10];
		toSplit = sourceString.Split("+"[0]);
		return toSplit [0];
		
	}
	
	Vector3 parsePosition(string sourceString) {
		
		Vector3 position;
		string[] toSplit = new string[10];
		toSplit = sourceString.Split("+"[0]);
		
		position = parseVector3 (toSplit[1]);
		
		return position;
		
	}

	void OnTriggerEnter(Collider obj){ 

		if (obj.tag == "Player")
		{
			print ("Entrou!");
			
//			if(tag == "nextScene")
//			{
//				scriptFade.toScene = true;
////				print ("Proxima Cena");
//				scriptFade.nextScene = this.name;
////				scriptFade.lastTag = tag;
//			}
			if(tag == "nextPosition")
			{
				print ("Proxima Posicao");
				scriptFade.sceneEnding = true;
				scriptFade.toScene = false;
				scriptFade.nextPosition = parseVector3(this.name);
				//				scriptFade.sceneEnding = true;
				////				scriptFade.lastTag = tag;
			}
			else
			if(tag == "nextScene")
			{
				scriptFade.toScene = true;
				print ("Proxima Cena");
				scriptFade.nextScene = this.name;
				scriptFade.sceneEnding = true;
//				scriptFade.lastTag = tag;
			}
//			if(tag == "nextScenePos")
//			{
//				//				scriptFade.toScene = true;
//				//				scriptFade.nextScene = parseScene(this.name);
//				//				scriptFade.position = parsePosition(this.name);
//				//				scriptFade.lastTag = tag;
//			}
		}

	}
}
