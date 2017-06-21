using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IVRSelectionManager
{
    void OnChangeInteractionAddListener(Action<InteractionType> listener);
    void OnChangeInteractionRemoveListener(Action<InteractionType> listener);

    void SetWandInteraction(InteractionType interactionType);
}
