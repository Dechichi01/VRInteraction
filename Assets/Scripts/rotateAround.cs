using UnityEngine;
using System.Collections;

public class rotateAround : MonoBehaviour {
	
	public Transform target;
	public Vector3 bkpPos;
	public Vector3 bkpRot;
	public float rotateSpeed = 20.0f;
	public float angleMax = 30.0f;
	public bool canRotate = true;
	
	private float rotationX = 0f;
	private float sensitivityX = 2f;
	public float minimumX = -10;
	public float maximumX = 10;
	
	private float rotationY = 0f;
	private float sensitivityY = 2f;
	public float minimumY = -10;
	public float maximumY = 10;
	
	//	public ParticleRenderer[] particulas = new ParticleRenderer[12];
	
	//	public GameObject water;
	//	public bool moveWater = false;
	public Vector3 newPosition;
	
	private Vector3 initialVector = Vector3.forward;
	// Use this for initialization
	void Start () {
		
//		target = GameObject.Find("mapa").transform;
	
		
		//		moveWater = false;
		//		newPosition = water.transform.position;
		
		bkpPos = transform.position;
		bkpPos.x = transform.position.x;
		bkpPos.y = transform.position.y;
		bkpPos.z = transform.position.z;
		bkpRot.x = transform.eulerAngles.x;
		bkpRot.y = transform.eulerAngles.y;
		bkpRot.z = transform.eulerAngles.z;
		
		initialVector = transform.position - target.position;
		initialVector.y = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		float rotateDegrees = 0f;
		
		rotationY += Input.GetAxis("Horizontal") * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
		target.transform.eulerAngles = new Vector3 (target.transform.eulerAngles.x, rotationY, target.transform.eulerAngles.z);
		
		rotationX += Input.GetAxis("Vertical") * sensitivityX;
		rotationX = Mathf.Clamp (rotationX, minimumX, maximumX);
		target.transform.eulerAngles = new Vector3 (target.transform.eulerAngles.x, target.transform.eulerAngles.y, rotationX);
		
		//		print (water.transform.position);
		//
		//		water.transform.position = Vector3.Lerp (water.transform.position, newPosition, 0.1f);
		//
		//		if (Input.GetKeyDown(KeyCode.Space)) 
		//		{
		//			if(moveWater)
		//			{
		//				moveWater = false;
		//				newPosition = water.transform.position;
		//				newPosition.y = -15f;
		//			}
		//			else
		//			{
		//				moveWater = true;
		//				newPosition = water.transform.position;
		//				newPosition.y = -19.8f;
		//			}
		//
		//		}
		
		if(!canRotate)
		{
			Quaternion qr = Quaternion.Euler(bkpRot);
			transform.rotation = Quaternion.Lerp(transform.rotation, qr, 5 * Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, bkpPos, 100 * Time.deltaTime);
		}
		
	}
	
	public void ResetarCamera()
	{
		GameObject mCamera = GameObject.FindGameObjectWithTag ("Main Camera");
		print(mCamera.transform.eulerAngles);
	}
}
