using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EletricConnection : MonoBehaviour {

    [SerializeField] private ConnectionType type;
    public float voltage = 0f;
    public float current = 0f;

    public bool isAvailable { get; private set; }
    public bool isInput { get { return type == ConnectionType.Input; } }
    public bool isOutput { get { return type == ConnectionType.Output; } }

    private Canvas connectIndicator;

    private void Start()
    {
        SetConnectIndicator(false);
        gameObject.tag = "EletricConnection";
        isAvailable = true;
    }

    public void SetConnector(ConductorHead head)
    {
        if (head != null && !isAvailable)
        {
            return;
        }

        isAvailable = head == null;
    }

    public void ReceivePower(float voltage, float current)
    {
        if (isOutput)
        {
            Debug.Log("Error: Transfering power to an output source");
        }
        else
        {
            this.voltage = voltage;
            this.current = current;
        }
    }

    public void SetConnectIndicator(bool value)
    {
        connectIndicator.gameObject.SetActive(value);
    }

    public enum ConnectionType { Input = -1, Output = 1}
}
