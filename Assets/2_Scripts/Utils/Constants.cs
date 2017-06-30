using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static class DefaultLayerNames
    {
        public const string VRInteraction = "VRInteraction";
        public const string Interactable = "Interactable";
        public const string Interactable_Touch = "Interactable_Touch";
        public const string Interactable_Ray = "Interactable_Ray";
    }

    public static class ErrorMsgs
    {
        public const string LayerMissing = "Please check documentation and create system's required layers.";
    }
}
