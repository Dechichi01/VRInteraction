using UnityEngine;
using System.Collections;

public abstract class VRInteraction : MonoBehaviour {

    [HideInInspector]
    public Interactable interactableInrange;
    [HideInInspector]
    public RaycastHit hit;

    public abstract bool CheckForInteractables(float radius);
    public abstract void TriggerPressed(VRWand_Controller wand);
    public abstract void TriggerReleased(VRWand_Controller wand);
    public abstract void GripPressed(VRWand_Controller wand);

}
