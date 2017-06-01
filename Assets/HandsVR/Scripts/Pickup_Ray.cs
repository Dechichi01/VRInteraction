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
                transform.position = Vector3.Lerp(referencePos, currSelectingCtrl.modelGrabPoint.position, percent);
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

    public override void OnSelected(VRInteraction caller)
    {
        base.OnSelected(caller);
        HandController_Ray rayCtrl = caller as HandController_Ray;
        if (rayCtrl != null)
        {
            referencePos = transform.position;
            rayCtrl.SetRenderLine(false);
            currSelectingCtrl = rayCtrl;
        }
    }

    public override void OnDeselected(VRInteraction caller)
    {
        base.OnDeselected(caller);
        HandController_Ray rayCtrl = caller as HandController_Ray;
        if (rayCtrl != null)
        {
            rayCtrl.SetRenderLine(true);
            currSelectingCtrl = null;

            rby.useGravity = true;
            rby.isKinematic = false;
        }
    }

    public override void OnManipulationStarted(VRInteraction caller)
    {
        base.OnManipulationStarted(caller);
        rby.useGravity = false;
        rby.isKinematic = true;
        currSelectingCtrl = null;
    }

    protected override void GetPicked(VRInteraction interaction)
    {
        interaction.SetManipulatedInteractable(this);

        SetPositionAndRotation();

        rby.useGravity = false;
        rby.isKinematic = true;
    }

    protected override void GetDropped(Vector3 throwVelocity)
    {
        holder.SetManipulatedInteractable(null);

        rby.useGravity = true;
        rby.isKinematic = false;

        rby.velocity = throwVelocity;
    }

}
