using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ConductorHead : MonoBehaviour {

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
    }

    private void OnDisable()
    {
        foreach (var pickupSelf in pickupSelfs)
        {
            pickupSelf.OnDeselectRemoveListener(OnDeselected);
            pickupSelf.OnManipulateRemoveListener(OnPicked);
            pickupSelf.OnReleaseRemoveListener(OnReleased);
        }
    }

    private void OnPicked(VRInteraction caller)
    {
        if (!connected)
        {
            return;
        }

        connected = false;
        elConnection.SetConnector(null);

        if (OnDisconnected != null)
        {
            OnDisconnected(elConnection);
        }

        elConnection = null;
    }

    private void OnReleased(VRInteraction caller)
    {
        //It doesn't happen often, so not worthy assigning a layer for collision checking
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, coll.bounds.extents*1.5f, Vector3.forward, Quaternion.identity, 50);

        if (hits.Length > 0)
        {
            EletricConnection elConnection;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("EletricConnection"))
                {
                    elConnection = hits[i].transform.GetComponent<EletricConnection>();
                    if (elConnection != null && elConnection.isAvailable)
                    {
                        connected = true;
                        this.elConnection = elConnection;
                        elConnection.SetConnector(this);

                        StartCoroutine(Teste());

                        if (OnConnected != null)
                        {
                            OnConnected(elConnection);
                        }
                        break;
                    }
                }
            }
        }
    }

    private void OnDeselected(VRInteraction caller)
    {
        if (connected)
        {
            StartCoroutine(Teste());
        }
    }

    private IEnumerator Teste()
    {
        yield return new WaitForEndOfFrame();

        rb.isKinematic = true;

        transform.SetParent(elConnection.transform);
        transform.localPosition = Vector3.zero;
        transform.forward = -elConnection.transform.forward;
    }
}
