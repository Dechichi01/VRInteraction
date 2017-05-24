using UnityEngine;
using System.Collections;
using System;

/*public class VRRayInteraction : VRInteraction {

    public LayerMask interactMask;
    [SerializeField]
    private LineRenderer lineRenderer;
    public Transform aimTargetPrefab;

    private Transform aimTargetInstance;
    private bool castRay = true;
    
    void OnEnable()
    {
        SetLineRenderer(lineRenderer);

        aimTargetInstance = Instantiate(aimTargetPrefab) as Transform;
        aimTargetInstance.gameObject.SetActive(false);
    }

    public void ToggleState()
    {
        castRay = !castRay;
        if (lineRenderer)
            lineRenderer.gameObject.SetActive(!lineRenderer.gameObject.activeSelf);
        if (aimTargetInstance)
            aimTargetInstance.gameObject.SetActive(!aimTargetInstance.gameObject.activeSelf);
    }

    public void SetLineRenderer(LineRenderer lr)
    {
        lr.SetWidth(0.002f, 0.002f);
        lineRenderer.gameObject.SetActive(false);
        lr.gameObject.SetActive(true);
        lineRenderer = lr;
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

}*/
