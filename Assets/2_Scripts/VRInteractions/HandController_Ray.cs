using UnityEngine;
using System.Collections;
using System;

public class HandController_Ray : HandController {

    [SerializeField] private LineRenderer lineRndr;
    [SerializeField] private Transform walkTarget;

    protected override void Start()
    {
        base.Start();
        InitializeLineRenderer();
        walkTarget.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (wand.triggerPressAmount > 0 && selecitonEnabled)
        {
            DisableSelection();
        }
        else if (wand.triggerPressAmount == 0 && !selecitonEnabled && currManipulatedInteractable == null)
        {
            EnableSelection();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnSelectAddListener(OnSelectInteractable);
        OnDeselectAddListener(OnDeselectInteractable);
        SetRenderLine(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnSelectRemoveListener(OnSelectInteractable);
        OnDeselectRemoveListener(OnDeselectInteractable);

        SetSelectedInteractable(null);
        SetManipulatedInteractable(null);
        SetRenderLine(false);
        walkTarget.gameObject.SetActive(false);
    }

    public override void OnTriggerPress(VRWand_Controller wand)
    {
        base.OnTriggerPress(wand);
        if (walkTarget.gameObject.activeSelf && currSelectedInteractable == null)
        {
            wand.playerVR.RequestMovement(walkTarget.transform.position);
        }
    }
    public override void SelectInteractableFromRange()
    {
        if (!selecitonEnabled)
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
                SetSelectedInteractable(null);
                if (hit.transform.CompareTag("WalkableGrid"))
                {
                    SetWalkTargetPos(hit.point);
                }
                else
                {
                    Debug.LogError(string.Format("Collided object ({0}) in the interact layer isn't an interactable.", hit.collider.name));
                    return;
                }
            }
            else if (interactable != currSelectedInteractable)
            {
                SetSelectedInteractable(interactable);
            }

            end = hit.point;
        }
        else//nothing hit
        {
            SetSelectedInteractable(null);
        }

        lineRndr.SetPosition(0, start);
        lineRndr.SetPosition(1, end);
    }

    public void SetRenderLine(bool value)
    {
        lineRndr.gameObject.SetActive(value);
    }

    private void SetWalkTargetPos(Vector3 pos)
    {
        walkTarget.position = pos;
        if (!walkTarget.gameObject.activeSelf)
        {
            walkTarget.gameObject.SetActive(true);
        }
    }

    public void InitializeLineRenderer()
    {
        lineRndr.useWorldSpace = true;
        lineRndr.startWidth = 0.004f;
        lineRndr.endWidth = 0.004f;

        lineRndr.gameObject.SetActive(true);
    }

    protected override void EnableSelection()
    {
        base.EnableSelection();
        lineRndr.gameObject.SetActive(true);
    }

    protected override void DisableSelection()
    {
        base.DisableSelection();
        lineRndr.gameObject.SetActive(false);
    }

    private void OnSelectInteractable(Interactable interactable)
    {
        walkTarget.gameObject.SetActive(false);
        animHand.SetBool("Prep", true);
    }

    private void OnDeselectInteractable(Interactable interactable)
    {
        animHand.SetBool("Prep", false);
    }
}
