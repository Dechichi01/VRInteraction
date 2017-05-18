using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider))]
public class VRTouchInteraction : VRInteraction
{
    public LayerMask interactMask;
    private BoxCollider bc;

    void Start()
    {
        bc = GetComponent<BoxCollider>();
        bc.isTrigger = true;
    }

    public override bool CheckForInteractables(float radius)
    {
        interactableInrange = null;
        Collider[] colliders = Physics.OverlapBox(bc.bounds.center, bc.bounds.extents, Quaternion.identity, interactMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Interactable interactable = colliders[i].GetComponent<Interactable>();
            if (interactable)
            {
                interactableInrange = interactable;
                break;
            }
        }

        if (interactableInrange)
            Debug.Log(interactableInrange.name);
        return true;
    }

    public override void GripPressed(VRWand_Controller wand)
    {
        throw new NotImplementedException();
    }

    public override void TriggerPressed(VRWand_Controller wand)
    {
        if (interactableInrange != null)
            interactableInrange.OnTriggerPress(this, wand);
    }

    public override void TriggerReleased(VRWand_Controller wand)
    {
        if (interactableInrange != null)
            interactableInrange.OnTriggerRelease(this, wand);
    }
}
