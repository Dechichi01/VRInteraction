using UnityEngine;
using System.Collections;
using System;

public class VRRayInteraction : HandController
{
    [SerializeField]
    private LineRenderer lineRndr;

    private Vector3 startLocalPos;
    private Quaternion startLocalRot;

    private bool castRay = true;
    
    public Vector3 GetLineStartPos() { return lineRndr.GetPosition(0); }
    public Vector3 GetLineEndPos() { return lineRndr.GetPosition(1); }

    protected override void Start()
    {
        base.Start();
        InitializeLineRenderer();

    }

    public void ToggleState()
    {
        castRay = !castRay;
        if (lineRenderer)
        {

        }
            lineRenderer.gameObject.SetActive(!lineRenderer.gameObject.activeSelf);
        if (aimTargetInstance)
            aimTargetInstance.gameObject.SetActive(!aimTargetInstance.gameObject.activeSelf);
    }

    public void InitializeLineRenderer()
    {
        lineRndr.startWidth = 0.002f;
        lineRndr.endWidth = 0.002f;

        lineRndr.gameObject.SetActive(true);
    }

    public LineRenderer GetLineRenderer() { return lineRenderer; }

    public override bool CheckForInteractables(float radius)
    {
        aimTargetInstance.gameObject.SetActive(false);
        if (!castRay) return false;

        if (interactableInrange)
            Debug.Log(interactableInrange.name);

        Vector3 direction = lineRenderer.transform.forward;

        GameObject lineRendererGO = lineRenderer.gameObject;
        if (!lineRendererGO.activeSelf)
            lineRendererGO.SetActive(true);

        Vector3 start = lineRendererGO.transform.position;
        Vector3 end = lineRendererGO.transform.position + direction * radius;
        bool bHitInteractable = Physics.Raycast(new Ray(start, direction), out hit, radius, interactMask);

        if (bHitInteractable)
        {
            Interactable interactable = hit.collider.transform.GetComponent<Interactable>();

            if (interactable != null && interactable != interactableInrange)
            {
                interactable.OnSelected();
                if (interactableInrange != null)
                    interactableInrange.OnDeselect();
            }

            interactableInrange = interactable;//Covers the case of becoming null if not hit an interactable
            end = hit.point;

            if (hit.collider.CompareTag("WalkableGrid"))
            {
                aimTargetInstance.position = hit.point;
                aimTargetInstance.gameObject.SetActive(true);
            }
        }
        else if (interactableInrange != null)//nothing hit
        {
            interactableInrange.OnDeselect();
            interactableInrange = null;
        }

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        return bHitInteractable;
    }

    public override void OnTriggerPress(VRWand_Controller wand)
    {
        if (interactableInrange != null)
            interactableInrange.OnTriggerPress(this, wand);
        else if (hit.collider.CompareTag("WalkableGrid"))
            wand.transform.root.position = new Vector3(hit.point.x, wand.transform.root.position.y, hit.point.z);
    }

    public override void OnTriggerRelease(VRWand_Controller wand)
    {
        if (interactableInrange != null)
            interactableInrange.OnTriggerRelease(this,wand);
    }

    public override void OnGripPress(VRWand_Controller wand)
    {
        ToggleState();
    }

}
