using UnityEngine;
using System.Collections;
using System;

//[RequireComponent (typeof(BoxCollider))]
public abstract class Interactable : MonoBehaviour, IVRInteractionObject {

    [SerializeField] protected Transform interactionPoint;

    protected LayerMask interactableLayer;
    protected bool canInteract;

    protected virtual void Start()
    {
        canInteract = true;
        EnableInteractions();
        SetDefaultLayer();
    }

    protected virtual void OnEnable()
    {
        SetDefaultLayer();
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
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public virtual bool CanBeManipulated(Transform other)
    {
        return canInteract;
    }

    protected virtual void SetDefaultLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    public abstract void OnSelected(VRInteraction caller);
    public abstract void OnDeselected(VRInteraction caller);
    public abstract void OnManipulationStarted(VRInteraction caller);
    public abstract void OnManipulationEnded(VRInteraction caller);

    public abstract void OnTriggerPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnTriggerRelease(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripRelease(VRInteraction caller, VRWand_Controller wand);

}
