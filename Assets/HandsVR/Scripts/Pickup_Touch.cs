using UnityEngine;
using System.Collections;
using System;

public class Pickup_Touch : Pickup
{
    #region Inspector Variables
    [SerializeField] protected float grabRange = .2f;
    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();
        SetDefaultLayer();
    }

    protected override void SetDefaultLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable_Touch");
    }

    public override bool CanBeManipulated(Transform other)
    {
        return base.CanBeManipulated(other) && GetInteractionDistance(other) < grabRange;
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
