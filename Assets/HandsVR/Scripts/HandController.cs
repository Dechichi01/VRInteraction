using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandController : MonoBehaviour {

    #region Inspector Variables
    [SerializeField] private GameObject ViveControllerRoot;       //(VRInput)										
    [SerializeField] private Animator animHand;
    #endregion

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }	//(VRInput)
	private SteamVR_TrackedObject trackedObj;																			//(VRInput)

	public HandController otherHand;
	public Transform controllerGrabPoint; //Used to measure real distance from the object
    public Transform modelGrabPoint;

	List<Pickup> nearby = new List<Pickup>();

    private Pickup closestPickUp;
    
	private Pickup heldPickUp = null;	
   
	public bool isLeftHand;

    private float grabToHoldDistance = float.MaxValue;

    private AnimatorOverrideController baseRunTimeAnim;

	void Start() {
        if(ViveControllerRoot != null){
			trackedObj = ViveControllerRoot.GetComponent<SteamVR_TrackedObject>();		//(VRInput)
        }

        baseRunTimeAnim = new AnimatorOverrideController(animHand.runtimeAnimatorController);
	}

    void Update(){
        GetClosest();
        ProcessVRInput();
    }

	void OnTriggerEnter(Collider obj){						// Whenever a new object hits the trigger, it adds it to a nearby list
        Pickup pickUp = obj.gameObject.GetComponent<Pickup>();

		if(pickUp != null && !nearby.Contains(pickUp))
            nearby.Add(pickUp);
	}

    void OnTriggerExit(Collider obj){                       // Removes the object from nearby list as it exits trigger
        Pickup pickup = obj.gameObject.GetComponent<Pickup>();

        if (pickup != null)
        {
            nearby.Remove(pickup);
            if (pickup == closestPickUp)
            {
                animHand.SetBool("Prep", false);
                SetCurrentClosest(null);
            }
        }
	}

    void ProcessVRInput(){
		bool gripButtonDown = controller.GetPressDown(VRInput.Vive.gripButton);		//(VRInput)
		bool triggerButtonDown = controller.GetPressDown(VRInput.Vive.triggerButton);	//(VRInput)
		bool triggerButtonUp = controller.GetPressUp(VRInput.Vive.triggerButton);		//(VRInput)

        animHand.SetFloat("closeAmount", VRInput.Vive.GetTriggerPressAmount(controller));

        if (triggerButtonDown && CanGrab() && heldPickUp == null)
            PickItUp();

		if (gripButtonDown && heldPickUp != null){			//button used to drop objects
            DropIt();
        }
    }

	//This routine picks up the object and parents it to the handmesh grabpoint
    void PickItUp() {
        if (closestPickUp.isBeingHeld)
            return;

        heldPickUp = closestPickUp;
        heldPickUp.GetPicked(this);

        animHand.SetBool("Grab", true);
        animHand.SetFloat("Squeeze", heldPickUp.squeeze);

		GetComponent<Collider>().enabled = false;
    }
    
	//Drops the object and resumes the hands colider to sence for more objects
    public void DropIt(){
        animHand.SetBool("Grab", false);
		
		GetComponent<Collider>().enabled = true;
		
        heldPickUp.GetDropped(controller.velocity);

        heldPickUp = null;
    }

	float NormalizeDistance(float dist){    //set  0 - 1 scale to control default to prep animation
                                            //Also contrain squeeze value here!
		dist =(Mathf.Clamp((.0009f / (Mathf.Pow(dist / 2f, 2.4f))) + .15f, 0f, 1f)) ;
		return dist;
	}

    void GetClosest(){		//used to find the nearest grab point

        if (nearby.Count == 0)
        {
            closestPickUp = null;
            grabToHoldDistance = float.MaxValue;
            return;
        }

        Pickup previousClosest = closestPickUp;
        closestPickUp = nearby[0];
        grabToHoldDistance = Vector3.Distance(closestPickUp.holdPoint.position, controllerGrabPoint.position);
		foreach (var nearbyObj in nearby) {
			float sqrDist = (nearbyObj.holdPoint.position - controllerGrabPoint.position).sqrMagnitude;
            if (sqrDist < Mathf.Pow(grabToHoldDistance, 2)){
                grabToHoldDistance = Mathf.Pow(sqrDist, 0.5f);
				closestPickUp = nearbyObj;
			}
		}

        if (previousClosest != closestPickUp)
        {
            SetCurrentClosest(closestPickUp);
            animHand.SetBool("Prep", true);
        }
    }

	//This is used to asses weather the object is in grabrange
	bool CanGrab(){
        return closestPickUp != null && grabToHoldDistance < closestPickUp.grabRange;
	}

    private void SetCurrentClosest(Pickup closest)
    {
        closestPickUp = closest;
        if (closestPickUp != null)
            closestPickUp.SetAnimOverride(animHand);
        else
            PersistentAnimator.instance.ChangeAnimRunTime_SmoothTransition(animHand, baseRunTimeAnim, this);
    }

}
