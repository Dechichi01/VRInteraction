using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VRViewportSelectManager : VRFrustumSelection {

    private enum InteractionType { Touch = 1, Ray = 2}
    [SerializeField] private VRWand_Controller rightWand;
    [SerializeField] private VRWand_Controller leftWand;
    [SerializeField] [Range(.1f,3f)] private float touchInteractionMaxDist = 1f;

    private InteractionType currInteractionType;

    private HandController_Ray rayIntRight;
    private HandController_Ray rayIntLeft;
    private HandController_Touch touchIntRight;
    private HandController_Touch touchIntLeft;
   
    protected override void Start()
    {
        base.Start();
        CashRayAndTouchInteractions();
        SetWandInteraction(InteractionType.Ray);
        StartCoroutine(UpdateCurrentInteraction());
    }

    private void SetWandInteraction(InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.Touch:
                rightWand.SetVRInteraction(touchIntRight);
                leftWand.SetVRInteraction(touchIntLeft);
                break;
            case InteractionType.Ray:
                rightWand.SetVRInteraction(rayIntRight);
                leftWand.SetVRInteraction(rayIntLeft);
                break;
        }

        currInteractionType = interactionType;
    }

    private void CashRayAndTouchInteractions()
    {
        touchIntRight = rightWand.GetComponentInChildren<HandController_Touch>(true);
        touchIntLeft = leftWand.GetComponentInChildren<HandController_Touch>(true);

        rayIntRight = rightWand.GetComponentInChildren<HandController_Ray>(true);
        rayIntLeft = leftWand.GetComponentInChildren<HandController_Ray>(true);

        HandController[] allInteractions = new HandController[4] { touchIntLeft, touchIntRight, rayIntRight, rayIntLeft };
        Array.ForEach(allInteractions, i => { if (i != null) i.enabled = false; });
    }

    private bool InTouchInteractionRange()
    {
        return currSelectedInteractable != null &&
            currSelectedInteractable.GetSquaredInteractionDistance(transform) < Mathf.Pow(touchInteractionMaxDist, 2);
    }

    private IEnumerator UpdateCurrentInteraction()
    {
        while(true)
        {
            if (currInteractionType != InteractionType.Touch)
            {
                if (InTouchInteractionRange())
                {
                    Debug.Log("changing to touch");
                    SetWandInteraction(InteractionType.Touch);
                }
            }
            else
            {
                if (currSelectedInteractable != null && !InTouchInteractionRange())
                {
                    Debug.Log("Changing to ray");
                    SetWandInteraction(InteractionType.Ray);
                }
            }

            yield return new WaitForSeconds(1);
        }
    }
}
