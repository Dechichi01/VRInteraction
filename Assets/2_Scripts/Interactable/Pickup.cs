using UnityEngine;
using System.Collections;
using System;

public abstract class Pickup : Interactable
{
    #region Inspector Variables
    [SerializeField] protected Transform _pickupT;
    [SerializeField] protected bool releaseWithGripOnly;
    [SerializeField] protected bool useMirroredRotations = true;
    [SerializeField] [Range(0,1)] protected float squeeze;
    [SerializeField] protected AnimatorOverrideController animOverride;
    #endregion

    [SerializeField] [ReadOnly] protected Vector3 rightHeldPosition;
    [SerializeField] [ReadOnly] protected Vector3 leftHeldPosition;
    [SerializeField] [ReadOnly] protected Vector3 rightHeldRotation;
    [SerializeField] [ReadOnly] protected Vector3 leftHeldRotation;

    public bool isBeingHeld { get { return holder != null; } }

    protected HandController holder = null;
    protected Rigidbody _rby;
    protected Transform pickupPrevParent = null;

    public Transform pickupT { get { return _pickupT; } }
    public Rigidbody rby { get { return _rby; } }

    protected virtual void GetPicked(VRInteraction interaction)
    {
        interaction.SetManipulatedInteractable(this);
    }

    protected virtual void GetDropped(Vector3 throwVelocity)
    {
        holder.SetManipulatedInteractable(null);
        _rby.velocity = throwVelocity;
    }

    public float GetSqueezeValue()
    {
        return squeeze;
    }

    protected override void Start()
    {
        base.Start();
        _rby = pickupT.GetComponent<Rigidbody>();
        holder = null;
        pickupPrevParent = _pickupT.parent;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnSelectAddListener(OnSelectCallback);
        OnDeselectAddListener(OnDeselectCallback);
        OnManipulateAddListener(OnManipulateCallback);
        OnReleaseAddListener(OnReleaseCallback);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnSelectRemoveListener(OnSelectCallback);
        OnDeselectRemoveListener(OnDeselectCallback);
        OnManipulateRemoveListener(OnManipulateCallback);
        OnReleaseRemoveListener(OnReleaseCallback);
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
            _pickupT.localPosition = (holder.isLeftHand) ? leftHeldPosition : rightHeldPosition;
            _pickupT.localRotation = (holder.isLeftHand) ? Quaternion.Euler(leftHeldRotation) : Quaternion.Euler(rightHeldRotation);
        }
    }

    public void UpdateHoldParamsFromCurrentPos()
    {
        if (isBeingHeld)
        {
            if (holder.isLeftHand)
            {
                leftHeldPosition = _pickupT.localPosition;
                leftHeldRotation = _pickupT.localRotation.eulerAngles;
                if (useMirroredRotations)
                {
                    rightHeldPosition = MathUtils.GetMirroredPosition(leftHeldPosition);
                    rightHeldRotation = MathUtils.GetMirroredRotation(Quaternion.Euler(leftHeldRotation)).eulerAngles;
                }
            }
            else
            {
                rightHeldPosition = _pickupT.localPosition;
                rightHeldRotation = _pickupT.localRotation.eulerAngles;
                if (useMirroredRotations)
                {
                    leftHeldPosition = MathUtils.GetMirroredPosition(rightHeldPosition);
                    leftHeldRotation = MathUtils.GetMirroredRotation(Quaternion.Euler(rightHeldRotation)).eulerAngles;
                }
            }
        }
    }

    public void UpdateSqueezeValue()
    {
        if (isBeingHeld)
        {
            OnManipulated(holder);
        }
    }

    public Vector3[] GetHoldParameters()
    {
        return new Vector3[4] { rightHeldPosition, rightHeldRotation, leftHeldPosition, leftHeldRotation };
    }

    public void SetHoldParameters(Vector3 rightPos, Vector3 rightRot, Vector3 leftPos, Vector3 leftRot)
    {
        rightHeldPosition = rightPos;
        rightHeldRotation = rightRot;
        leftHeldPosition = leftPos;
        leftHeldRotation = leftRot;
    }
    #endregion

    protected virtual void OnSelectCallback(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.SetAnimOverride(animOverride);
        }
    }

    protected virtual void OnDeselectCallback(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.RecoverBaseAnimator();
        }
    }

    protected virtual void OnManipulateCallback(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            holder = hand;
            _pickupT.SetParent(holder.modelGrabPoint);
            hand.SetGrabAnimParams(this);

            SetPositionAndRotation();
            _rby.useGravity = false;
            _rby.isKinematic = true;
        }
    }

    protected virtual void OnReleaseCallback(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.SetReleaseAnimParams(this);
        }

        holder = null;

        _pickupT.SetParent(pickupPrevParent);

        _rby.useGravity = true;
        _rby.isKinematic = false;
    }
}
