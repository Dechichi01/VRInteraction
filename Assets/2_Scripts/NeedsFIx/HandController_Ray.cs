using UnityEngine;
using System.Collections;
using System;

public class HandController_Ray : HandController {

    [SerializeField] private Transform pointReference;
    [SerializeField] private LineRenderer lineRndr;
    [SerializeField] private Transform walkTarget;

    protected override void Start()
    {
        base.Start();
        InitializeLineRenderer();
    }

    protected override void Update()
    {
        base.Update();

        if (wand.triggerPressAmount > 0 && interactionEnabled)
        {
            DisableInteration();
        }
        else if (wand.triggerPressAmount == 0 && !interactionEnabled && currManipulatedInteractable == null)
        {
            EnableInteration();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnSelectAddListener(OnSelectInteractable);
        OnDeselectAddListener(OnDeselectInteractable);
        SetRenderLine(true);
    }

    private void OnDisable()
    {
        OnSelectRemoveListener(OnSelectInteractable);
        OnDeselectRemoveListener(OnDeselectInteractable);

        SetSelectedInteractable(null);
        SetManipulatedInteractable(null);
        SetRenderLine(false);
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
            Interactable interactable = hit.collider.transform.GetActiveComponent<Interactable>();
            if (interactable == null)
            {
                Debug.LogError("Collided object in the interact layer isn't an interactable.");
                return;
            }
            
            SetSelectedInteractable(interactable);
            animHand.SetBool("Prep", true);

            end = hit.point;

            if (interactable is WalkableGrid)
            {
                walkTarget.position = hit.point;
            }
        }
        else if (currSelectedInteractable != null)//nothing hit
        {
            SetSelectedInteractable(null);
            animHand.SetBool("Prep", false);
        }

        lineRndr.SetPosition(0, start);
        lineRndr.SetPosition(1, end);
    }

    public void SetRenderLine(bool value)
    {
        lineRndr.gameObject.SetActive(value);
    }

    public void InitializeLineRenderer()
    {
        lineRndr.useWorldSpace = true;
        lineRndr.startWidth = 0.004f;
        lineRndr.endWidth = 0.004f;

        Transform previousParent = lineRndr.transform.parent;
        lineRndr.transform.SetParent(pointReference, false);

        lineRndr.transform.localPosition = Vector3.zero;

        lineRndr.transform.parent = previousParent;

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

    private void OnSelectInteractable(Interactable interactable)
    {
        if (interactable is WalkableGrid)
        {
            walkTarget.gameObject.SetActive(true);
        }
    }

    private void OnDeselectInteractable(Interactable interactable)
    {
        if (interactable is WalkableGrid)
        {
            walkTarget.gameObject.SetActive(true);
        }
    }
}
