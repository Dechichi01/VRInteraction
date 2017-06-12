using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController_Touch : HandController {

    protected override void OnEnable()
    {
        base.OnEnable();
        OnSelectAddListener(SetAnimPrep);
        OnDeselectAddListener(UnsetAnimPrep);
    }

    private void OnDisable()
    {
        OnSelectRemoveListener(SetAnimPrep);
        OnDeselectRemoveListener(UnsetAnimPrep);

        SetSelectedInteractable(null);
        SetManipulatedInteractable(null);
    }

    private void SetAnimPrep(Interactable interactable)
    {
        animHand.SetBool("Prep", true);
    }
    
    private void UnsetAnimPrep(Interactable interactable)
    {
        animHand.SetBool("Prep", false);
    }

    public override void SelectInteractableFromRange()
    {
        float distToInteractable = float.MaxValue;

        if (interactablesInRange.Count == 0)
        {
            SetSelectedInteractable(null);
            return;
        }

        Interactable previousClosest = currSelectedInteractable;
        Interactable currClosest = interactablesInRange[0];

        distToInteractable = currClosest.GetInteractionDistance(interactionPoint);

        foreach (var nearbyInteractable in interactablesInRange)
        {
            float sqrDist = nearbyInteractable.GetSquaredInteractionDistance(interactionPoint);
            if (sqrDist < Mathf.Pow(distToInteractable, 2))
            {
                distToInteractable = Mathf.Pow(sqrDist, 0.5f);
                currClosest = nearbyInteractable;
            }
        }

        if (previousClosest != currClosest)
        {
            SetSelectedInteractable(currClosest);
        }
    }

    void OnTriggerEnter(Collider obj)
    {						
        Interactable interactable = obj.gameObject.GetActiveComponent<Interactable>();

        if (interactable != null && !interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Add(interactable);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        Interactable interactable = obj.gameObject.GetActiveComponent<Interactable>();

        if (interactable != null)
        {
            interactablesInRange.Remove(interactable);
        }
    }
}
