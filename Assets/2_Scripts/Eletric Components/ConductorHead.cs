using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ConductorHead : MonoBehaviour {

    [SerializeField] private EletricConnection backConnection;

    private bool connected = false;
    [HideInInspector] public Conductor parent;

    private Pickup[] pickupSelfs;
    public EletricConnection elConnection { get; private set; }

    private Rigidbody rb;
    private Collider coll;

    private Action<EletricConnection> OnConnected;
    private Action<EletricConnection> OnDisconnected;

    #region Callback
    public void OnConnectedAddListener(Action<EletricConnection> action)
    {
        OnConnected += action;
    }
    public void OnConnectedRemoveListener(Action<EletricConnection> action)
    {
        OnConnected -= action;
    }
    public void OnDisconnectedAddListener(Action<EletricConnection> action)
    {
        OnDisconnected += action;
    }
    public void OnDisconnectedRemoveListener(Action<EletricConnection> action)
    {
        OnDisconnected -= action;
    }
    #endregion

    private void Awake()
    {
        pickupSelfs = GetComponentsInChildren<Pickup>();
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        foreach (var pickupSelf in pickupSelfs)
        {
            pickupSelf.OnDeselectAddListener(OnDeselected);
            pickupSelf.OnManipulateAddListener(OnPicked);
            pickupSelf.OnReleaseAddListener(OnReleased);
        }

        OnConnectedAddListener(OnConnect);
    }

    private void OnDisable()
    {
        foreach (var pickupSelf in pickupSelfs)
        {
            pickupSelf.OnDeselectRemoveListener(OnDeselected);
            pickupSelf.OnManipulateRemoveListener(OnPicked);
            pickupSelf.OnReleaseRemoveListener(OnReleased);
        }

        OnConnectedRemoveListener(OnConnect);
    }

    private void OnPicked(VRInteraction caller)
    {
        coll.enabled = false;

        if (!connected)
        {
            return;
        }

        connected = false;
        elConnection.SetConnector(null);

        SetBackConnection(0, 0);

        if (OnDisconnected != null)
        {
            OnDisconnected(elConnection);
        }

        elConnection = null;
    }

    private void OnReleased(VRInteraction caller)
    {
        //It doesn't happen often, so not worthy assigning a layer for collision checking
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, coll.bounds.extents*1.5f, Vector3.forward, Quaternion.identity, 50).Where(h => h.collider.CompareTag("EletricConnection")).ToArray();

        if (hits.Length > 0)
        {
            EletricConnection elConnection = hits
                .OrderBy(h => (transform.position - h.collider.transform.position).sqrMagnitude)
                .Select(h => h.collider.GetComponent<EletricConnection>())
                .Where(e => e != backConnection).First();

            if (elConnection != null && elConnection.isAvailable)
            {
                connected = true;
                this.elConnection = elConnection;
                elConnection.SetConnector(this);

                SetBackConnection(elConnection.voltage, elConnection.current);

                if (OnConnected != null)
                {
                    OnConnected(elConnection);
                }
            }
        }

        if (!connected)
        {
            coll.enabled = true;
        }

    }

    private void SetBackConnection(float voltage, float current)
    {
        if (backConnection != null)
        {
            backConnection.voltage = voltage;
            backConnection.current = current;
        }
    }

    private void OnConnect(EletricConnection elConnection)
    {
        rb.isKinematic = true;
        coll.enabled = false;
        transform.SetParent(elConnection.transform);
        transform.localPosition = Vector3.zero;
        transform.forward = -elConnection.transform.forward;
    }

    private void OnDeselected(VRInteraction caller)
    {
        if (connected)
        {
            OnConnect(elConnection);
        }
    }

}
