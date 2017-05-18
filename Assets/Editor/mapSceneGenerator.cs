using UnityEngine;
using UnityEditor;
using System.Collections;

public class mapSceneGenerator : EditorWindow {

	[MenuItem("CEMIG/Gerador de Mapa")]

	public static void  ShowWindow () {
		EditorWindow.GetWindow(typeof(mapSceneGenerator));
	}

	static void Init() {
		UnityEditor.EditorWindow window = GetWindow(typeof(mapSceneGenerator));
		window.position = new Rect(0, 0, 250, 80);
		window.Show();
	}

	void OnInspectorUpdate() {
		Repaint();
	}

	public GameObject mapMesh = null;
	public Texture2D mapTexture = null;
	public Camera mapCamera = null;
	public Material mapSkybox = null;
	public bool btnGerar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void destruir()
	{
		GameObject[] objToDestroy = new GameObject[10];
		objToDestroy = GameObject.FindGameObjectsWithTag("cenaMapa");
		for(var i = 0 ; i < objToDestroy.Length ; i ++)
		{
			DestroyImmediate (objToDestroy[i]);
		}
	}

	void OnGUI()
	{

		GUILayout.Label ("Gerador de Mapa", EditorStyles.boldLabel);

		GUILayout.BeginHorizontal (GUILayout.Height(180));

		GUILayout.Label ("");

		mapMesh = (GameObject) EditorGUI.ObjectField(new Rect(3, 33, position.width - 6, 16), "Mapa Mesh: ", mapMesh, typeof(GameObject));
		mapCamera = (Camera) EditorGUI.ObjectField (new Rect (3,63,position.width - 6, 16), "Mapa Camera: ", mapCamera, typeof(Camera));
		mapTexture = (Texture2D) EditorGUI.ObjectField (new Rect (3,93,position.width - 6,70), "Mapa Texura: ", mapTexture, typeof(Texture2D));
		mapSkybox = (Material) EditorGUI.ObjectField (new Rect (3,173,position.width - 6,16), "Skybox: ", mapSkybox, typeof(Material));

		GUILayout.EndHorizontal ();


		btnGerar = GUILayout.Button ("Gerar Cena");
		if(btnGerar)
		{

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

			mapCamera.transform.position = new Vector3(0, 199, 336);

			GameObject terreno = Instantiate(mapMesh, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			terreno.name = "mapa";
			terreno.tag = "cenaMapa";
			terreno.AddComponent<rotateAround>();
//			Renderer mesh = terreno.GetComponentInChildren<Renderer>();
			Renderer mesh = GameObject.Find("terreno").GetComponent<Renderer>();
			mesh.material.color = Color.white;
			mesh.material.mainTexture = mapTexture;
			mapCamera.transform.LookAt(terreno.transform);



			GameObject pinSub = Instantiate(pin, new Vector3(100, 100, 100), Quaternion.identity) as GameObject;
			pinSub.name = "Subestacao";
			pinSub.tag = "cenaMapa";
			pinSub.transform.position = GameObject.Find ("targetSubestacao").transform.position;
			pinSub.AddComponent<BoxCollider>();
			pinSub.GetComponent<BoxCollider>().size *= 19;
			pinSub.AddComponent<Selecao>();

			GameObject pinUsi = Instantiate(pin, new Vector3(100, 100, 100), Quaternion.identity) as GameObject;
			pinUsi.name = "Usina";
			pinUsi.tag = "cenaMapa";
			pinUsi.transform.position = GameObject.Find ("targetUsina").transform.position;
			pinUsi.AddComponent<BoxCollider>();
			pinUsi.GetComponent<BoxCollider>().size *= 19;
			pinUsi.AddComponent<Selecao>();

			Texture2D pinUsiTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Usinas/Texturas/pinUsina.png", typeof(Texture2D));
			pinUsi.transform.FindChild("pinHolder").GetComponent<Renderer>().material.mainTexture = pinUsiTexture;

			GameObject lightGameObject = new GameObject("directLight");
			Light lightComp = lightGameObject.AddComponent<Light>();
			lightComp.type = LightType.Directional;
			lightComp.transform.eulerAngles = new Vector3(50, 0, 0);
			lightComp.intensity = 0.6f;
			lightGameObject.transform.position = new Vector3(0, 5, 0);
			lightGameObject.tag = "cenaMapa";

			GameObject txtGameObject = new GameObject("txt");
			GUIText txtComp = txtGameObject.AddComponent<GUIText>();
			txtComp.tabSize = 4;
			txtComp.fontSize = 20;
			txtComp.fontStyle = FontStyle.Bold;
			txtGameObject.tag = "cenaMapa";

			Object waterPrefab = AssetDatabase.LoadAssetAtPath("Assets/Usinas/Prefabs/water.prefab", typeof(GameObject));
			GameObject waterObj = Instantiate(waterPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			waterObj.transform.position = GameObject.Find("agua").transform.position;
			waterObj.name = "water";
			waterObj.tag = "cenaMapa";
//			Mesh meshWater = GameObject.Find("Tile").GetComponent<MeshFilter>().mesh;
			GameObject.Find("Tile").GetComponent<MeshFilter>().mesh = GameObject.Find("agua").GetComponent<MeshFilter>().mesh;
			MeshRenderer meshAgua = GameObject.Find("agua").GetComponent<MeshRenderer>();
			meshAgua.enabled = false;

			waterObj.transform.parent = terreno.transform;

			Object waferfallPrefab = AssetDatabase.LoadAssetAtPath("Assets/Usinas/Prefabs/waterfall3.prefab", typeof(GameObject));
			GameObject waterfallObj = Instantiate(waferfallPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
			waterfallObj.transform.position = GameObject.Find("duto").transform.position;
			waterfallObj.tag = "cenaMapa";

			waterfallObj.transform.parent = terreno.transform;

			RenderSettings.skybox = mapSkybox;

		}

//		if (mapMesh)
//			if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Dependencies"))
//				Selection.objects = EditorUtility.CollectDependencies(new GameObject[] {mapMesh});
//		
//		else
//			EditorGUI.LabelField(new Rect(3, 25, position.width - 6, 20), "Missing:", "Select an object first");
	}
}
