using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : VirtualRope {

    private ConductorHead startHead;
    private ConductorHead endHead;

    private float voltage;
    private float current;

    protected override void Start()
    {
        base.Start();
        startHead = start.GetComponent<ConductorHead>();
        endHead = end.GetComponent<ConductorHead>();

        startHead.positivePolarity = true;
        endHead.positivePolarity = false;
        startHead.parent = endHead.parent = this;

        startHead.OnConnectedAddListener(OnPlusConnected);
        startHead.OnDisconnectedAddListener(OnPlusDisconnected);
        endHead.OnConnectedAddListener(OnMinusConnected);
        endHead.OnDisconnectedAddListener(OnMinusDisconnected);
    }

    private void OnDestroy()
    {
        startHead.OnConnectedRemoveListener(OnPlusConnected);
        startHead.OnDisconnectedRemoveListener(OnPlusDisconnected);
        endHead.OnConnectedRemoveListener(OnMinusConnected);
        endHead.OnDisconnectedRemoveListener(OnMinusDisconnected);
    }

    private void OnPlusConnected(EletricConnection elConnection)
    {
        voltage = elConnection.voltage;
        current = elConnection.current;
    }

    private void OnPlusDisconnected(EletricConnection elConnection)
    {
        voltage = current = 0f;
    }

    private void OnMinusConnected(EletricConnection elConnection)
    {
        if (elConnection.isInput)
        {
            elConnection.voltage = voltage;
            elConnection.current = current;
        }
        else
        {
            Debug.Log("Connecdting on output, butn equipement");
        }
    }

    private void OnMinusDisconnected(EletricConnection elConnection)
    {
        elConnection.voltage = elConnection.current = 0f;
    }
}
