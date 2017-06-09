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
    public Animator animHand { get { return _animHand; } }
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

    public void RecoverBaseAnimator()
    {
        PersistentAnimator.instance.ChangeAnimRunTime_SmoothTransition(_animHand, baseRunTimeAnim, this);
    }

}
