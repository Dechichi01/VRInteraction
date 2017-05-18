using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Selecao : MonoBehaviour {
	Vector3 offset;
	Vector3 textPosition;
	Text texto;
	Vector3 camPos2MousePos;
	Camera cameraMapa;
	// Use this for initialization
	void Start () {
		offset = new Vector3(0f,-40f,0f);
		textPosition = new Vector3 (0, 0, 0);
		if(GameObject.Find ("Text") != null)
		{
			texto = GameObject.Find ("Text").GetComponent<Text> ();
			cameraMapa = GameObject.Find ("CameraMapa").GetComponent<Camera> ();
		}

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(GameObject.Find("Usina") != null)
			{
				Application.LoadLevel("cenaMinas");
			}
		}
		if(this.name == "Subestacao")
		{
			this.transform.position = GameObject.Find("targetSubestacao").transform.position;
		}
		if(this.name == "Usina")
		{

		}
		if(texto != null)
			texto.rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x + 90, Input.mousePosition.y);
		
	}
	
	void OnMouseEnter(){

		this.gameObject.transform.localScale += new Vector3(0.2F, 0.2F, 0.2F);
		if(texto != null)
		{
			texto.rectTransform.transform.position = new Vector3 (0,0,0);
			GameObject.Find("CanvasTexto").GetComponent<Canvas>().enabled = true;
			texto.text = this.gameObject.name;
		}
	}
	
	
	void OnMouseExit(){
		if(texto != null)
			GameObject.Find("CanvasTexto").GetComponent<Canvas>().enabled = false;
		this.gameObject.transform.localScale -= new Vector3(0.2F, 0.2F, 0.2F);
	}
	void OnMouseDown(){
		
		
		if (this.gameObject.name == "Usina") {
			
			print(GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true);
		}
		
		else
		{
			string nomeCena = this.gameObject.name;
			nomeCena = nomeCena.Replace(" ", "");
			print (nomeCena);
			Application.LoadLevel(nomeCena);
		}
		
		if (this.gameObject.name == "Subestacao") {
			
			
		}
		
	}
}
