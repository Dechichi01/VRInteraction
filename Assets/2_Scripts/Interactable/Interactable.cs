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

    private Collider _coll;
    public Collider coll { get { return _coll; } }

    private Action<VRInteraction> OnSelect;
    private Action<VRInteraction> OnDeselect;
    private Action<VRInteraction> OnManipulate;
    private Action<VRInteraction> OnRelease;

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

    public void OnManipulateAddListener(Action<VRInteraction> action)
    {
        OnManipulate += action;
    }

    public void OnManipulateRemoveListener(Action<VRInteraction> action)
    {
        OnManipulate -= action;
    }

    public void OnReleaseAddListener(Action<VRInteraction> action)
    {
        OnRelease += action;
    }

    public void OnReleaseRemoveListener(Action<VRInteraction> action)
    {
        OnRelease -= action;
    }
    #endregion

    protected virtual void Start()
    {
        _coll = GetComponent<Collider>();
        canInteract = true;
        EnableInteractions();
        SetDefaultLayer();
    }

    protected virtual void OnEnable()
    {
        if (_coll != null)
        {
            _coll.enabled = true;
        }
        SetDefaultLayer();
    }

    protected virtual void OnDisable()
    {
        if (_coll != null)
        {
            _coll.enabled = false;
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
        gameObject.SetLayer(Constants.DefaultLayerNames.Interactable);
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
    public void OnManipulated(VRInteraction caller)
    {
        isManipulated = true;
        if (OnManipulate != null)
        {
            OnManipulate(caller);
        }
    }
    public void OnReleased(VRInteraction caller)
    {
        isManipulated = false;
        if (OnRelease != null)
        {
            OnRelease(caller);
        }
    }

    public abstract void OnTriggerPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnTriggerRelease(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripPress(VRInteraction caller, VRWand_Controller wand);

    public abstract void OnGripRelease(VRInteraction caller, VRWand_Controller wand);

}
