using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(FrustumMesh))]
public class VRFrustumSelection : VRInteraction
{
    [SerializeField] private Image noteSelectionImage;
    [SerializeField] [Range(0,10)] private float disableThresholdDist = 3f;

    private MeshCollider meshColl;
    private FrustumMesh frustumMesh;
    private Canvas imageCanvas;
    private RectTransform canvasRect;

    private VRInteraction[] _otherInteractionsOnParent;
    protected VRInteraction[] otherInteractionsOnParent
    {
        get
        {
            if (_otherInteractionsOnParent == null || _otherInteractionsOnParent.Length == 0)
            {
                CashOtherInteractionsInParent();
            }
            return _otherInteractionsOnParent;
        }
    }

    protected override void Start()
    {
        base.Start();
        meshColl = GetComponent<MeshCollider>();
        imageCanvas = noteSelectionImage.canvas;
        canvasRect = imageCanvas.GetComponent<RectTransform>();

        frustumMesh = GetComponent<FrustumMesh>();
        frustumMesh.GetMeshRenderer().enabled = false;
        frustumMesh.GenerateMesh();
        meshColl.sharedMesh = frustumMesh.GetMesh();
        meshColl.convex = true;
        meshColl.isTrigger = true;

        noteSelectionImage.gameObject.SetActive(false);
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (currSelectedInteractable != null)
        {
            UpdateSelectImagePos();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnSelectAddListener(ShowSelectNote);
        OnDeselectAddListener(HideSelectNote);

        foreach (VRInteraction interaction in otherInteractionsOnParent)
        {
            interaction.OnSelectAddListener(OnOtherInteractionSelect);
            interaction.OnDeselectAddListener(OnOtherInteractionDeselect);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnSelectRemoveListener(ShowSelectNote);
        OnDeselectRemoveListener(HideSelectNote);

        foreach (VRInteraction interaction in otherInteractionsOnParent)
        {
            interaction.OnSelectRemoveListener(OnOtherInteractionSelect);
            interaction.OnDeselectRemoveListener(OnOtherInteractionDeselect);
        }
    }

    private void ShowSelectNote(Interactable interactable)
    {
        UpdateSelectImagePos();
        noteSelectionImage.gameObject.SetActive(true);
    }

    private void HideSelectNote(Interactable interactable)
    {
        noteSelectionImage.gameObject.SetActive(false);
    }

    private void UpdateSelectImagePos()
    {
        noteSelectionImage.rectTransform.anchoredPosition = imageCanvas.WorldToCanvas(canvasRect, interactable.GetInteractionPoint().position);
    }

    private void OnOtherInteractionSelect(Interactable interactable)
    {
        DisableSelection();
    }

    private void OnOtherInteractionDeselect(Interactable interactable)
    {
        SelectInteractableFromRange();
        if (currSelectedInteractable != null && !currSelectedInteractable.isSelected)
        {
            EnableSelection();
        }
    }

    private void CashOtherInteractionsInParent()
    {
        List<VRInteraction> interactionsOnParent = new List<VRInteraction>(transform.root.GetComponentsInChildren<VRInteraction>(true));
        interactionsOnParent.RemoveAll(i => i == this);
        _otherInteractionsOnParent = interactionsOnParent.ToArray();
    }

    public override void SelectInteractableFromRange()
    {
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
            if (distToInteractable < disableThresholdDist)
            {
                HideSelectNote(currClosest);
            }
        }
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

    public override void OnGripPress(VRWand_Controller wand)
    {
        
    }

    public override void OnGripRelease(VRWand_Controller wand)
    {
        
    }

    public override void OnTriggerPress(VRWand_Controller wand)
    {
        
    }

    public override void OnTriggerRelease(VRWand_Controller wand)
    {
        
    }

    protected override void DisableSelection()
    {
        base.DisableSelection();
        SetSelectedInteractable(null);
    }
}
