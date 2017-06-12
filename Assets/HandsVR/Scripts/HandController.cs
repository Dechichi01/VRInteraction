using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class HandController : VRInteraction {

    #region Inspector Variables								
    [SerializeField] private Animator _animHand;
    [SerializeField] private AnimatorOverrideController baseRunTimeAnim;
    public Transform modelGrabPoint;
    #endregion																		

    protected VRWand_Controller _wand;
    public VRWand_Controller wand
    {
        get
        {
            if (_wand == null)
            {
                _wand = GetComponentInParent<VRWand_Controller>();
            }
            return _wand;
        }
    }
    protected Animator animHand { get { return _animHand; } }
	public bool isLeftHand { get; private set; }

	protected override void Start() {
        base.Start();
        _wand = GetComponentInParent<VRWand_Controller>();
        isLeftHand = wand.isLeftHand;

        RecoverBaseAnimator();
	}

    protected virtual void Update()
    {
        _animHand.SetFloat("closeAmount", wand.triggerPressAmount);
    }

    protected virtual void OnEnable()
    {
        
    }

    protected override void DisableInteration()
    {
        base.DisableInteration();
        GetComponent<Collider>().enabled = false;
    }

    protected override void EnableInteration()
    {
        base.EnableInteration();
        GetComponent<Collider>().enabled = true;
    }

    public override void OnTriggerPress(VRWand_Controller wand)
    {
        if (interactable != null)
        {
            interactable.OnTriggerPress(this, wand);
        }
    }

    public override void OnTriggerRelease(VRWand_Controller wand)
    {
        if (interactable != null)
        {
            interactable.OnTriggerRelease(this, wand);
        }
    }

    public override void OnGripPress(VRWand_Controller wand)
    {
        if (interactable != null)
        {
            interactable.OnGripPress(this, wand);
        }
    }

    public override void OnGripRelease(VRWand_Controller wand)
    {
        if (interactable != null)
        {
            interactable.OnTriggerRelease(this, wand);
        }
    }

    public void SetGrabAnimParams(Pickup pickup)
    {
        if (pickup == currManipulatedInteractable)
        {
            animHand.SetBool("Grab", true);
            animHand.SetFloat("Squeeze", pickup.GetSqueezeValue());
        }
    }

    public void SetReleaseAnimParams(Pickup pickup)
    {
        animHand.SetBool("Grab", false);
    }

    public void SetAnimOverride(AnimatorOverrideController animOverride)
    {
        if (animOverride != null)
        {
            PersistentAnimator.instance.ChangeAnimRunTime_SmoothTransition(animHand, animOverride, this);
        }
    }

    public void RecoverBaseAnimator()
    {
        SetAnimOverride(baseRunTimeAnim);
    }

}
