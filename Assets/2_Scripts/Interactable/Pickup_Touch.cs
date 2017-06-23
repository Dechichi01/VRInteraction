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
        gameObject.SetLayer(Constants.DefaultLayerNames.Interactable_Touch);
    }

    public override bool CanBeManipulated(Transform other)
    {
        return base.CanBeManipulated(other) && GetInteractionDistance(other) < grabRange;
    }
}
