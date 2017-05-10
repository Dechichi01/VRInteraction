using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Pickup : MonoBehaviour
{

    #region Inspector Variables
    public Transform holdPoint;

    public bool useMirroredRotations = true;
    public float grabRange = .2f;
    //degree of squeezing
    public float squeeze;
    #endregion

    [HideInInspector] [SerializeField] protected Vector3 rightHeldPosition;
    [HideInInspector] [SerializeField] protected Vector3 leftHeldPosition;
    [HideInInspector] [SerializeField] protected Vector3 rightHeldRotation;
    [HideInInspector] [SerializeField] protected Vector3 leftHeldRotation;

    [SerializeField] private AnimatorOverrideController animOverride;

    public bool isBeingHeld { get { return holder != null; } }

    protected HandController holder = null;
    protected Rigidbody rby;

    private void Start()
    {
        rby = GetComponent<Rigidbody>();
        holder = null;
    }

    public void OnTriggerPress(HandController holder)
    {
        if (!isBeingHeld && holder.CanGrab(this))
            GetPicked(holder);
    }

    public void OnTriggerRelease(HandController holder)
    {
        return;
        if (this.holder == holder)
            GetDropped(holder.controller.velocity);
    }

    public void OnGripPress(HandController holder)
    {
        if (this.holder == holder)
            GetDropped(holder.controller.velocity);
    }

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

    public void SetAnimOverride(Animator anim)
    {
        if (animOverride != null)
            PersistentAnimator.instance.ChangeAnimRunTime_SmoothTransition(anim, animOverride, this);
    }

    private void GetPicked(HandController holder)
    {
        this.holder = holder;

        holder.SetHoldPickUp(this);

        transform.parent = holder.modelGrabPoint;
        SetPositionAndRotation();

        rby.useGravity = false;
        rby.isKinematic = true;
    }

    private void GetDropped(Vector3 throwVelocity)
    {
        holder.DropHeldPickUp(this);

        holder = null;

        transform.parent = null;

        rby.useGravity = true;
        rby.isKinematic = false;

        rby.velocity = throwVelocity;
    }

}
