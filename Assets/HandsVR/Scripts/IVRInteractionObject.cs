using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVRInteractionObject {

    void OnTriggerPress(VRInteraction caller, VRWand_Controller wand);

    void OnTriggerRelease(VRInteraction caller, VRWand_Controller wand);

    void OnGripPress(VRInteraction caller, VRWand_Controller wand);

    void OnGripRelease(VRInteraction caller, VRWand_Controller wand);

}
