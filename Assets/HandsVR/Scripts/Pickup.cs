using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : Interactable
{
    #region Inspector Variables
    [SerializeField] protected bool releaseWithGripOnly;
    [SerializeField] protected bool useMirroredRotations = true;
    [SerializeField] protected float grabRange = .2f;
    [SerializeField] [Range(0,1)] protected float squeeze;
    [SerializeField] private AnimatorOverrideController animOverride;
    #endregion

    [HideInInspector] [SerializeField] protected Vector3 rightHeldPosition;
    [HideInInspector] [SerializeField] protected Vector3 leftHeldPosition;
    [HideInInspector] [SerializeField] protected Vector3 rightHeldRotation;
    [HideInInspector] [SerializeField] protected Vector3 leftHeldRotation;

    public bool isBeingHeld { get { return holder != null; } }

    protected HandController holder = null;
    protected Rigidbody rby;

    protected override void Start()
    {
        base.Start();
        rby = GetComponent<Rigidbody>();
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

    public override bool CanBeManipulated(Transform other)
    {
        return base.CanBeManipulated(other) && GetInteractionDistance(other) < grabRange;
    }

    #region Used by inspector only
    public void SetPositionAndRotation()
    {
        if (isBeingHeld)
        {
            transform.localPosition = (holder.isLeftHand) ? leftHeldPosition : rightHeldPosition;
            transform.localRotation = (holder.isLeftHand) ? Quaternion.Euler(leftHeldRotation) : Quaternion.Euler(rightHeldRotation);
        }
    }

    public void UpdateOffSetsFromCurrentPos()
    {
        if (isBeingHeld)
        {
            if (holder.isLeftHand)
            {
                leftHeldPosition = transform.localPosition;
                leftHeldRotation = transform.localRotation.eulerAngles;
                if (useMirroredRotations)
                {
                    rightHeldPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                    rightHeldRotation = new Quaternion(transform.localRotation.x, -transform.localRotation.y,
                        -transform.localRotation.z, transform.localRotation.w).eulerAngles;
                }
            }
            else
            {
                rightHeldPosition = transform.localPosition;
                rightHeldRotation = transform.localRotation.eulerAngles;
                if (useMirroredRotations)
                {
                    leftHeldPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                    leftHeldRotation = new Quaternion(transform.localRotation.x, -transform.localRotation.y,
                        -transform.localRotation.z, transform.localRotation.w).eulerAngles;
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

    public void SetAnimOverride(Animator anim)
    {
        if (animOverride != null)
        {
            PersistentAnimator.instance.ChangeAnimRunTime_SmoothTransition(anim, animOverride, this);
        }
    }

    private void GetPicked(VRInteraction interaction)
    {
        interaction.SetManipulatedInteractable(this);

        SetPositionAndRotation();

        rby.useGravity = false;
        rby.isKinematic = true;
    }

    private void GetDropped(Vector3 throwVelocity)
    {
        holder.SetManipulatedInteractable(null);

        rby.useGravity = true;
        rby.isKinematic = false;

        rby.velocity = throwVelocity;
    }

    public override void OnSelected(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            SetAnimOverride(hand.animHand);
        }
    }

    public override void OnDeselected(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.RecoverBaseAnimator();
        }
    }

    public override void OnManipulationStarted(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.animHand.SetBool("Grab", true);
            hand.animHand.SetFloat("Squeeze", squeeze);
        }

        holder = hand;

        transform.parent = holder.modelGrabPoint;
    }

    public override void OnManipulationEnded(VRInteraction caller)
    {
        HandController hand = caller as HandController;
        if (hand != null)
        {
            hand.animHand.SetBool("Grab", false);
        }

        holder = null;

        transform.parent = null;
    }
}
