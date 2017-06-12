using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VRViewportSelectManager : VRFrustumSelection {

    private enum InteractionType { Touch = 1, Ray = 2}
    [SerializeField] private VRWand_Controller rightWand;
    [SerializeField] private VRWand_Controller leftWand;

    private HandController_Ray rayIntRight;
    private HandController_Ray rayIntLeft;
    private HandController_Touch touchIntRight;
    private HandController_Touch touchIntLeft;
   
    protected override void Start()
    {
        base.Start();
        CashRayAndTouchInteractions();
        SetWandInteraction(InteractionType.Touch);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Ray");
            SetWandInteraction(InteractionType.Ray);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Touch");
            SetWandInteraction(InteractionType.Touch);
        }
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
}
