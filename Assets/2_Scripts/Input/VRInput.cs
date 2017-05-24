using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRInput
{
    public static class Vive
    {
        public static Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
        public static Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
        public static Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
        public static Valve.VR.EVRButtonId trackPadAxis = Valve.VR.EVRButtonId.k_EButton_Axis0;
        public static ulong padButton = SteamVR_Controller.ButtonMask.Touchpad;

        public static float GetTriggerPressAmount(SteamVR_Controller.Device controller)
        {
            return controller.GetAxis(triggerButton).x;
        }

    }
}
