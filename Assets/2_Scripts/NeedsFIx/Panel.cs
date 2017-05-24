using UnityEngine;
using System.Collections;

/*public class Panel : FixedEquipment_Ray {

    private Animation anim;

    public AnimationClip waitHandAnim;
    public AnimationClip openAnim;
    public AnimationClip closeAnim;

    private bool open = false;

    protected override void Start()
    {
        anim = GetComponent<Animation>();
        base.Start();
    }

    public override void OnTriggerPress(VRInteraction caller, VRWand_Controller wand)
    {
        if (!canInteract) return;

        GetComponent<BoxCollider>().enabled = false;

        if (!open)
        {
            wand.hand.ControllerOff(caller, wand);//hand not holding controller anymore
            string[] animations = new string[2];
            animations[1] = waitHandAnim.name;
            animations[0] = openAnim.name;
            wand.hand.PerformAnimation(transform, anim, animations);
            open = true;
        }
        else
        {
            /*string[] animations = new string[2];
            animations[1] = waitHandAnim.name;
            animations[0] = closeAnim.name;
            wand.hand.PerformAnimation(transform, anim, animations);*//*
            open = false;
            wand.hand.ControllerOn(caller, wand);
        }
    }

    protected override IEnumerator BringPlayer(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, Transform player)
    {
        yield return base.BringPlayer(startPos, startRot, endPos, endRot, player);
        player.GetComponent<VRPlayer_Controller>().canMove = false;
    }

}*/
