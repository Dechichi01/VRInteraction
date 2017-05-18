using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sceneFade : MonoBehaviour {

	public float fadeSpeed = 0.01f;  
	public bool sceneStarting = false;      // Whether or not the scene is still fading in.
	public bool sceneEnding = false;
	public CanvasGroup fade;
	public Image imagemFade;

	public bool toScene = true;
	public string nextScene;
	public Vector3 nextPosition;
	public string lastTag;
	public Vector3 position;

	// Use this for initialization
	void Awake ()
	{
		fade = GetComponent<CanvasGroup> ();
		imagemFade = GetComponent<Image> ();

	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		print ("Starting: " + sceneStarting);
		print ("Ending: " + sceneEnding);

		// If the scene is starting...
		if(sceneStarting)
			// ... call the StartScene function.
			StartScene();
		
		if(sceneEnding)
			// ... call the EndScene function.
			EndScene();

		if(Input.GetKeyDown(KeyCode.M))
		{
			StartCoroutine(FadeToBlack());
		}
		if(Input.GetKeyDown(KeyCode.N))
		{
			StartCoroutine(FadeToClear());
		}
	}

	public void StartScene()
	{
		StartCoroutine(FadeToClear());
		sceneStarting = false;
	}

	public void EndScene()
	{
		StartCoroutine(FadeToBlack());
		sceneEnding = false;
//
//
//
//		if(toScene)
//		{
//			Application.LoadLevel(nextScene);
//		}
//		else
//		{
//			GameObject.FindGameObjectWithTag("Player").transform.position = nextPosition;
//			print ("Mudou!");
//		}


	}



	IEnumerator FadeToBlack()
	{
		while (fade.alpha <= 1)
		{
			fade.alpha += fadeSpeed * Time.deltaTime/2;
			yield return null;
		}
	}

	IEnumerator FadeToClear()
	{
		while (fade.alpha <= 1)
		{
			fade.alpha -= fadeSpeed * Time.deltaTime/2;
			yield return null;
		}
	}
}
