using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Ray : Pickup {

    private Vector3 referencePos;
    private HandController_Ray currSelectingCtrl;

    private void Update()
    {
        if (currSelectingCtrl != null)
        {
            float percent = currSelectingCtrl.wand.triggerPressAmount;
            if (percent > 0.1f)
            {
                if (!rby.isKinematic)
                {
                    rby.isKinematic = true;
                }
                tRoot.position = Vector3.Lerp(referencePos, currSelectingCtrl.modelGrabPoint.position, percent);
            }
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetDefaultLayer();
    }

    protected override void SetDefaultLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable_Ray");
    }

    protected override void OnSelectCallback(VRInteraction caller)
    {
        base.OnSelectCallback(caller);
        HandController_Ray rayCtrl = caller as HandController_Ray;
        if (rayCtrl != null)
        {
            referencePos = tRoot.position;
            rayCtrl.SetRenderLine(false);
            currSelectingCtrl = rayCtrl;
        }
    }

    protected override void OnDeselectCallback(VRInteraction caller)
    {
        base.OnDeselectCallback(caller);
        HandController_Ray rayCtrl = caller as HandController_Ray;
        if (rayCtrl != null)
        {
            rayCtrl.SetRenderLine(true);
            currSelectingCtrl = null;

            rby.useGravity = true;
            rby.isKinematic = false;
        }
    }

    protected override void OnManipulationStartCallback(VRInteraction caller)
    {
        base.OnManipulationStartCallback(caller);
        currSelectingCtrl = null;
    }
}
