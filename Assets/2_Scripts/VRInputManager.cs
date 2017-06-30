using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRInputManager : MonoBehaviour {

    #region VR Controller variables
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    #endregion

    public Vector3 velocity { get { return controller.velocity; } }
    public float triggerPressAmount { get { return VRInput.Vive.GetTriggerPressAmount(controller); } }

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    public bool GetPressDown(Valve.VR.EVRButtonId button)
    {
        return controller.GetPressDown(button);
    }

    public bool GetPressUp(Valve.VR.EVRButtonId button)
    {
        return controller.GetPressUp(button);
    }

    public bool GetPress(Valve.VR.EVRButtonId button)
    {
        return controller.GetPress(button);
    }

    public bool GetTriggerPressDown(float threshold = .9f)
    {
        return triggerPressAmount > threshold;
    }

    public bool TrackPadPress(TrackPadXAxis xDir, TrackPadYAxis yDir)
    {
        int x = (int)xDir;
        int y = (int)yDir;

        Vector2 input = controller.GetAxis(VRInput.Vive.trackPadAxis);
        Vector2 threshold = new Vector2(x, y) * .3f;

        return controller.GetPress(VRInput.Vive.padButton) && 
            (x == 0 || input.x*x > .3f) && 
            (y == 0 || input.y*y > .3f);
    }
}

public enum TrackPadXAxis { None = 0, Left = -1, Right = 1}
public enum TrackPadYAxis { None = 0, Down = -1, Up = 1 }
