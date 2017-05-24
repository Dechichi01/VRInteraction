using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*public class PanelController : MonoBehaviour {

    public List<Interactable> panelComponents;

    public Dictionary<string, Interactable> panelComponentsDict = new Dictionary<string, Interactable>();

    void Awake()
    {
        foreach (Interactable component in panelComponents)
            panelComponentsDict.Add(component.name, component);

        //GetStateBtn(panelComponentsDict["btn_tensao3"]);
        //SetStateBtn(panelComponentsDict["btn_tensao3"], false);

        if (GetStateBtn(panelComponentsDict["btn_tensao3"]) == false)
        {
            //Debug.Log("Ok");
        }
        
    }

	public void ChangeState(string name, bool state)
    {
        TwoStateInteractable_Ray obj = (TwoStateInteractable_Ray)panelComponentsDict[name];

        if (obj != null) SetStateBtn(panelComponentsDict[name], state);

    }

    public bool GetStateBtn(Interactable interactable)
    {
        TwoStateInteractable_Ray obj = (TwoStateInteractable_Ray)interactable;

        if (obj != null) return obj.turnedOn;

        return false;
    }

    public void SetStateBtn(Interactable interactable, bool state)
    {
        TwoStateInteractable_Ray obj = (TwoStateInteractable_Ray)interactable;

        if (obj != null) obj.turnedOn = state;
    }

    public bool GetStateVolume(Interactable interactable)
    {
        VolumeController_Ray obj = (VolumeController_Ray)interactable;

        if (obj != null) return obj.turnedOn;

        return false;
    }

    public void SetStateVolume(Interactable interactable, bool state)
    {
        VolumeController_Ray obj = (VolumeController_Ray)interactable;

        if (obj != null) obj.turnedOn = state;
    }
}*/
