using UnityEditor;
using UnityEngine;
using System;

public class CableCreator : EditorWindow {
	
	#region Private Attributes

	const float LarguraMin = 0.01f;
	const float LarguraMax = 0.2f;
	const float CurvaturaMin = 0.01f;
	const float CurvaturaMax = 0.5f;
	
	private float distance;
	private float largura = 0.1f;
	private float curvatura = 0.2f;  // % distance
	private GameObject inicio;
	private GameObject fim;
	private LineRenderer lineRenderer;
	private Parabola parabola;
	private static CableCreator cable;
	
	#endregion
	
	
	#region Properties
	
	public static CableCreator Instance {
		get {
			if ((cable) == null)
				cable = (CableCreator) EditorWindow.GetWindow(typeof(CableCreator));
			return cable;	
		}
	}
	
	#endregion
	
	
	#region Public Methods
	
	[MenuItem("CEMIG/Janelas/Cabos", false, 0)]
    static void Init() {
		//Construindo Janela
		Instance.position = new Rect(0, 0, 250, 80);
        Instance.title = "Extensões CEMIG";
        Instance.Show();
    }
	
	
    void OnInspectorUpdate() {
        Repaint();
    }


	void OnGUI() {

		bool selecionarA, selecionarB, desenhar;
		string entryLargura, entryCurvatura;
		float largNorm, curvNorm;

		selecionarA  = GUILayout.Button ("Extremidade A");
		selecionarB  = GUILayout.Button ("Extremidade B");
		desenhar     = GUILayout.Button ("Desenhar");

		//Tratamento de Faixa Incorreta
		if(largura < LarguraMin)
			largura = LarguraMin;
		else if(largura > LarguraMax)
			largura = LarguraMax;
		if(curvatura < CurvaturaMin)
			curvatura = CurvaturaMin;
		else if(curvatura > CurvaturaMax)
			curvatura = CurvaturaMax;


		//Largura
		GUILayout.BeginHorizontal();
		GUILayout.Label("Largura: ");
		largNorm = normalize(largura, LarguraMin, LarguraMax);
		entryLargura = GUILayout.TextField(largNorm.ToString("###"));
		try {
			largura  = denormalize(float.Parse(entryLargura), LarguraMin, LarguraMax);
		}
		catch(FormatException e) { 
			Debug.LogError("Valor de largura incorreto: " + entryLargura); 
		}
		if(GUILayout.Button ("+"))
			largura += (LarguraMax - LarguraMin) / 99;
		if(GUILayout.Button ("-"))
			largura -= (LarguraMax - LarguraMin) / 99;
		GUILayout.EndHorizontal();


		//Curvatura
		GUILayout.BeginHorizontal();
		GUILayout.Label("Curvatura: ");
		curvNorm = normalize(curvatura, CurvaturaMin, CurvaturaMax);
		entryCurvatura = GUILayout.TextField(curvNorm.ToString("###"));
		try {
			curvatura  = denormalize(float.Parse(entryCurvatura), CurvaturaMin, CurvaturaMax);
		}
		catch(FormatException e) { 
			Debug.LogError("Valor de curvatura incorreto: " + entryCurvatura); 
		}
		if(GUILayout.Button ("+"))
			curvatura += (CurvaturaMax - CurvaturaMin) / 99;
		if(GUILayout.Button ("-"))
			curvatura -= (CurvaturaMax - CurvaturaMin) / 99;
		GUILayout.EndHorizontal();


		if(selecionarA) 
			selectStart();
		if(selecionarB)
			selectEnd();
		if(desenhar)
    		drawCable();
		
		/*var curva = GUILayout.HorizontalSlider(0.5f, 0.0f, 0.1f);
		Debug.Log (curva);
		var texto = GUILayout.TextField("GameObject");
		Debug.Log (texto);*/
    }
	
	[MenuItem("CEMIG/Cabos/Selecionar Início &a", false, 1)]
    public static void MenuSelectStart () {
    	CableCreator.Instance.selectStart();
    }
	
	[MenuItem("CEMIG/Cabos/Selecionar Fim &s", false, 1)]
    public static void MenuSelectEnd () {
    	CableCreator.Instance.selectEnd();
    }
	
	[MenuItem("CEMIG/Cabos/Desenhar &d", false, 1)]
    public static void MenuDrawCable () {
    	CableCreator.Instance.drawCable();
    }
	
	public void selectStart() {
	    inicio = getSelection();
	}

	public void selectEnd() {
		fim = getSelection();
	}
	
	public void drawCable() {
		GameObject go = new GameObject();
		go.name = string.Format("Cabo_{0}_{1}", inicio.name, fim.name);
		go.AddComponent<LineRenderer>();
		//TODO: Adicionar material
		
		distance = evaluateDistance();
		var deltaY = curvatura * distance;
		Vector3 meio = (inicio.transform.position + fim.transform.position) / 2;
		meio.y -= deltaY;
		
		lineRenderer = (LineRenderer) go.GetComponent("LineRenderer");
		lineRenderer.SetWidth(largura, largura);
		Material m = Resources.Load("metal", typeof(Material)) as Material;
		lineRenderer.GetComponent<Renderer>().sharedMaterial = m;
		
		parabola = new Parabola(lineRenderer);
		parabola.Plot(inicio.transform.position , meio , fim.transform.position);
	}
	
	#endregion
	
	
	#region Private Methods

	private GameObject getSelection() {
		var selected = Selection.objects;
		if(selected.Length == 0)
			return null;
		else 
			return selected[0] as GameObject;
    }
	
	
	private float evaluateDistance() {
		Vector3 delta = inicio.transform.position - fim.transform.position;
		return Mathf.Sqrt( Mathf.Pow(delta.x, 2) + 
						   Mathf.Pow(delta.y, 2) + 
						   Mathf.Pow(delta.z, 2));
	}

	private float normalize(float val, float min, float max) {
		return (val - min) * 99f / (max - min);
	}

	private float denormalize(float normVal, float min, float max) {
		return (max - min) * normVal / 99f + min;
	}

	
	#endregion
}
