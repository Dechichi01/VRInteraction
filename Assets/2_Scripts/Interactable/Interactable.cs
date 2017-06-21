using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour, IVRInteractionObject {

    [SerializeField] protected Transform interactionPoint;
    public Transform GetInteractionPoint() { return interactionPoint; }

    protected LayerMask interactableLayer;
    protected bool canInteract;
    public bool isSelected { get; private set; }
    public bool isManipulated { get; private set; }

    private Collider coll;

    protected virtual void Start()
    {
        coll = GetComponent<Collider>();
        canInteract = true;
        EnableInteractions();
        SetDefaultLayer();
    }

    protected virtual void OnEnable()
    {
        if (coll != null)
        {
            coll.enabled = true;
        }
        SetDefaultLayer();
    }

    protected virtual void OnDisable()
    {
        if (coll != null)
        {
            coll.enabled = false;
        }
    }

    public float GetInteractionDistance(Transform other)
    {
        return (interactionPoint.position - other.position).magnitude;
    }

    public float GetSquaredInteractionDistance(Transform other)
    {
        return (interactionPoint.position - other.position).sqrMagnitude;
    }

    protected virtual void DisableInteractions()
    {
        gameObject.layer = LayerMask.NameToLayer("NonInteractable");
    }

    protected virtual void EnableInteractions()
    {
        SetDefaultLayer();
    }

    public virtual bool CanBeManipulated(Transform other)
    {
        return canInteract;
    }

    protected virtual void SetDefaultLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public virtual void OnSelected(VRInteraction caller)
    {
        isSelected = true;
    }
    public virtual void OnDeselected(VRInteraction caller)
    {
        isSelected = false;
    }
    public virtual void OnManipulationStarted(VRInteraction caller)
    {
        isManipulated = true;
    }
    public virtual void OnManipulationEnded(VRInteraction caller)
    {
        isManipulated = false;
    }

    public abstract void OnTriggerPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnTriggerRelease(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripRelease(VRInteraction caller, VRWand_Controller wand);

}
