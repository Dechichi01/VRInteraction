using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController_Touch : HandController {

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
            animHand.SetBool("Prep", true);
        }
    }

    void OnTriggerEnter(Collider obj)
    {						
        Pickup pickUp = obj.gameObject.GetComponent<Pickup>();

        if (pickUp != null && !interactablesInRange.Contains(pickUp))
        {
            interactablesInRange.Add(pickUp);
        }
    }

    void OnTriggerExit(Collider obj)
    {                      
        Pickup pickup = obj.gameObject.GetComponent<Pickup>();

        if (pickup != null)
        {
            interactablesInRange.Remove(pickup);
            if (pickup == currSelectedInteractable)
            {
                animHand.SetBool("Prep", false);
                SetSelectedInteractable(null);
            }
        }
    }
}
