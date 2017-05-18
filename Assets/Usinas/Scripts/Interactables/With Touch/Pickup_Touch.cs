using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Pickup_Touch : Interactable
{
    Transform initialParent;
    Rigidbody rb;
    bool picked = false;
    public Vector3 pickLocalRotaion { get; private set; }

    protected override void Start()
    {
        base.Start();
        gameObject.layer = LayerMask.NameToLayer("Interactable_Touch");
        initialParent = transform.parent;
        rb = GetComponent<Rigidbody>();
        picked = false;
    }

    public override void OnTriggerPress(VRInteraction caller, VRWand_Controller wand)
    {
        if (canInteract)
        {
            if (transform.parent != wand.transform)
            {
                rb.isKinematic = true;
                transform.parent = wand.transform;
                transform.localPosition = wand.pickupHolder.localPosition;
                transform.localRotation = Quaternion.Euler(pickLocalRotaion);
                picked = true;
            }
        }
    }

    public override bool OnTriggerRelease(VRInteraction caller, VRWand_Controller wand)
    {
        transform.parent = initialParent;
        rb.isKinematic = false;
        rb.velocity = wand.velocity * wand.throwSpeed;
        picked = false;
        return true;
    }

    public void SetLocalRotation(Vector3 newRot)
    {
        if (picked)
        {
            pickLocalRotaion = newRot;
            transform.localRotation = Quaternion.Euler(pickLocalRotaion);
        }
    }

    public override void OnDeselect()
    {

    }

    public override void OnSelected()
    {

    }
}
