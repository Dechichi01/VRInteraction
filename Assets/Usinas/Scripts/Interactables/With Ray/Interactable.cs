using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider))]
public abstract class Interactable : MonoBehaviour {

    [SerializeField]
    protected bool canInteract;

    private LayerMask previousLayer;

    virtual protected void Start()
    {
        previousLayer = LayerMask.NameToLayer("SelectableObject");
        gameObject.tag = "Interactable";
        canInteract = true;
    }

    abstract public void OnTriggerPress(VRInteraction caller, VRWand_Controller wand);

    abstract public bool OnTriggerRelease(VRInteraction caller, VRWand_Controller wand);

    abstract public void OnSelected();

    abstract public void OnDeselect();

    virtual protected void DisableInteractions()
    {
        canInteract = false;
        previousLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    virtual protected void EnableInteractions()
    {
        canInteract = true;
        gameObject.layer = previousLayer;
    }

}
