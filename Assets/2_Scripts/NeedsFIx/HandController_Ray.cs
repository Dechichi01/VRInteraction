using UnityEngine;
using System.Collections;
using System;

public class HandController_Ray : HandController {

    [SerializeField] private Transform pointReference;
    [SerializeField] private LineRenderer lineRndr;
    [SerializeField] private Transform walkTarget;

    private Vector3 startLocalPos;
    private Quaternion startLocalRot;

    protected override void Start()
    {
        base.Start();
        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
        InitializeLineRenderer();
    }

    protected override void Update()
    {
        base.Update();
        if (currSelectedInteractable != null)
        {
            Debug.Log(currSelectedInteractable.name);
        }
    }

    public override void SelectInteractableFromRange()
    {
        if (!interactionEnabled)
        {
            return;
        }

        Vector3 direction = lineRndr.transform.forward;

        float length = 50f;
        Vector3 start = lineRndr.transform.position;
        Vector3 end = start + direction * length;

        RaycastHit hit;

        bool bHitInteractable = Physics.Raycast(new Ray(start, direction), out hit, length, interactMask);
        if (bHitInteractable)
        {
            Interactable interactable = hit.collider.transform.GetComponent<Interactable>();
            if (interactable == null)
            {
                Debug.LogError("Collided object in the interact layer isn't an interactable.");
                return;
            }

            SetSelectedInteractable(interactable);

            end = hit.point;

            if (interactable is WalkableGrid)
            {
                walkTarget.position = hit.point;
            }
        }
        else if (currSelectedInteractable != null)//nothing hit
        {
            SetSelectedInteractable(null);
        }

        lineRndr.SetPosition(0, start);
        lineRndr.SetPosition(1, end);
    }

    public void InitializeLineRenderer()
    {
        lineRndr.useWorldSpace = true;
        lineRndr.startWidth = 0.004f;
        lineRndr.endWidth = 0.004f;

        lineRndr.transform.SetParent(pointReference, false);

        lineRndr.transform.localPosition = Vector3.zero;

        lineRndr.gameObject.SetActive(true);
    }

    protected override void EnableInteration()
    {
        base.EnableInteration();
        lineRndr.gameObject.SetActive(true);
    }
    protected override void DisableInteration()
    {
        base.DisableInteration();
        lineRndr.gameObject.SetActive(false);
    }

    protected override bool SelectInteractable(Interactable interactable)
    {
        if (base.SelectInteractable(interactable))
        {
            if (interactable is WalkableGrid)
            {
                walkTarget.gameObject.SetActive(true);
            }
            return true;
        }

        return false;
    }

    protected override bool DeselectInteractable(Interactable interactable)
    {
        if (base.DeselectInteractable(interactable))
        {
            if (interactable is WalkableGrid)
            {
                walkTarget.gameObject.SetActive(true);
            }
            return true;
        }

        return false;
    }
}
