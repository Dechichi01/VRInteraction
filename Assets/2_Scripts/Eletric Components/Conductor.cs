using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : VirtualRope {

    private ConductorHead startHead;
    private ConductorHead endHead;

    private EletricConnection sourceConnection;
    private EletricConnection receiverConnection;

    protected override void Start()
    {
        base.Start();
        startHead = start.GetComponent<ConductorHead>();
        endHead = end.GetComponent<ConductorHead>();

        startHead.parent = endHead.parent = this;

        startHead.OnConnectedAddListener(OnHeadConnect);
        startHead.OnDisconnectedAddListener(OnHeadDisconnect);
        endHead.OnConnectedAddListener(OnHeadConnect);
        endHead.OnDisconnectedAddListener(OnHeadDisconnect);
    }

    protected override void Update()
    {
        base.Update();

        if (receiverConnection != null)
        {
            float voltage = 0f;
            float current = 0f;
            if (sourceConnection != null)
            {
                voltage = sourceConnection.voltage;
                current = sourceConnection.current;
            }

            receiverConnection.ReceivePower(voltage, current);
        }
    }

    private void OnDestroy()
    {
        startHead.OnConnectedRemoveListener(OnHeadConnect);
        startHead.OnDisconnectedRemoveListener(OnHeadDisconnect);
        endHead.OnConnectedRemoveListener(OnHeadConnect);
        endHead.OnDisconnectedRemoveListener(OnHeadDisconnect);
    }

    private void OnHeadConnect(EletricConnection elConnection)
    {
        if (elConnection.isOutput)
        {
            sourceConnection = elConnection;
        }
        else
        {
            receiverConnection = elConnection;
        }
    }

    private void OnHeadDisconnect(EletricConnection elConnection)
    {
        if (elConnection.isOutput)
        {
            if (elConnection == sourceConnection)
            {
                sourceConnection = null;
            }
        }
        else if (elConnection == receiverConnection)
        {
            receiverConnection.ReceivePower(0, 0);
            receiverConnection = null;
        }
    }
}
