using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VRViewportSelectManager : VRFrustumSelection {

    [SerializeField] private bool controlRightWand = true;
    [SerializeField] private bool controlLeftWand = true;
    [SerializeField] private VRWand_Controller rightWand;
    [SerializeField] private VRWand_Controller leftWand;
    [SerializeField] [Range(.1f,3f)] private float touchInteractionMaxDist = 1f;

    private InteractionType currInteractionType;

    private HandController_Ray rayInRight;
    private HandController_Ray rayIntLeft;
    private HandController_Touch touchIntRight;
    private HandController_Touch touchIntLeft;
   
    protected override void Start()
    {
        base.Start();
        CashRayAndTouchInteractions();
        SetWandInteraction(InteractionType.Ray);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (currInteractionType != InteractionType.Touch)
        {
            if (InTouchInteractionRange())
            {
                SetWandInteraction(InteractionType.Touch);
            }
        }
        else
        {
            if (currSelectedInteractable != null && !InTouchInteractionRange())
            {
                SetWandInteraction(InteractionType.Ray);
            }
        }
    }

    private void SetWandInteraction(InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.Touch:
                if (controlRightWand)
                {
                    rightWand.SetVRInteraction(touchIntRight);
                }
                if (controlLeftWand)
                {
                    leftWand.SetVRInteraction(touchIntLeft);
                }
                break;
            case InteractionType.Ray:
                if (controlRightWand)
                {
                    rightWand.SetVRInteraction(rayInRight);
                }
                if (controlLeftWand)
                {
                    leftWand.SetVRInteraction(rayIntLeft);
                }
                break;
        }

        currInteractionType = interactionType;
        interactablesInRange.Clear();
    }

    private void CashRayAndTouchInteractions()
    {
        touchIntRight = rightWand.GetComponentInChildren<HandController_Touch>(true);
        touchIntLeft = leftWand.GetComponentInChildren<HandController_Touch>(true);

        rayInRight = rightWand.GetComponentInChildren<HandController_Ray>(true);
        rayIntLeft = leftWand.GetComponentInChildren<HandController_Ray>(true);

        HandController[] allInteractions = new HandController[4] { controlLeftWand?touchIntLeft:null, controlRightWand ? touchIntRight:null, controlRightWand ? rayInRight : null, controlLeftWand ? rayIntLeft:null };
        Array.ForEach(allInteractions, i => { if (i != null) i.enabled = false; });
    }

    private bool InTouchInteractionRange()
    {
        return currSelectedInteractable != null &&
            currSelectedInteractable.GetSquaredInteractionDistance(transform) < Mathf.Pow(touchInteractionMaxDist, 2);
    }
}

public enum InteractionType { Touch = 1, Ray = 2 }
