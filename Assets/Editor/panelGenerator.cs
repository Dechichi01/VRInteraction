using UnityEngine;
using UnityEditor;
using System.Collections;

public class panelGenerator : EditorWindow
{
	
	string myString = "Hello World";
	bool groupQtd1Lado;
	bool groupQtd2Lados;
	bool myBool = true;
	float myFloat = 1.23f;
	
	string txtUsina;
	
	string qtdPaineisR;
	string qtdPaineisL;
	
	string txtNewOffset;
	string txtPanelDistanceL;
	string txtPanelDistanceR;
	string txtPatioOffset;
	
	string txtQtdPanelGroupL;
	string[] txtGroupsL = new string[25];
	string txtQtdPanelGroupR;
	string[] txtGroupsR = new string[25];
	
	int qtdPaineisRInt;
	int qtdPaineisLInt;

	string txtConsole;
	
	
	public bool qtdLados1 = false;
	public bool qtdLados2 = true;
	
	Object prefab;
	
	public float offsetX = 2;
	public int offsetZ = 3;
	
	public float newOffsetX;
	
	[MenuItem("CEMIG/Gerador de Paineis")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(panelGenerator));
	}
	
	void Start()
	{
		
	}
	
	void OnGUI()
	{
		bool btnGerar;
		bool btnDestruir;
		bool btnDistribuirR;
		bool btnDistribuirL;
		bool btnTexturizarL;
		bool btnTexturizarR;
		bool btnAdjustDistance;
		bool btnCheck;
		
		GUILayout.Label ("Gerador de Paineis", EditorStyles.boldLabel);
		
		txtUsina = EditorGUILayout.TextField ("Usina: ", txtUsina);
		
		//		groupQtd1Lado = EditorGUILayout.BeginToggleGroup ("Um Lado", groupQtd1Lado);		
		//		
		//		EditorGUILayout.EndToggleGroup ();
		
		//		groupQtd2Lados = EditorGUILayout.BeginToggleGroup ("Dois Lados", groupQtd2Lados);
		//		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		//		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		
		
		//		GUILayout.Label ("Quantidade de paineis: ", EditorStyles.label);
		
		
		
		txtPatioOffset = EditorGUILayout.TextField ("Largura Corredor: ", txtPatioOffset);
		
		
		//		EditorGUILayout.EndToggleGroup ();
		
		GUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical ("Button");
		GUILayout.Label ("Paineis Esquerdos: ", EditorStyles.boldLabel);
		qtdPaineisL = EditorGUILayout.TextField ("Quantidade", qtdPaineisL);
		txtPanelDistanceL = EditorGUILayout.TextField ("Distancia entre paineis: ", txtPanelDistanceL);
		txtQtdPanelGroupL = EditorGUILayout.TextField ("Quantidade de Grupos: ", txtQtdPanelGroupL);
		int number;
		if(int.TryParse(txtQtdPanelGroupL, out number) == true)
		{
			if(int.Parse(txtQtdPanelGroupL) >=0 && int.Parse(txtQtdPanelGroupL) <= 23 )
				for(int i = 1; i <= int.Parse(txtQtdPanelGroupL); i++)
			{
				txtGroupsL[i] = EditorGUILayout.TextField ("Grupo "+i+": ", txtGroupsL[i]);
			}
		}
		
		btnDistribuirL = GUILayout.Button ("Redistribuir", GUILayout.Width(Screen.width/2-18));
		btnTexturizarL = GUILayout.Button ("Texturizar");
		
		EditorGUILayout.EndVertical ();
		
		EditorGUILayout.BeginVertical ("Button");
		GUILayout.Label ("Paineis Direitos: ", EditorStyles.boldLabel);
		qtdPaineisR = EditorGUILayout.TextField ("Quantidade", qtdPaineisR);
		txtPanelDistanceR = EditorGUILayout.TextField ("Distancia entre paineis: ", txtPanelDistanceR);
		txtQtdPanelGroupR = EditorGUILayout.TextField ("Quantidade de Grupos: ", txtQtdPanelGroupR);
		
		if(int.TryParse(txtQtdPanelGroupR, out number) == true)
		{
			if(int.Parse(txtQtdPanelGroupR) >=0 && int.Parse(txtQtdPanelGroupR) <= 23 )
				for(int i = 1; i <= int.Parse(txtQtdPanelGroupR); i++)
			{
				txtGroupsR[i] = EditorGUILayout.TextField ("Grupo "+i+": ", txtGroupsR[i]);
			}
		}
		
		btnDistribuirR = GUILayout.Button ("Redistribuir", GUILayout.Width(Screen.width/2-18));
		btnTexturizarR = GUILayout.Button ("Texturizar");
		EditorGUILayout.EndVertical ();
		
		GUILayout.EndHorizontal();



		btnCheck = GUILayout.Button ("Checar");
		btnGerar = GUILayout.Button ("Gerar");
		btnDestruir = GUILayout.Button ("Destruir");
		
		
		btnAdjustDistance = GUILayout.Button ("Ajustar Automaticamente");

		
		if(btnGerar)
		{
			prefab = AssetDatabase.LoadAssetAtPath("Assets/Usinas/Prefabs/panelPrefab.prefab", typeof(GameObject));
			gerarPaineis();
		}
		if(btnDestruir)
		{
			destruir();
		}
		if(btnDistribuirR)
		{
			setDistanceR();
		}
		if(btnDistribuirL)
		{
			setDistanceL();
		}
		if(btnAdjustDistance)
		{
			adjustDistance();
		}
		if(btnTexturizarL)
		{
			texturizarL(txtUsina.ToString());
		}
		if(btnTexturizarR)
		{
			texturizarR(txtUsina.ToString());
		}
		if(btnCheck)
		{
			int tempQtdL = 0;
			int tempQtdR = 0;

			for (int x = 1; x <= int.Parse(txtQtdPanelGroupL); x++) 
			{
				for(int y = 1; y <= int.Parse(txtGroupsL[x]); y++)
				{
					tempQtdL++;
				}
			
			}
			if(tempQtdL != int.Parse(qtdPaineisL))
			{
				txtConsole = "A quantidade de Paineis esquerdos esta incorreta";
			}
			else
			{
				txtConsole = "OK!";
			}

			for (int x = 1; x <= int.Parse(txtQtdPanelGroupR); x++) 
			{
				for(int y = 1; y <= int.Parse(txtGroupsR[x]); y++)
				{
					tempQtdR++;
				}
				
			}
			if(tempQtdR != int.Parse(qtdPaineisR))
			{
				txtConsole = "A quantidade de Paineis direitos esta incorreta";
			}
			else
			{
				txtConsole = "OK!";
			}
		}
		
		Rect r = EditorGUILayout.BeginHorizontal ("Box");
		EditorGUILayout.LabelField(txtConsole);
		EditorGUILayout.EndHorizontal ();


	}

	
	void gerarPaineis()
	{
		destruir ();
		//Define o ponto de origem dos paineis
		GameObject obj = GameObject.Find("paineis");
		var positionY = obj.transform.position.y;
		var positionX = obj.transform.position.x;
		var positionZ = obj.transform.position.z;
		
		int nameIndex = 1;
		
		for (int x = 1; x <= int.Parse(txtQtdPanelGroupR); x++) 
		{
			for(int y = 1; y <= int.Parse(txtGroupsR[x]); y++)
			{
				if(y == 1)
					offsetX = 0.9f;
				if(y == int.Parse(txtGroupsR[x]))
					offsetX = float.Parse(txtPanelDistanceR);
				
				GameObject painel = Instantiate(prefab, new Vector3 (positionX, 0, positionZ), Quaternion.identity) as GameObject;
				painel.transform.parent = obj.transform;
				painel.tag = "objPanel";
				//				painel.name = "painel_direito_"+nameIndex;
				painel.name = "painel_direito_grupo"+x+"_id"+y;
				painel.AddComponent<BoxCollider>();
				positionX = positionX + offsetX;
				nameIndex++;
			}
			
		}
		
		offsetZ = int.Parse(txtPatioOffset);
		positionY = obj.transform.position.y;
		positionX = obj.transform.position.x;
		positionZ = obj.transform.position.z + offsetZ;
		
		nameIndex = 1;
		for (int x = 1; x <= int.Parse(txtQtdPanelGroupL); x++) 
		{
			for(int y = 1; y <= int.Parse(txtGroupsL[x]); y++)
			{
				
				if(y == 1)
					offsetX = 0.9f;
				if(y == int.Parse(txtGroupsL[x]))
					offsetX = float.Parse(txtPanelDistanceL);
					
				GameObject painel = Instantiate(prefab, new Vector3 (positionX, 0, positionZ), Quaternion.Euler(0, 180, 0)) as GameObject;
				painel.transform.parent = obj.transform;
				painel.tag = "objPanel";
				//				painel.name = "painel_esquerdo_"+nameIndex;
				painel.name = "painel_esquerdo_grupo"+x+"_id"+y;
				painel.AddComponent<BoxCollider>();
				positionX = positionX + offsetX;
				nameIndex++;
			}
		}
		
	}
	
	void destruir()
	{
		GameObject[] objToDestroy = new GameObject[100];
		objToDestroy = GameObject.FindGameObjectsWithTag("objPanel");
		for(var i = 0 ; i < objToDestroy.Length ; i ++)
		{
			DestroyImmediate (objToDestroy[i]);
		}
	}
	
	void setDistanceR()
	{

		GameObject obj = GameObject.Find("paineis");
		var positionY = obj.transform.position.y;
		var positionX = obj.transform.position.x;
		var positionZ = obj.transform.position.z;
		
		for (int x = 1; x <= int.Parse(txtQtdPanelGroupR); x++) 
		{
			for(int y = 1; y <= int.Parse(txtGroupsR[x]); y++)
			{
				if(y == 1)
					offsetX = 0.9f;
				if(y == int.Parse(txtGroupsR[x]))
					offsetX = float.Parse(txtPanelDistanceR);
				GameObject painel = GameObject.Find("painel_direito_grupo"+x+"_id"+y);
				Vector3 tempPosition = new Vector3(positionX, painel.transform.position.y, positionZ);
				painel.transform.position = tempPosition;

				positionX = positionX + offsetX;
			}
		}


//		int nameIndex = 1;
//		
//		if(int.Parse(txtQtdPanelGroupR) > 0)
//		{
//			for(int x = 2; x <= int.Parse(txtQtdPanelGroupR); x++) 
//			{
//				for(int y = x; y <= int.Parse(txtQtdPanelGroupR); y++) 
//				{
//					for(int z = 1; z <= int.Parse(txtGroupsR[y]); z++)
//					{
//
//						GameObject currentPanel = GameObject.Find("painel_direito_grupo"+y+"_id"+z);
//						Vector3 bkpPosition = currentPanel.transform.position;
//						Vector3 currentPosition = new Vector3(currentPanel.transform.position.x, currentPanel.transform.position.y, currentPanel.transform.position.z);
//						Vector3 newPosition = new Vector3(currentPosition.x + float.Parse(txtPanelDistanceL), currentPosition.y, currentPosition.z);
//						currentPanel.transform.position = newPosition;
//					}
//				}
//			}
//		}
	}
	
	void setDistanceL()
	{

		GameObject obj = GameObject.Find("paineis");
		var positionY = obj.transform.position.y;
		var positionX = obj.transform.position.x;
		var positionZ = obj.transform.position.z;

		offsetZ = int.Parse(txtPatioOffset);
		positionY = obj.transform.position.y;
		positionX = obj.transform.position.x;
		positionZ = obj.transform.position.z + offsetZ;


		for (int x = 1; x <= int.Parse(txtQtdPanelGroupL); x++) 
		{
			for(int y = 1; y <= int.Parse(txtGroupsL[x]); y++)
			{
				if(y == 1)
					offsetX = 0.9f;
				if(y == int.Parse(txtGroupsL[x]))
					offsetX = float.Parse(txtPanelDistanceL);
				GameObject painel = GameObject.Find("painel_esquerdo_grupo"+x+"_id"+y);
				Vector3 tempPosition = new Vector3(positionX, painel.transform.position.y, painel.transform.position.z);
				painel.transform.position = tempPosition;
				
				positionX = positionX + offsetX;
			}
		}



//		int nameIndex = 1;
//
//		if(int.Parse(txtQtdPanelGroupL) > 0)
//		{
//			//			for(int x = int.Parse(txtGroupsL[1]) + 1; x <= int.Parse(qtdPaineisL); x++)
//			//			{
//			for(int x = 2; x <= int.Parse(txtQtdPanelGroupL); x++) 
//			{
//				for(int y = x; y <= int.Parse(txtQtdPanelGroupL); y++) 
//				{
//					for(int z = 1; z <= int.Parse(txtGroupsL[y]); z++)
//					{
//						
//	
//						GameObject currentPanel = GameObject.Find("painel_esquerdo_grupo"+y+"_id"+z);
//						Vector3 bkpPosition = currentPanel.transform.position;
//						Vector3 currentPosition = new Vector3(currentPanel.transform.position.x, currentPanel.transform.position.y, currentPanel.transform.position.z);
//						Vector3 newPosition = new Vector3(currentPosition.x + float.Parse(txtPanelDistanceL), currentPosition.y, currentPosition.z);
//						currentPanel.transform.position = newPosition;
//					}
//				}
//			}






			//				GameObject prevPanel = GameObject.Find("painel_esquerdo_" + (x-1));
			//				GameObject currentPanel = GameObject.Find("painel_esquerdo_" + x);
			//				Vector3 currentPosition = new Vector3(prevPanel.transform.position.x, prevPanel.transform.position.y, prevPanel.transform.position.z);
			//				Vector3 newPosition = new Vector3(currentPosition.x + float.Parse(txtPanelDistanceL), currentPosition.y, currentPosition.z);
			//				currentPanel.transform.position = newPosition;
			//			}
			
			//			for (int x = 2; x <= int.Parse(txtQtdPanelGroupL); x++) 
			//			{
			//				for(int y = 1; y <= int.Parse(txtGroupsL[x]); y++)
			//				{
			//					GameObject currentPanel = GameObject.Find("painel_esquerdo_grupo"+x+"_id"+y);
			//					Vector3 currentPosition = new Vector3(currentPanel.transform.position.x, currentPanel.transform.position.y, currentPanel.transform.position.z);
			//					Vector3 newPosition = new Vector3(currentPosition.x + float.Parse(txtPanelDistanceL), currentPosition.y, currentPosition.z);
			//					currentPanel.transform.position = newPosition;
			//				}
			//			}

		
		
		
		//			GameObject prevPanel = GameObject.Find("painel_esquerdo_" + (x-1));
		//			GameObject currentPanel = GameObject.Find("painel_esquerdo_" + x);
		//			Vector3 currentPosition = new Vector3(prevPanel.transform.position.x, prevPanel.transform.position.y, prevPanel.transform.position.z);
		//			Vector3 newPosition = new Vector3(currentPosition.x + float.Parse(txtPanelDistanceL), currentPosition.y, currentPosition.z);
		//			currentPanel.transform.position = newPosition;
		
	}
	
	
	void adjustDistance()
	{
		float currentDistance = 0;
		bool finalizou = false;

		if(getDistanceL() < getDistanceR())
		{
			float x = 0.1f;
			do
			{
				txtPanelDistanceL =x.ToString();
				setDistanceL();
				
				if(getDistanceL() >= getDistanceR())
				{
					finalizou = true;
				}
		
				x += 0.1f;
				x = Mathf.Round(x * 100f) / 100f; 
			}
			while(finalizou == false);
			txtPanelDistanceL = x.ToString();
		}
		else
		{			
			if(getDistanceR() < getDistanceL())
			{
				float x = 0.1f;
				do
				{
					txtPanelDistanceR =x.ToString();
					setDistanceR();
					
					if(getDistanceR() >= getDistanceL())
					{
						finalizou = true;
					}

					x += 0.1f;
					x = Mathf.Round(x * 100f) / 100f; 
				}
				while(finalizou == false);
				txtPanelDistanceR = x.ToString();

			}
		}
	}
	
	float getDistanceR()
	{
		float distanceR;
		GameObject firstR = GameObject.Find("painel_direito_grupo1_id1");
		GameObject lastL = GameObject.Find("painel_direito_grupo" + int.Parse(txtQtdPanelGroupR) + "_id" + int.Parse(txtGroupsR[int.Parse(txtQtdPanelGroupR)]));
		distanceR = lastL.transform.position.x - firstR.transform.position.x;
		return distanceR;
	}
	
	float getDistanceL()
	{
		float distanceL;
		GameObject firstL = GameObject.Find("painel_esquerdo_grupo1_id1");
		GameObject lastL = GameObject.Find("painel_esquerdo_grupo" + int.Parse(txtQtdPanelGroupL) + "_id" + int.Parse(txtGroupsL[int.Parse(txtQtdPanelGroupL)]));
		distanceL = lastL.transform.position.x - firstL.transform.position.x;
		return distanceL;
	}
	void texturizarR(string usina)
	{
		for (int x = 1; x <= int.Parse(txtQtdPanelGroupR); x++) 
		{
			for(int y = 1; y <= int.Parse(txtGroupsR[x]); y++)
			{
				string texturaFolder = "Assets/Usinas/"+txtUsina.ToString()+"/Texturas/Paineis2/usina_"+txtUsina.ToString()+"_painel_direito_grupo" + x + "_id" + y + ".jpg";
				Debug.Log(texturaFolder);
				Texture2D texturaPanel = (Texture2D)AssetDatabase.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
				GameObject panel = GameObject.Find("painel_direito_grupo" + x + "_id" + y);
				Debug.Log(panel.name);
				if(texturaPanel == null)
				{
					texturaFolder = "Assets/Usinas/Texturas/painelPrefabNotFound.png";
					texturaPanel = (Texture2D)AssetDatabase.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
					panel.GetComponent<Renderer>().material.mainTexture = texturaPanel;
					
				}
				else
				{
					panel.GetComponent<Renderer>().material.mainTexture = texturaPanel;
				}		
			}
		}
	}
	void texturizarL(string usina)
	{


		for (int x = 1; x <= int.Parse(txtQtdPanelGroupL); x++) 
		{
			for(int y = 1; y <= int.Parse(txtGroupsL[x]); y++)
			{
				string texturaFolder = "Assets/Usinas/"+txtUsina.ToString()+"/Texturas/Paineis2/usina_"+txtUsina.ToString()+"_painel_esquerdo_grupo" + x + "_id" + y + ".jpg";
				Debug.Log(texturaFolder);
				Texture2D texturaPanel = (Texture2D)AssetDatabase.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
				GameObject panel = GameObject.Find("painel_esquerdo_grupo" + x + "_id" + y);
				Debug.Log(panel.name);
				if(texturaPanel == null)
				{
					texturaFolder = "Assets/Usinas/Texturas/painelPrefabNotFound.png";
					texturaPanel = (Texture2D)AssetDatabase.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
					panel.GetComponent<Renderer>().material.mainTexture = texturaPanel;
					
				}
				else
				{
					panel.GetComponent<Renderer>().material.mainTexture = texturaPanel;
				}		
			}
		}





		
//		for (int x = 1; x <= int.Parse(qtdPaineisR); x++) 
//		{
//			string texturaFolder = "Assets/Usinas/"+txtUsina.ToString()+"/Texturas/Paineis/usina_"+txtUsina.ToString()+"_maquinaX_painel"+x+"_R.png";
//			Texture2D texturaPanel = (Texture2D)Resources.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
//			
//			GameObject panel = GameObject.Find("painel_direito_" + x);
//			
//			if(texturaPanel == null)
//			{
//				texturaFolder = "Assets/Usinas/Texturas/painelPrefabNotFound.png";
//				texturaPanel = (Texture2D)Resources.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
//				panel.renderer.material.mainTexture = texturaPanel;
//				
//			}
//			else
//			{
//				panel.renderer.material.mainTexture = texturaPanel;
//			}					
//			
//		}
//		
//		for (int x = 1; x <= int.Parse(qtdPaineisL); x++) 
//		{
//			string texturaFolder = "Assets/Usinas/"+txtUsina.ToString()+"/Texturas/Paineis/usina_"+txtUsina.ToString()+"_maquinaX_painel"+x+"_L.png";
//			Texture2D texturaPanel = (Texture2D)Resources.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
//			
//			GameObject panel = GameObject.Find("painel_esquerdo_" + x);
//			
//			if(texturaPanel == null)
//			{
//				texturaFolder = "Assets/Usinas/Texturas/painelPrefabNotFound.png";
//				texturaPanel = (Texture2D)Resources.LoadAssetAtPath(texturaFolder, typeof(Texture2D));
//				panel.renderer.material.mainTexture = texturaPanel;
//				
//			}
//			else
//			{
//				panel.renderer.material.mainTexture = texturaPanel;
//			}
//		}
	}
	
	void clear()
	{
		
	}
	
}
