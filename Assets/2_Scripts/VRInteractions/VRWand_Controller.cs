using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VRInputManager))]
public class VRWand_Controller : MonoBehaviour {

    #region Variables to be assigned in inspector 
    public float throwSpeed = 2;
    #endregion

    #region Private Variables
    private VRInputManager inputManager;
    private VRPlayer_Controller _playerVR;
    private VRInteraction vrInteraction;
    #endregion

    #region Public Variables
    [HideInInspector] public float rotInput;
    [HideInInspector] public float walkInput;
    public bool isLeftHand { get; private set; }
    public Vector3 throwVelocity { get { return throwSpeed * inputManager.velocity; } }
    public float triggerPressAmount { get { return inputManager.triggerPressAmount; } }
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
        inputManager = GetComponent<VRInputManager>();
        SetVRInteraction(transform.GetActiveComponentInChildren<VRInteraction>());
    }

    void Update ()
    {
        ProcessButtonsInput();       
    }

    void ProcessButtonsInput()
    {
        bool gripButtonDown = inputManager.GetPressDown(VRInput.Vive.gripButton);     
        bool triggerButtonDown = inputManager.GetTriggerPressDown();   
        bool triggerButtonUp = inputManager.GetPressUp(VRInput.Vive.triggerButton);		
        bool gripButtonUp = inputManager.GetPressUp(VRInput.Vive.gripButton);

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
}
