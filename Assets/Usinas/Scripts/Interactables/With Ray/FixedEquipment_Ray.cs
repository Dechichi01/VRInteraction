using UnityEngine;
using System.Collections;

public class FixedEquipment_Ray : SelectableObject_Ray {

    public Transform arrivalLocation;

    public float moveTowardsTime = 3f;
    // Use this for initialization
    override protected void Start () {
        base.Start();
        //arrivePosition = transform.FindChild("PlayerPosition").position;
	}

    public override void OnTriggerPress(VRInteraction caller, VRWand_Controller wand)
    {
        ChangeToBaseShader();
        if (!canInteract) return;
        Transform player = wand.transform.root;
        float delta = player.position.x - Camera.main.transform.position.x;
        Vector3 arrivalPos = new Vector3(arrivalLocation.position.x, player.position.y, arrivalLocation.position.z) + transform.right * delta;
        StartCoroutine(BringPlayer(player.position, player.rotation, arrivalPos, arrivalLocation.rotation, player));
    }

    public override bool OnTriggerRelease(VRInteraction caller, VRWand_Controller wand)
    {
        return true;                                             
    }

    protected virtual IEnumerator BringPlayer(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, Transform player)
    {
        VRPlayer_Controller playerCtrl = player.GetComponent<VRPlayer_Controller>();
        playerCtrl.canMove = false;
        
        float percent = 0;
        float moveTowardsSpeed = 1 / moveTowardsTime;


        while (percent <= 1)
        {
            percent += (Time.deltaTime * moveTowardsSpeed);
            player.position = Vector3.Lerp(startPos, endPos, percent);
            player.rotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        playerCtrl.canMove = true;
    }
}
