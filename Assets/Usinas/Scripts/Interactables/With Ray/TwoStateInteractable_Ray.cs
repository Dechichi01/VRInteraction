using UnityEngine;
using System.Collections;
using System;

/*public class TwoStateInteractable_Ray : SelectableObject_Ray {

    protected Animation anim;
    public AnimationClip turnOnAnim;
    public AnimationClip turnOffAnim;
    private PanelController panelCtrl;

    [SerializeField]
    protected HandController_Ray rightHand;

    [SerializeField]
    protected VRWand_Controller wand;

    //[HideInInspector]
    public bool turnedOn = false;

    protected override void Start()
    {
        base.Start();
        //anim = transform.root.GetComponent<Animation>();
        anim = GetComponent<Animation>();
        panelCtrl = FindObjectOfType<PanelController>();
    }

    public override void OnTriggerPress(VRInteraction caller, VRWand_Controller wand)
    {
        wand.ToggleLineRenderer();
        if (turnedOn)
        {
            turnedOn = false;
            panelCtrl.ChangeState(this.name, turnedOn);
            string[] animNames = new string[2];
            animNames[0] = turnOffAnim.name;
            animNames[1] = "reversed";
            rightHand.PerformAnimation(transform.parent, anim, animNames);
        }
        else
        {
            turnedOn = true;
            panelCtrl.ChangeState(this.name, turnedOn);
            string[] animNames = new string[2];
            animNames[0] = turnOnAnim.name;
            animNames[1] = "direct";
            rightHand.PerformAnimation(transform.parent, anim, animNames);
        }
    }

    public override bool OnTriggerRelease(VRInteraction caller, VRWand_Controller wand)
    {
        return true;
    }

    public void ToggleLineRenderer()
    {
        wand.ToggleLineRenderer();
    }
	
}*/
