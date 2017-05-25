using UnityEngine;
using System.Collections;
using System;

public class HandController_Ray : HandController {

    public LineRenderer lineRenderer;
    public Transform pickupHolder;
    private LineRenderer previousLineRenderer;
    private Transform previousPickupHolder;

    public AnimationClip controllerOnAnim;
    public AnimationClip controllerOffAnim;

    private Vector3 startLocalPos;
    private Quaternion startLocalRot;

    protected override void Start()
    {
        base.Start();
        startLocalPos = transform.localPosition;
        startLocalRot = transform.localRotation;
    }
	
    public void ControllerOn(VRInteraction interaction, VRWand_Controller wand)
    {
        wand.transform.FindChild("Model").gameObject.SetActive(true);
        VRRayInteraction rayInteract = (VRRayInteraction)interaction;
        if (!rayInteract)
        {
            Debug.LogWarning("No RayInteraction passed to toggle raycast!");
            return;
        }
        if (previousLineRenderer != null)
            rayInteract.SetLineRenderer(previousLineRenderer);

        animHand.Play(controllerOnAnim.name);
        startLocalPos = transform.localPosition;
    }

    public void ControllerOff(VRInteraction interaction, VRWand_Controller wand)
    {
        wand.transform.FindChild("Model").gameObject.SetActive(false);

        VRRayInteraction rayInteract = (VRRayInteraction)interaction;
        if (!rayInteract)
        {
            Debug.LogWarning("No RayInteraction passed to toggle raycast!");
            return;
        }

        previousLineRenderer = rayInteract.GetLineRenderer();
        rayInteract.SetLineRenderer(lineRenderer);
        animHand.Play(controllerOffAnim.name);
    }

    public void PerformAnimation(Transform panel, Animation componentAnim, string[] animNames, bool isVolumeCtrl = false)
    {
        StartCoroutine(Positionate_HandPanel(panel, componentAnim, animNames, isVolumeCtrl));
    }

    public void OpenPanelDoor(Transform panel, Animation componentAnim, string[] animNames)
    {
        StartCoroutine(Positionate_HandPanel(panel, componentAnim, animNames));
    }

    private string ChangeToControllerAnim(string s1)
    {
        string s2 = s1.Remove(0, 1);
        s2 = s2.Insert(0, "h");
        return s2;
    }

    public void PlayAnimation(string s1)
    {
        animHand.Play(ChangeToControllerAnim(s1));
    }

    //Used for iteractions that involves animation
    public IEnumerator Positionate_HandPanel(Transform panel, Animation componentAnim, string[] animNames, bool isVolCtrl = false)
    {
        //Move a mão até o painel
        Transform parent = transform.parent;
        transform.parent = null;

        Vector3 start = transform.position;
        Vector3 end = panel.position;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = panel.rotation;

        float percent = 0;
        float speed = 1 / 0.5f;

        //start playing animations
        string turnOnAnimName = animNames[0];
        string animation = ChangeToControllerAnim(turnOnAnimName);

        //Play the anim direct or reversed
        if (animNames[1] == "reversed")
        {
            componentAnim[turnOnAnimName].speed = -1.0f;
            componentAnim[turnOnAnimName].time = componentAnim[turnOnAnimName].length;

            animHand[animation].speed = -1.0f;
            animHand[animation].time = animHand[animation].length;
        }
        else
        {
            componentAnim[turnOnAnimName].speed = 1.0f;
            componentAnim[turnOnAnimName].time = 0;

            animHand[animation].speed = 1.0f;
            animHand[animation].time = 0;
        }

        componentAnim.Play(turnOnAnimName);
        animHand.Play(animation);

        while (percent<1)
        {
            percent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, end, percent);
            transform.rotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        transform.position = end;

        //Wait for animation to finish
        while (animHand.IsPlaying(animation)) yield return null;

        //Debug.Log(isVolCtrl);
        if (!isVolCtrl) StartCoroutine(Positionate_PanelHand(parent, componentAnim, animNames, isVolCtrl));
        else StartCoroutine(PerformVolumeCtrl(parent, componentAnim, animNames, isVolCtrl));
        
    }

    public IEnumerator Positionate_PanelHand(Transform parent, Animation componentAnim, string[] animNames, bool isVolCtrl = false)
    {
        //Retorna a mão à posição inicial
        transform.parent = parent;

        if (isVolCtrl)
        {
            string releaseAnim = animNames[2];
            string releaseAnim_h = ChangeToControllerAnim(releaseAnim);
            componentAnim.Play(releaseAnim);
            animHand.Play(releaseAnim_h);
        }

        Vector3 start = transform.localPosition;
        Vector3 end = startLocalPos;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot = startLocalRot;

        float percent = 0;
        float speed = 1 / 0.5f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, end, percent);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        transform.localPosition = end;
        transform.localRotation = endRot;

        TwoStateInteractable_Ray obj = componentAnim.transform.GetComponent<TwoStateInteractable_Ray>();
        if (obj !=null) obj.ToggleLineRenderer();
    }

    IEnumerator PerformVolumeCtrl(Transform parent, Animation componentAnim, string[] animNames, bool isVolCtrl = false)
    {
        int sign = (animNames[3] == "reversed")?-1:1;

        float initialRotZ = sign*Mathf.Abs(parent.localRotation.z);
        float finalRotZ = initialRotZ + sign*0.5f;

        string rotAnim = animNames[1];
        string rotAnim_h = ChangeToControllerAnim(rotAnim);

        float rot = sign*Mathf.Abs((parent.localRotation * Quaternion.Euler(0f, 0f, -initialRotZ)).z); ;

        float animTime = 0f;
        while (animTime != animHand[rotAnim_h].length)
        {
            //Lock CCW rotations and prevents animations jump due to rotation sign change 
            if (sign*parent.localRotation.z <= 0 && !(sign*rot > 0.7f && animTime < 0.1f*animHand[rotAnim_h].length)) 
                animTime = Mathf.InverseLerp(initialRotZ, finalRotZ, rot) * animHand[rotAnim_h].length;

            componentAnim[rotAnim].speed = 0;
            componentAnim.Play(rotAnim);
            componentAnim[rotAnim].time = (sign ==1)?animTime:(1-animTime);

            animHand[rotAnim_h].speed = 0;
            animHand.Play(rotAnim_h);
            animHand[rotAnim_h].time = (sign == 1) ? animTime : (1 - animTime);

            yield return null;

            rot = sign*Mathf.Abs((parent.localRotation*Quaternion.Euler(0f, 0f, -initialRotZ)).z);
        }

        StartCoroutine(Positionate_PanelHand(parent, componentAnim, animNames, isVolCtrl));
    }

    //Used for iteractions that involves animation
    public IEnumerator OpenDoor(Transform panel, Animation componentAnim, string[] animNames)
    {
        //Move a mão até o painel
        Transform parent = transform.parent;
        transform.parent = null;

        Vector3 start = transform.position;
        Vector3 end = panel.position;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = panel.rotation;

        float percent = 0;
        float speed = 1 / 0.5f;

        //start playing animations
        string handAnim = animNames[0];
        string openAnim = animNames[1];
        string animation = ChangeToControllerAnim(handAnim);

        componentAnim.Play(handAnim);
        animHand.Play(animation);
        Debug.Log("APARECdfasdfasfsadfasdfdsadfsE");
        Debug.LogWarning("askjsakjsakjsajkaskj");
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, end, percent);
            transform.rotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        transform.position = end;

        start = transform.localPosition;
        end = startLocalPos;

        startRot = transform.localRotation;
        endRot = startLocalRot;

        percent = 0;
        speed = 1 / 0.5f;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            transform.localPosition = Vector3.Lerp(start, end, percent);
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);
            yield return null;
        }

        transform.localPosition = end;
        transform.localRotation = endRot;
    }

    public override void SelectInteractableFromRange()
    {
        throw new NotImplementedException();
    }
}
