using UnityEngine;
using System.Collections.Generic;

public abstract class VRInteraction : MonoBehaviour {

    protected List<Interactable> interactablesInRange = new List<Interactable>();

    [SerializeField] protected Transform interactionPoint;

    protected bool interactionEnabled = true;

    protected Interactable currSelectedInteractable { get; private set; }
    protected Interactable currManipulatedInteractable { get; private set; }
    protected Interactable interactable { get { return currManipulatedInteractable ?? currSelectedInteractable; } }

    public abstract void SelectInteractableFromRange();
    public abstract void OnTriggerPress(VRWand_Controller wand);
    public abstract void OnTriggerRelease(VRWand_Controller wand);
    public abstract void OnGripPress(VRWand_Controller wand);
    public abstract void OnGripRelease(VRWand_Controller wand);

    protected virtual void Start()
    {
        EnableInteration();
    }

    protected virtual void LateUpdate()
    {
        if (interactionEnabled)
        {
            SelectInteractableFromRange();
        }
    }

    protected virtual void DisableInteration()
    {
        interactionEnabled = false;
    }

    protected virtual void EnableInteration()
    {
        interactionEnabled = true;
    }

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

    protected virtual bool SelectInteractable(Interactable interactable)
    {
        if (interactable == null)
        {
            Debug.LogError("The interactable you're trying to select is null. Try using SetSelectedInteractable() instead.");
            return false;
        }

        currSelectedInteractable = interactable;
        interactable.OnSelected(this);

        return true;
    }

    protected virtual bool DeselectInteractable(Interactable interactable)
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

    protected virtual bool ManipulateInteractable(Interactable interactable)
    {
        if (!CanManipulate(interactable))
        {
            return false;
        }

        currManipulatedInteractable = interactable;
        DisableInteration();

        interactable.OnManipulationStarted(this);

        return true;
    }

    protected virtual bool ReleaseManipulatedInteractable(Interactable interactable)
    {
        if (interactable == null || currManipulatedInteractable != interactable)
        {
            Debug.LogError("The interactable you're trying to stop manipulation is not being manipulated. Current manipulated interactable is: " +
                currManipulatedInteractable == null ? "null" : currManipulatedInteractable.name);
            return false;
        }

        EnableInteration();

        currManipulatedInteractable = null;

        interactable.OnManipulationEnded(this);

        return true;
    }
    #endregion
}
