using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultimeterSelector : Pickup_Touch
{
    [SerializeField] private bool editMode = false;
    private Transform handPrevParent;
    private Vector3 handPrevLocalPos;
    private Quaternion handPrevLocalRot;

    protected override void OnManipulateCallback(VRInteraction caller)
    {
        Vector3 initialPos = _pickupT.position;
        Quaternion initialRot = _pickupT.rotation;

        base.OnManipulateCallback(caller);

        if (!editMode && holder != null)
        {
            _pickupT.SetParent(pickupPrevParent);

            handPrevParent = holder.handParent.parent;
            handPrevLocalPos = holder.handParent.localPosition;
            handPrevLocalRot = holder.handParent.localRotation;

            holder.handParent.SetParent(_pickupT);

            _pickupT.position = initialPos;
            _pickupT.rotation = initialRot;

            StartCoroutine(RotateWithWand(holder.wand.transform));            
        }
    }

    protected override void OnReleaseCallback(VRInteraction caller)
    {
        if (!editMode && holder != null)
        {
            holder.handParent.SetParent(handPrevParent);
            holder.handParent.localPosition = handPrevLocalPos;
            holder.handParent.localRotation = handPrevLocalRot;
        }

        base.OnReleaseCallback(caller);

        _rby.isKinematic = true;
        _rby.useGravity = false;
    }

    public void ReleaseHand()
    {
        GetDropped(Vector3.zero);
    }

    private IEnumerator RotateWithWand(Transform wandTransform)
    {
        Vector3 startRot = _pickupT.rotation.eulerAngles;
        Vector3 wandReferenceRot = wandTransform.rotation.eulerAngles;

        while (holder != null)
        {
            Vector3 currRot = wandTransform.rotation.eulerAngles;
            float z = currRot.z - wandReferenceRot.z;
            _pickupT.rotation = Quaternion.Euler(new Vector3(startRot.x, startRot.y, startRot.z - z));
            yield return null;
        }
    }
}
