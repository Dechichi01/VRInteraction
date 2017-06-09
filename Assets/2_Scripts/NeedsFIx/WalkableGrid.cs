using UnityEngine;
using System.Collections;
using System;

public class WalkableGrid : Interactable
{
    protected override void SetDefaultLayer()
    {
        base.SetDefaultLayer();
        gameObject.layer = LayerMask.NameToLayer("WalkableGrid");
    }

    public override void OnDeselected(VRInteraction caller)
    {
        
    }

    public override void OnGripPress(VRInteraction caller, VRWand_Controller wand)
    {
        
    }

    public override void OnGripRelease(VRInteraction caller, VRWand_Controller wand)
    {
        
    }

    public override void OnManipulationEnded(VRInteraction caller)
    {
        
    }

    public override void OnManipulationStarted(VRInteraction caller)
    {
        
    }

    public override void OnSelected(VRInteraction caller)
    {
        
    }

    public override void OnTriggerPress(VRInteraction caller, VRWand_Controller wand)
    {
        
    }

    public override void OnTriggerRelease(VRInteraction caller, VRWand_Controller wand)
    {
        
    }
}
	