using UnityEngine;
using System.Collections;
using System;

public abstract class Pickup : Interactable
{
    #region Inspector Variables
    [SerializeField] protected bool releaseWithGripOnly;
    [SerializeField] protected bool useMirroredRotations = true;
    [SerializeField] [Range(0,1)] protected float squeeze;
    [SerializeField] protected AnimatorOverrideController animOverride;
    #endregion

    [SerializeField] [HideInInspector] protected Vector3 rightHeldPosition;
    [SerializeField] [HideInInspector] protected Vector3 leftHeldPosition;
    [SerializeField] [HideInInspector] protected Vector3 rightHeldRotation;
    [SerializeField] [HideInInspector] protected Vector3 leftHeldRotation;

    private Transform _tRoot;
    public Transform tRoot
    {
        get
        {
            if (_tRoot == null)
            {
                _tRoot = transform.root;
            }
            return _tRoot;
        }
    }

    public bool isBeingHeld { get { return holder != null; } }

    protected HandController holder = null;
    protected Rigidbody rby;

    protected abstract void GetPicked(VRInteraction interaction);
    protected abstract void GetDropped(Vector3 throwVelocity);

    public float GetSqueezeValue()
    {
        return squeeze;
    }

    protected override void Start()
    {
        base.Start();
        _tRoot = transform.root;
        rby = GetComponentInParent<Rigidbody>();
        holder = null;
    }

    public override void OnTriggerPress(VRInteraction caller, VRWand_Controller wand)
    {
        if (!isBeingHeld && caller.CanManipulate(this))
        {
            GetPicked(caller);
        }
    }

    public override void OnTriggerRelease(VRInteraction caller, VRWand_Controller wand)
    {
        if (!releaseWithGripOnly && holder == caller)
        {
            GetDropped(wand.throwVelocity);
        }
    }

    public override void OnGripPress(VRInteraction caller, VRWand_Controller wand)
    {
        if (holder == caller)
        {
            GetDropped(wand.throwVelocity);
        }
    }

    public override void OnGripRelease(VRInteraction caller, VRWand_Controller wand)
    {
        
    }

    #region Used by inspector only
    public void SetPositionAndRotation()
    {
        if (isBeingHeld)
        {
            tRoot.localPosition = (holder.isLeftHand) ? leftHeldPosition : rightHeldPosition;
            tRoot.localRotation = (holder.isLeftHand) ? Quaternion.Euler(leftHeldRotation) : Quaternion.Euler(rightHeldRotation);
        }
    }

    public void UpdateOffSetsFromCurrentPos()
    {
        if (isBeingHeld)
        {
            if (holder.isLeftHand)
            {
                leftHeldPosition = tRoot.localPosition;
                leftHeldRotation = tRoot.localRotation.eulerAngles;
                if (useMirroredRotations)
                {
                    rightHeldPosition = new Vector3(-tRoot.localPosition.x, tRoot.localPosition.y, tRoot.localPosition.z);
                    rightHeldRotation = new Quaternion(tRoot.localRotation.x, -tRoot.localRotation.y,
                        -tRoot.localRotation.z, tRoot.localRotation.w).eulerAngles;
                }
            }
            else
            {
                rightHeldPosition = tRoot.localPosition;
                rightHeldRotation = tRoot.localRotation.eulerAngles;
                if (useMirroredRotations)
                {
                    leftHeldPosition = new Vector3(-tRoot.localPosition.x, tRoot.localPosition.y, tRoot.localPosition.z);
                    leftHeldRotation = new Quaternion(tRoot.localRotation.x, -tRoot.localRotation.y,
                        -tRoot.localRotation.z, tRoot.localRotation.w).eulerAngles;
                }
            }
        }
    }

    public void UpdateSqueezeValue()
    {
        if (isBeingHeld)
        {
            OnManipulationStarted(holder);
        }
    }
    #endregion

    public override void OnSelected(VRInteraction caller)
    {
        base.OnSelected(caller);
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.SetAnimOverride(animOverride);
        }
    }

    public override void OnDeselected(VRInteraction caller)
    {
        base.OnDeselected(caller);
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.RecoverBaseAnimator();
        }
    }

    public override void OnManipulationStarted(VRInteraction caller)
    {
        base.OnManipulationStarted(caller);
        HandController hand = caller as HandController;
        if (hand != null)
        {
            holder = hand;
            tRoot.parent = holder.modelGrabPoint;
            hand.SetGrabAnimParams(this);
        }
    }

    public override void OnManipulationEnded(VRInteraction caller)
    {
        base.OnManipulationEnded(caller);
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.SetReleaseAnimParams(this);
        }

        holder = null;

        tRoot.parent = null;
    }
}
