using UnityEngine;
using System.Collections;

public class VRWand_Controller : MonoBehaviour {

    #region Variables to be assigned in inspector 
    public float throwSpeed = 2;
    #endregion

    #region VR Controller variables
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    #endregion

    #region Private Variables
    private VRPlayer_Controller _playerVR;
    private VRInteraction vrInteraction;
    #endregion

    #region Public Variables
    [HideInInspector] public float rotInput;
    [HideInInspector] public float walkInput;
    public bool isLeftHand { get; private set; }
    public Vector3 velocity { get { return controller.velocity; } }
    public Vector3 throwVelocity { get { return throwSpeed * controller.velocity; } }
    public float triggerPressAmount { get { return VRInput.Vive.GetTriggerPressAmount(controller); } }
    #endregion

    #region Getters/Setters
    public bool SetVRInteraction(VRInteraction interaction)
    {
        if (interaction != null)
        {
            if (vrInteraction != null)
            {
                if (vrInteraction.state == InteractionState.Manipulating)
                {
                    return false;
                }
                vrInteraction.enabled = false;
            }

            vrInteraction = interaction;
            vrInteraction.enabled = true;

            return true;
        }

        return false;
    }
    public VRPlayer_Controller playerVR
    {
        get
        {
            if (_playerVR == null)
            {
                _playerVR = GetComponentInParent<VRPlayer_Controller>();
            }

            return _playerVR;
        }
    }
    #endregion

    private void Awake()
    {
        SetVRInteraction(transform.GetActiveComponentInChildren<VRInteraction>());
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        isLeftHand = trackedObj.transform.name.ToLower().Contains("left");
    }

    void Update () {
	    if (controller == null)
        {
            Debug.LogError("No tracked object find for this instance");
            return;
        }

        ProcessButtonsInput();
        ProcessMovementInput();        
    }

    void ProcessButtonsInput()
    {
        bool gripButtonDown = controller.GetPressDown(VRInput.Vive.gripButton);     
        bool triggerButtonDown = controller.GetPressDown(VRInput.Vive.triggerButton);   
        bool triggerButtonUp = controller.GetPressUp(VRInput.Vive.triggerButton);		
        bool gripButtonUp = controller.GetPressUp(VRInput.Vive.gripButton);

        if (triggerButtonDown)
        {
            vrInteraction.OnTriggerPress(this);
        }

        if (triggerButtonUp)
        {
            vrInteraction.OnTriggerRelease(this);
        }

        if (gripButtonDown)
        {
            vrInteraction.OnGripPress(this);
        }

        if (gripButtonUp)
        {
            vrInteraction.OnGripRelease(this);
        }
    }

    private void ProcessMovementInput()
    {
        Vector2 input = controller.GetAxis(VRInput.Vive.trackPadAxis);
        rotInput = input.x;
        if (!controller.GetPress(VRInput.Vive.padButton) || Mathf.Abs(rotInput) < 0.3f)
        {
            rotInput = 0f;
        }

        walkInput = input.y;
        if (!controller.GetPress(VRInput.Vive.padButton) || Mathf.Abs(walkInput) < 0.3f)
        {
            walkInput = 0f;
        }
    }

}
