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

    private Action<VRInteraction> OnSelect;
    private Action<VRInteraction> OnDeselect;
    private Action<VRInteraction> OnManipulationStart;
    private Action<VRInteraction> OnManipulationEnd;

    #region Callbacks
    public void OnSelectAddListener(Action<VRInteraction> action)
    {
        OnSelect += action;
    }

    public void OnSelectRemoveListener(Action<VRInteraction> action)
    {
        OnSelect -= action;
    }

    public void OnDeselectAddListener(Action<VRInteraction> action)
    {
        OnDeselect += action;
    }

    public void OnDeselectRemoveListener(Action<VRInteraction> action)
    {
        OnDeselect -= action;
    }

    public void OnManipulationStartAddListener(Action<VRInteraction> action)
    {
        OnManipulationStart += action;
    }

    public void OnManipulationStartRemoveListener(Action<VRInteraction> action)
    {
        OnManipulationStart -= action;
    }

    public void OnManipulationEndAddListener(Action<VRInteraction> action)
    {
        OnManipulationEnd += action;
    }

    public void OnManipulationEndRemoveListener(Action<VRInteraction> action)
    {
        OnManipulationEnd -= action;
    }
    #endregion

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

    public void OnSelected(VRInteraction caller)
    {
        isSelected = true;
        if (OnSelect != null)
        {
            OnSelect(caller);
        }
    }
    public void OnDeselected(VRInteraction caller)
    {
        isSelected = false;
        if (OnDeselect != null)
        {
            OnDeselect(caller);
        }
    }
    public void OnManipulationStarted(VRInteraction caller)
    {
        isManipulated = true;
        if (OnManipulationStart != null)
        {
            OnManipulationStart(caller);
        }
    }
    public void OnManipulationEnded(VRInteraction caller)
    {
        isManipulated = false;
        if (OnManipulationEnd != null)
        {
            OnManipulationEnd(caller);
        }
    }

    public abstract void OnTriggerPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnTriggerRelease(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripRelease(VRInteraction caller, VRWand_Controller wand);

}
