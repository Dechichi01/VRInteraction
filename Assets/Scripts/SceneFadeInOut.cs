using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
	public float fadeSpeed;
	public bool sceneStarting = true;
	public bool sceneEnding = false;
	public CanvasGroup fadeSystem;

	public string nextScene;
	public Vector3 nextPosition;

	public bool toScene = true;
	

	void Start()
	{
		fadeSystem = GetComponent<CanvasGroup> ();
	}

	void Awake()
	{
		sceneStarting = true;
		sceneEnding = false;
//		GetComponent<CanvasGroup> ().alpha = 1f;
	}

	void Update()
	{
//		print ("Alpha: " + GameObject.Find ("Image").GetComponent<UnityEngine.UI.Image> ().canvasRenderer.GetAlpha());
//		print ("Scene Starting: " + sceneStarting);

		if(Input.GetKeyDown(KeyCode.M))
		{
			sceneStarting = false;
			sceneEnding = true;
		}

		if(Input.GetKeyDown(KeyCode.N))
		{
			sceneStarting = true;
			sceneEnding = false;
		}


		if(sceneStarting)
		{
			StartScene();
		}
		if(sceneEnding)
		{
			EndScene();
		}

	}

	void FadeToClear()
	{
		GameObject.Find("Image").GetComponent<UnityEngine.UI.Image> ().CrossFadeAlpha (0f, fadeSpeed, false);
	}

	void FadeToBlack()
	{
//		GameObject.Find ("Image").GetComponent<UnityEngine.UI.Image> ().canvasRenderer.SetAlpha (0f);
		GameObject.Find("Image").GetComponent<UnityEngine.UI.Image> ().CrossFadeAlpha (1f, fadeSpeed, false);
	}

	void StartScene()
	{
		FadeToClear ();

		if(GameObject.Find ("Image").GetComponent<UnityEngine.UI.Image> ().canvasRenderer.GetAlpha() <= 0.1f)
		{
			sceneStarting = false;
			//print ("Terminou!!");
		}
	}

	public void EndScene()
	{
		FadeToBlack ();

		if(GameObject.Find ("Image").GetComponent<UnityEngine.UI.Image> ().canvasRenderer.GetAlpha() >= 0.9f)
		{
			sceneEnding = false;
			if(toScene)
			{
				Application.LoadLevel(nextScene);
			}
			else
			{
				GameObject.FindGameObjectWithTag("Player").transform.position = nextPosition;
				sceneEnding = false;
				sceneStarting = true;
			}
		}
	}
}