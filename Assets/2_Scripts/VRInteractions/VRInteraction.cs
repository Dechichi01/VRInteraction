using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class VRInteraction : MonoBehaviour {

    protected CallbackList<Interactable> interactablesInRange = new CallbackList<Interactable>();

    [SerializeField] protected Transform interactionPoint;
    public LayerMask interactMask;

    public bool selecitonEnabled { get; private set; }

    protected Interactable currSelectedInteractable { get; private set; }
    protected Interactable currManipulatedInteractable { get; private set; }
    protected Interactable interactable { get { return currManipulatedInteractable ?? currSelectedInteractable; } }

    public InteractionState state
    {
        get
        {
            if (currManipulatedInteractable != null)
            {
                return InteractionState.Manipulating;
            }
            if (currSelectedInteractable != null)
            {
                return InteractionState.Selecting;
            }
            return InteractionState.Idle;
        }
    }

    private Action<Interactable> OnSelect;
    private Action<Interactable> OnDeselect;
    private Action<Interactable> OnManipulate;
    private Action<Interactable> OnRelease;

    public abstract void SelectInteractableFromRange();
    public abstract void OnTriggerPress(VRWand_Controller wand);
    public abstract void OnTriggerRelease(VRWand_Controller wand);
    public abstract void OnGripPress(VRWand_Controller wand);
    public abstract void OnGripRelease(VRWand_Controller wand);

    protected virtual void Start()
    {
        EnableSelection();
    }

    protected virtual void LateUpdate()
    {
        if (selecitonEnabled)
        {
            SelectInteractableFromRange();
        }
    }

    protected virtual void OnEnable()
    {
        SetCollisionRestrictions();
        interactablesInRange.OnAddItemAddListener(RemoveFromListWhenDisabled);
    }

    protected virtual void OnDisable()
    {
        SetManipulatedInteractable(null);
        SetSelectedInteractable(null);
        interactablesInRange.Clear();

        interactablesInRange.OnAddItemRemoveListener(RemoveFromListWhenDisabled);
    }

    protected virtual void DisableSelection()
    {
        selecitonEnabled = false;
    }

    protected virtual void EnableSelection()
    {
        selecitonEnabled = true;
    }

    private void SetCollisionRestrictions()
    {
        int objectLayer = gameObject.layer;
        
        if (interactMask == 0)
        {
            Debug.LogError("Leaving interactMask to 'Default' will cause collision issues. " + Constants.ErrorMsgs.LayerMissing);
            return;
        }

        for (int i = 0; i < 32; i++)
        {
            Physics.IgnoreLayerCollision(objectLayer, i, true);
            if (LayerMaskUtils.LayerInMask(i, interactMask))
            {
                Physics.IgnoreLayerCollision(objectLayer, i, false);
            }
        }
    }

    private void RemoveFromListWhenDisabled(Interactable item)
    {
        item.OnDisableAddListener(i => interactablesInRange.Remove(i));
    }

    #region Callbacks
    public void OnSelectAddListener(Action<Interactable> action)
    {
        OnSelect += action;
    }

    public void OnSelectRemoveListener(Action<Interactable> action)
    {
        OnSelect -= action;
    }

    public void OnDeselectAddListener(Action<Interactable> action)
    {
        OnDeselect += action;
    }

    public void OnDeselectRemoveListener(Action<Interactable> action)
    {
        OnDeselect -= action;
    }

    public void OnManipulateAddListener(Action<Interactable> action)
    {
        OnManipulate += action;
    }

    public void OnManipulateRemoveListener(Action<Interactable> action)
    {
        OnManipulate -= action;
    }

    public void OnReleaseAddListener(Action<Interactable> action)
    {
        OnRelease += action;
    }

    public void OnReleaseRemoveListener(Action<Interactable> action)
    {
        OnRelease -= action;
    }
    #endregion

    #region ObjectSelection
    public void SetSelectedInteractable(Interactable interactable)
    {
        if (interactable != null)
        {
            if (currSelectedInteractable != null)
            {
                if (currSelectedInteractable == interactable)
                {
                    return;
                }
                DeselectInteractable(currSelectedInteractable);
            }
            SelectInteractable(interactable);
        }
        else if (currSelectedInteractable != null)
        {
            DeselectInteractable(currSelectedInteractable);
        }
    }

    private bool SelectInteractable(Interactable interactable)
    {
        if (interactable == null)
        {
            Debug.LogError("The interactable you're trying to select is null. Try using SetSelectedInteractable() instead.");
            return false;
        }

        currSelectedInteractable = interactable;
        interactable.OnSelected(this);
        
        if (OnSelect != null)
        {
            OnSelect(interactable);
        }

        return true;
    }

    private bool DeselectInteractable(Interactable interactable)
    {
        if (interactable == null)
        {
            Debug.LogError("You must send a interactable to be deselected.");
            return false;
        }

        if (interactable != currSelectedInteractable)
        {
            Debug.LogError("The interactable you're trying to deselect hasn't been selected. Current selected interactable is: " +
                currSelectedInteractable == null ? "null" : currSelectedInteractable.name);
            return false;
        }

        currSelectedInteractable = null;

        interactable.OnDeselected(this);

        if (OnDeselect != null)
        {
            OnDeselect(interactable);
        }

        return true;
    }
    #endregion

    #region Object Manipulation
    public bool CanManipulate(Interactable interactable)
    {
        if (interactable == null || currManipulatedInteractable != null)
        {
            return false;
        }

        SelectInteractableFromRange();
        return interactable == currSelectedInteractable && interactable.CanBeManipulated(interactionPoint);
    }

    public void SetManipulatedInteractable(Interactable interactable)
    {
        if (interactable != null)
        {
            if (currManipulatedInteractable != null)
            {
                if (currManipulatedInteractable == interactable)
                {
                    return;
                }
                ReleaseManipulatedInteractable(currManipulatedInteractable);
            }
            ManipulateInteractable(interactable);
        }
        else if (currManipulatedInteractable != null)
        {
            ReleaseManipulatedInteractable(currManipulatedInteractable);
        }
    }

    public void ForceManipulatedInteractable(Interactable interactable)
    {
        interactable.transform.root.position = transform.position;
        SetManipulatedInteractable(interactable);
    }

    private bool ManipulateInteractable(Interactable interactable)
    {
        if (!CanManipulate(interactable))
        {
            return false;
        }

        interactablesInRange.Clear();
        currManipulatedInteractable = interactable;
        DisableSelection();

        interactable.OnManipulated(this);

        if (OnManipulate != null)
        {
            OnManipulate(interactable);
        }

        return true;
    }

    private bool ReleaseManipulatedInteractable(Interactable interactable)
    {
        if (interactable == null || currManipulatedInteractable != interactable)
        {
            Debug.LogError("The interactable you're trying to stop manipulation is not being manipulated. Current manipulated interactable is: " +
                currManipulatedInteractable == null ? "null" : currManipulatedInteractable.name);
            return false;
        }

        EnableSelection();

        currManipulatedInteractable = null;

        interactable.OnReleased(this);

        if (OnRelease != null)
        {
            OnRelease(interactable);
        }

        return true;
    }
    #endregion

}

public enum InteractionState { Idle = 0, Selecting = 1, Manipulating = 2 }
