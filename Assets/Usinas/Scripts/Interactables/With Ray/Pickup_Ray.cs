using UnityEngine;
using System.Collections;

public class Pickup_Ray : SelectableObject_Ray {

    Transform initialParent;    
    Vector3 initialPosition;
    public float approximationTime = 1.6f;
    public float moveAwayTime = 1.2f;

    private float initialScale;
    public float onHandsScale = 0.1f;

    [HideInInspector]
    public bool onHand = false;

    override protected void Start()
    {
        base.Start();
        initialPosition = transform.position;
        initialParent = transform.parent;

        initialScale = transform.localScale.x;
    }

    public override void OnTriggerPress(VRInteraction caller, VRWand_Controller wand)
    {
        if (canInteract)
        {
            if (transform.parent != wand.transform)
            {
                transform.parent = wand.transform;
                wand.childPickup = this;
                Approximate(wand);
            }
            else
            {
                transform.parent = initialParent;
                wand.childPickup = null;
                ChangeToBaseShader();
                MoveAway(wand);
            }
        }
    }

    public override bool OnTriggerRelease(VRInteraction caller, VRWand_Controller wand)
    {
        return true;
    }

    public void Approximate(VRWand_Controller wand)
    {
        if (canInteract && !onHand)
        {
            ChangeToBaseShader();
            StartCoroutine(PerformApproximate(wand));
        }
    }

    public void MoveAway(VRWand_Controller wand)
    {
        if (canInteract && onHand)
            StartCoroutine(PerformMoveAway(wand));
    }

    IEnumerator PerformApproximate(VRWand_Controller wand)
    {
        wand.ToggleLineRenderer();
        Transform targetChild = wand.pickupHolder;

        canInteract = false;

        float moveVelocity = 1 / approximationTime;
        float percent = 0;
        Vector3 endScale = transform.localScale * onHandsScale;

        while (percent < 1)
        {
            percent += Time.deltaTime * moveVelocity;
            transform.position = Vector3.Lerp(transform.position, targetChild.position, percent);
            transform.localScale = Vector3.Lerp(transform.localScale, endScale, percent);
            yield return null;
        }
        transform.position = targetChild.position;
        transform.localScale = endScale;

        onHand = true;
        canInteract = true;

    }

    IEnumerator PerformMoveAway(VRWand_Controller wand)
    {
        canInteract = false;
        onHand = false;

        float moveVelocity = 1 / moveAwayTime;
        float percent = 0;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = initialPosition;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = startScale / onHandsScale;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.identity;
        while (percent <1)
        {
            percent += Time.deltaTime * moveVelocity;
            transform.position = Vector3.Lerp(startPosition, endPosition, percent);
            transform.localScale = Vector3.Lerp(startScale, endScale, percent);
            transform.rotation = Quaternion.Slerp(startRot, endRot, percent);
            yield return null;
        }
        transform.position = endPosition;
        transform.rotation = endRot;
        transform.localScale = endScale;
        canInteract = true;

        wand.ToggleLineRenderer();
    }

}
