using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class VRPlayer_Controller : MonoBehaviour {

    public float speed = 2f;
    public float rotSpeed = 20f;
    public bool canMove;
    private Vector3 playerVelocity = Vector3.zero;
    private CharacterController playerController;

    public VRWand_Controller rightWand, leftWand;    

	void Start () {
        canMove = true;
        playerController = GetComponent<CharacterController>();
	}
	
    void FixedUpdate()
    {
        float rotFromRight = rightWand.rotInput;
        float rotFromLeft = leftWand.rotInput;

        float walkFromRight = rightWand.walkInput;
        float walkFromLeft = leftWand.walkInput;

        Vector3 fwd = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
        if (canMove)
        {
            transform.Rotate(Vector3.up * Time.fixedDeltaTime * rotSpeed * ((rotFromRight == 0) ? rotFromLeft : rotFromRight));
            playerController.SimpleMove(fwd * speed * ((walkFromRight == 0) ? walkFromLeft : walkFromRight));
        }
  
    }

}
