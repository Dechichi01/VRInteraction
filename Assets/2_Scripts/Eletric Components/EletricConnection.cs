using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EletricConnection : MonoBehaviour {

    [SerializeField] private bool _isInput = false;
    public bool isAvailable { get; private set; }
    public float voltage = 0f;
    public float current = 0f;

    public bool isInput { get { return _isInput; } }

    private void Start()
    {
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
}
