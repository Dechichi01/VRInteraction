using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup_Ray : Pickup {

    protected Vector3 referencePos;
    protected HandController_Ray currSelectingCtrl;

    private void Update()
    {
        if (currSelectingCtrl != null)
        {
            float percent = currSelectingCtrl.wand.triggerPressAmount;
            if (percent > 0.1f)
            {
                if (!_rby.isKinematic)
                {
                    _rby.isKinematic = true;
                }
                _pickupT.position = Vector3.Lerp(referencePos, currSelectingCtrl.modelGrabPoint.position, percent);
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
        gameObject.SetLayer(Constants.DefaultLayerNames.Interactable_Ray);
    }

    protected override void OnSelectCallback(VRInteraction caller)
    {
        base.OnSelectCallback(caller);
        HandController_Ray rayCtrl = caller as HandController_Ray;
        if (rayCtrl != null)
        {
            referencePos = _pickupT.position;
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

            _rby.useGravity = true;
            _rby.isKinematic = false;
        }
    }

    protected override void OnManipulateCallback(VRInteraction caller)
    {
        base.OnManipulateCallback(caller);
        currSelectingCtrl = null;
    }
}
