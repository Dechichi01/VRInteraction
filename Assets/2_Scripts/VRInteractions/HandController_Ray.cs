using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(FrustumMesh))]
public class HandController_Ray : HandController {

    [SerializeField] private LineRenderer lineRndr;
    [SerializeField] private Transform walkTarget;

    private MeshCollider meshColl;
    private FrustumMesh frustumMesh;
    
    protected override void Start()
    {
        base.Start();

        InitializeLineRenderer();
        walkTarget.gameObject.SetActive(false);

        meshColl = GetComponent<MeshCollider>();
        frustumMesh = GetComponent<FrustumMesh>();
        frustumMesh.GetMeshRenderer().enabled = false;
        frustumMesh.GenerateMesh();
        meshColl.sharedMesh = frustumMesh.GetMesh();
        meshColl.convex = true;
        meshColl.isTrigger = true;

        frustumMesh.transform.position = lineRndr.transform.position;
        frustumMesh.transform.rotation = lineRndr.transform.rotation;
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

    void OnTriggerEnter(Collider obj)
    {
        Interactable interactable = obj.gameObject.GetActiveComponent<Interactable>();
        if (interactable != null && !interactablesInRange.Contains(interactable))
        {
            interactablesInRange.Add(interactable);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        Interactable interactable = obj.gameObject.GetActiveComponent<Interactable>();
        if (interactable != null)
        {
            interactablesInRange.Remove(interactable);
        }
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

        //Update line renderer
        Vector3 direction = lineRndr.transform.forward;

        Vector3 start = lineRndr.transform.position;
        Vector3 end = start + direction * 50f;
        lineRndr.SetPosition(0, start);
        lineRndr.SetPosition(1, end);
        
        //Select closest
        float distToInteractable = float.MaxValue;

        if (interactablesInRange.Count == 0)
        {
            SetSelectedInteractable(null);
            return;
        }

        Interactable previousClosest = currSelectedInteractable;
        Interactable currClosest = interactablesInRange[0];

        distToInteractable = currClosest.GetInteractionDistance(interactionPoint);
        foreach (var nearbyInteractable in interactablesInRange)
        {
            float sqrDist = nearbyInteractable.GetSquaredInteractionDistance(interactionPoint);
            if (sqrDist < Mathf.Pow(distToInteractable, 2))
            {
                distToInteractable = Mathf.Pow(sqrDist, 0.5f);
                currClosest = nearbyInteractable;
            }
        }

        if (previousClosest != currClosest)
        {
            SetSelectedInteractable(currClosest);
        }

        if (currSelectedInteractable != null)
        {
            lineRndr.SetPosition(1, currSelectedInteractable.GetInteractionPoint().position);
        }
    }

    private void UpdateWalkTarget()
    {
        if (!walkTarget.gameObject.activeSelf)
        {
            return;
        }
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
