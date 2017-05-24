using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class HandController : VRInteraction {

    #region Inspector Variables								
    [SerializeField] private Animator _animHand;
    public Transform modelGrabPoint;
    public LayerMask interactMask;
    #endregion																		

    protected VRWand_Controller wand;
   
    public Animator animHand { get { return _animHand; } }
	public bool isLeftHand { get; private set; }

    private AnimatorOverrideController baseRunTimeAnim;

	protected override void Start() {
        base.Start();
        wand = GetComponentInParent<VRWand_Controller>();
        isLeftHand = wand.isLeftHand;

        baseRunTimeAnim = new AnimatorOverrideController(_animHand.runtimeAnimatorController);

        SetCollisionRestrictions();
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

    private void SetCollisionRestrictions()
    {
        int objectLayer = gameObject.layer;
        
        for (int i = 0; i < 32; i++)
        {
            Physics.IgnoreLayerCollision(objectLayer, i, true);
            if (((1<<i) & interactMask) > 0)
            {
                Physics.IgnoreLayerCollision(objectLayer, i, false);
            }
        }
    }

    public void RecoverBaseAnimator()
    {
        PersistentAnimator.instance.ChangeAnimRunTime_SmoothTransition(_animHand, baseRunTimeAnim, this);
    }

}
