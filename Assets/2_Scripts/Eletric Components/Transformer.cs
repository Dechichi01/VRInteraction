using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : EletricEquipment {

    public float primaryToSecondary = .5f;
    private EletricConnection primary;
    private EletricConnection secondary;

    private void Start()
    {
        primary = inputConnections[0];
        secondary = outputConnections[0];
    }

    private void Update()
    {
        secondary.voltage = GetSecondaryVoltage(primary.voltage);
        secondary.current = GetSecondaryCurrent(primary.current);
    }

    private float GetSecondaryVoltage(float primaryVoltage)
    {
        return primaryVoltage * primaryToSecondary;
    }

    private float GetSecondaryCurrent(float primaryCurrent)
    {
        return primaryCurrent * (1f / primaryToSecondary);
    }
}
