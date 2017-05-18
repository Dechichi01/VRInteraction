using UnityEngine;
using UnityEditor;
using System.Collections;

public class modelLineUp : EditorWindow {

	[MenuItem("CEMIG/Distribuir Modulos")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(modelLineUp));
	}

	public GameObject obj = null;
	string txtQtd = "";
	bool btnGerar;
	bool btnDestruir;
	string txtDist;
	public float lastPos;

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI()
	{
		bool btnGerar;

		GUILayout.Label ("Distribuidor de Modelos", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal (GUILayout.Height(180));

		obj = (GameObject) EditorGUI.ObjectField(new Rect(3, 33, position.width - 6, 16), "Objeto: ", obj, typeof(GameObject));
		txtQtd = EditorGUI.TextField (new Rect(3, 63, position.width - 6, 16), "Quantidade: ", txtQtd);
		btnGerar = GUILayout.Button ("Gerar", GUILayout.Width(Screen.width/2));
		btnDestruir = GUILayout.Button ("Destruir", GUILayout.Width(Screen.width/2));
		txtDist = EditorGUI.TextField (new Rect(3, 93, position.width - 6, 16), "Distancia: ", txtDist);

		GUILayout.EndHorizontal ();

		if(btnGerar)
		{
			lastPos = obj.transform.localPosition.x + float.Parse(txtDist);
			GameObject objCopy = Instantiate(obj, new Vector3 (lastPos, obj.transform.localPosition.y, obj.transform.localPosition.z), Quaternion.Euler(-90, 90, 0)) as GameObject;
			objCopy.tag = "clone";
			for(int i = 0; i <= (System.Int32.Parse(txtQtd)) - 1; i++)
			{
				GameObject objCopy2 = Instantiate(obj, new Vector3 (lastPos, obj.transform.localPosition.y, obj.transform.localPosition.z), Quaternion.Euler(-90, 90, 0)) as GameObject;
				objCopy2.tag = "clone";
				lastPos = lastPos +  float.Parse(txtDist);
			}
		}

		if(btnDestruir)
		{
			GameObject[] objToDestroy = new GameObject[100];
			objToDestroy = GameObject.FindGameObjectsWithTag("clone");
			for(var i = 0 ; i < objToDestroy.Length ; i ++)
			{
				DestroyImmediate (objToDestroy[i]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
