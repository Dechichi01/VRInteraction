using UnityEngine;
using UnityEditor;
using System.Collections;

public class tempMapSceneGenerator : EditorWindow {
	
	[MenuItem("CEMIG/mapTest")]
	
	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(tempMapSceneGenerator));
	}
	
	static void Init() {
		UnityEditor.EditorWindow window = GetWindow(typeof(tempMapSceneGenerator));
		window.position = new Rect(0, 0, 250, 80);
		window.Show();
	}
	
	void OnInspectorUpdate() {
		Repaint();
	}

	public string txtUsina;
	public GameObject mapMesh = null;
	public Texture2D mapTexture = null;
	public Camera mapCamera = null;
	public Material mapSkybox = null;
	public bool btnGerar;
	public bool btnDestruir;
	public Vector3 cameraPosition;
	public Vector3 cameraRotation;
	public Quaternion cameraRotation2;


	bool m_supposedToCheckTime = false;
	float m_time = 0.0f;

	bool checkLista = true;
	bool checkTerreno = true;
	bool checkTextura = true;
	bool checkCamera = true;
	bool checkPinSub = true;
	bool checkPinUsi = true;
	bool checkPinTextura = true;
	bool checkLuz = true;
	bool checkTexto = true;
	bool checkAgua = true;
	bool checkAgua2 = true;
	bool checkCachoeira = true;
	bool checkSkybox = true;

	
	// Use this for initialization
	void Start () {

		cameraPosition = new Vector3(0, 200, 0);

	}
	
	// Update is called once per frame
	void Update () {

		Repaint();
//		Debug.Log (m_time);
		if (m_supposedToCheckTime)
		{
			m_time += 0.01f;
			
			if (m_time >= 10.0f)
			{
				//make sure you reset your time
				m_time = 0.0f;
				m_supposedToCheckTime = false;
				
				//TODO: take action
			}
		}


	}

	void destruir()
	{
		GameObject[] objToDestroy = new GameObject[10];
		GameObject[] objToDestroy2 = new GameObject[10];
		GameObject[] objToDestroy3 = new GameObject[10];
		objToDestroy = GameObject.FindGameObjectsWithTag("cenaMapa");
		objToDestroy2 = GameObject.FindGameObjectsWithTag("particula");
		objToDestroy3 = GameObject.FindGameObjectsWithTag("Untagged");
		for(var i = 0 ; i < objToDestroy.Length ; i ++)
		{
			DestroyImmediate (objToDestroy[i]);
		}
		for(var i = 0 ; i < objToDestroy2.Length ; i ++)
		{
			DestroyImmediate (objToDestroy2[i]);
		}
		for(var i = 0 ; i < objToDestroy3.Length ; i ++)
		{
			DestroyImmediate (objToDestroy3[i]);
		}
	}
	
	void OnGUI()
	{
		GUILayout.Label ("Gerador de Mapa", EditorStyles.boldLabel);
		
		GUILayout.BeginHorizontal (GUILayout.Height(290));
		
		GUILayout.Label ("");

		txtUsina = EditorGUI.TextField(new Rect(3, 30, position.width - 6, 16), "Usina: ", txtUsina);
		mapMesh = (GameObject) EditorGUI.ObjectField(new Rect(3, 53, position.width - 6, 16), "Mapa Mesh: ", mapMesh, typeof(GameObject));
		mapCamera = (Camera) EditorGUI.ObjectField (new Rect (3,83,position.width - 6, 16), "Mapa Camera: ", mapCamera, typeof(Camera));
		cameraPosition = EditorGUI.Vector3Field (new Rect(3,113,position.width-6,20), "Posiçao da Camera: ", cameraPosition);
		cameraRotation = EditorGUI.Vector3Field (new Rect(3,143,position.width-6,20), "Rotaçao da Camera: ", cameraRotation);
		mapTexture = (Texture2D) EditorGUI.ObjectField (new Rect (3,193,position.width - 6,70), "Mapa Texura: ", mapTexture, typeof(Texture2D));
		mapSkybox = (Material) EditorGUI.ObjectField (new Rect (3,273,position.width - 6,16), "Skybox: ", mapSkybox, typeof(Material));
		
		GUILayout.EndHorizontal ();
		
		
		btnGerar = GUILayout.Button ("Gerar Cena");
		btnDestruir = GUILayout.Button ("Destruir Cena");
		if(btnGerar)
		{
			m_supposedToCheckTime = true;

//			Debug.Log (m_time);
			m_supposedToCheckTime = true;

			destruir();
			
			if(mapCamera == null)
			{
				mapCamera = Camera.main;
			}
			
			Object pin = AssetDatabase.LoadAssetAtPath("Assets/CenaMapa/Pin.fbx", typeof(GameObject));
			
			Object IGUIPrefab = AssetDatabase.LoadAssetAtPath("Assets/Usinas/Prefabs/listaIGUI.prefab", typeof(GameObject));
			GameObject listaIGUI = Instantiate(IGUIPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			listaIGUI.name = "lista";
			listaIGUI.tag = "cenaMapa";
//			GameObject.Find("ListaUsina").GetComponent<iGUI.iGUIDropDownList>().setWidth(0.4f);
//			GameObject.Find("ListaUsina").GetComponent<iGUI.iGUIDropDownList>().label.text = "";
//			GameObject.Find("ListaUsina").GetComponent<iGUI.iGUIDropDownList>().enabled = false;
//			GameObject.Find("ListaUsina").GetComponent<iGUI.iGUIDropDownList>().options[0].text = "Patio dos Transformadores";
//			GameObject.Find("ListaUsina").GetComponent<iGUI.iGUIDropDownList>().options[1].text = "Galeria Eletrica";
//			GameObject.Find("ListaUsina").GetComponent<iGUI.iGUIDropDownList>().options[2].text = "Galeria Mecanica";

			mapCamera.transform.position = cameraPosition;
			Vector3 temp = cameraRotation;


//			mapCamera.transform.rotation = cameraRotation2;



			//checkLista
			
			GameObject terreno = Instantiate(mapMesh, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			terreno.name = "mapa";
			terreno.tag = "cenaMapa";
//			terreno.AddComponent<rotateAround>();

			//checkTerreno

			//			Renderer mesh = terreno.GetComponentInChildren<Renderer>();
			Renderer mesh = GameObject.Find("terreno").GetComponent<Renderer>();
			mesh.material.color = Color.white;
			mesh.material.mainTexture = mapTexture;

			//checkTextura

//			mapCamera.transform.LookAt(terreno.transform);
			
			//checkCamera
			
//			GameObject pinSub = Instantiate(pin, new Vector3(100, 100, 100), Quaternion.identity) as GameObject;
//			pinSub.name = "Subestacao";
//			pinSub.tag = "cenaMapa";
//			pinSub.transform.position = GameObject.Find ("targetSubestacao").transform.position;
//			pinSub.AddComponent<BoxCollider>();
//			pinSub.GetComponent<BoxCollider>().size *= 19;
//			pinSub.AddComponent<Selecao>();

			//checkPinSub

			GameObject pinUsi = Instantiate(pin, new Vector3(100, 100, 100), Quaternion.identity) as GameObject;
			pinUsi.name = "Usina";
			pinUsi.tag = "cenaMapa";
			pinUsi.transform.position = GameObject.Find ("targetUsina").transform.position;
			pinUsi.AddComponent<BoxCollider>();
			pinUsi.GetComponent<BoxCollider>().size *= 19;
			pinUsi.AddComponent<Selecao>();



			//checkPinUsi

			Texture2D pinUsiTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Usinas/Texturas/pinUsina.png", typeof(Texture2D));
			pinUsi.transform.FindChild("pinHolder").GetComponent<Renderer>().material.mainTexture = pinUsiTexture;

			pinUsi.transform.parent = terreno.transform;

			//checkPinTextura
			
			GameObject lightGameObject = new GameObject("directLight");
			Light lightComp = lightGameObject.AddComponent<Light>();
			lightComp.type = LightType.Directional;
			lightComp.transform.eulerAngles = new Vector3(50, 0, 0);
			lightComp.intensity = 0.6f;
			lightGameObject.transform.position = new Vector3(0, 5, 0);
			lightGameObject.tag = "cenaMapa";

			//checkLuz
			
			GameObject txtGameObject = new GameObject("txt");
			GUIText txtComp = txtGameObject.AddComponent<GUIText>();
			txtComp.tabSize = 4;
			txtComp.fontSize = 20;
			txtComp.fontStyle = FontStyle.Bold;
			txtGameObject.tag = "cenaMapa";

			//checkTexto
			
			Object waterPrefab = AssetDatabase.LoadAssetAtPath("Assets/Usinas/Prefabs/water2.prefab", typeof(GameObject));
			GameObject waterObj = Instantiate(waterPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			waterObj.transform.position = GameObject.Find("agua").transform.position;
			waterObj.name = "water";
			waterObj.tag = "cenaMapa";
//			waterObj.transform.eulerAngles = new Vector3(0, 90, -90);

			//checkAgua

//			Mesh meshWater = GameObject.Find("Tile").GetComponent<MeshFilter>().mesh;




			GameObject.Find("Tile").GetComponent<MeshFilter>().mesh = GameObject.Find("agua").GetComponent<MeshFilter>().mesh;
			MeshRenderer meshAgua = GameObject.Find("agua").GetComponent<MeshRenderer>();
			meshAgua.enabled = false;




//
//			//checkAgua2
//			
			waterObj.transform.parent = terreno.transform;
//			
			Object waferfallPrefab = AssetDatabase.LoadAssetAtPath("Assets/Usinas/Prefabs/waterfall3.prefab", typeof(GameObject));
			GameObject waterfallObj = Instantiate(waferfallPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			waterfallObj.transform.position = GameObject.Find("duto").transform.position;
			waterfallObj.tag = "Untagged";
			
			waterfallObj.transform.parent = terreno.transform;
//
//			//checkCachoeira
//			
//			RenderSettings.skybox = mapSkybox;
//
//			//checkSkybox

			mapCamera.gameObject.AddComponent<rotateAround>();
			rotateAround scriptRotate =  mapCamera.gameObject.GetComponent<rotateAround>();
			scriptRotate.target = GameObject.Find("mapa").transform;

			
		}

		if(mapCamera != null)
		{
			cameraPosition = mapCamera.transform.position;
			cameraRotation = mapCamera.transform.eulerAngles;
			cameraRotation2.eulerAngles = mapCamera.transform.rotation.eulerAngles;
		}

		if(btnDestruir)
		{
			destruir();
		}
		
		//		if (mapMesh)
		//			if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Dependencies"))
		//				Selection.objects = EditorUtility.CollectDependencies(new GameObject[] {mapMesh});
		//		
		//		else
		//			EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
	}
}
