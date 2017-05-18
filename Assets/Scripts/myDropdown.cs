using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class myDropdown : MonoBehaviour {

	public Dropdown myDrop;

	// Use this for initialization
	void Start () {

		myDrop = this.GetComponent<UnityEngine.UI.Dropdown>();

	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			hideDropdown();
		}

	}

	public void mostrar()
	{
		if(myDrop.value == 1)
		{
			if(Application.loadedLevelName == "NovaPonte")
				Application.LoadLevel("NovaPonte_GaleriaEletrica");	
			if(Application.loadedLevelName == "AmadorAguiar2")
				Application.LoadLevel ("AmadorAguiar2_GaleriaEletrica");
			if(Application.loadedLevelName == "AmadorAguiar1")
				Application.LoadLevel ("AmadorAguiar1_GaleriaEletrica");
		}
			
		if(myDrop.value == 2)
		{
			if(Application.loadedLevelName == "NovaPonte")
				Application.LoadLevel("NovaPonte_GaleriaMecânica");	
			if(Application.loadedLevelName == "AmadorAguiar2")
				Application.LoadLevel ("AmadorAguiar2_GaleriaMecanica");
			if(Application.loadedLevelName == "AmadorAguiar1")
				Application.LoadLevel ("AmadorAguiar1_GaleriaMecanica");
		}
			
		if(myDrop.value == 3)
		{
			if(Application.loadedLevelName == "NovaPonte")
				Application.LoadLevel("NovaPonte_Patio");	
			if(Application.loadedLevelName == "AmadorAguiar2")
				Application.LoadLevel ("AmadorAguiar2_PatioTransformadores");
			if(Application.loadedLevelName == "AmadorAguiar1")
				Application.LoadLevel ("AmadorAguiar1_Patio");
		}
			
	}

	public void hideDropdown()
	{
		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
	}

	public void showDropdown()
	{
		print ("Mostrou!");
	}

}
