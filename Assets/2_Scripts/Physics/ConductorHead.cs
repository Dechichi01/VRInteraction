using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Pickup))]
public class ConductorHead : MonoBehaviour {

    private bool connected = false;
    [HideInInspector] public bool positivePolarity = false;
    [HideInInspector] public Conductor parent;

    private Pickup pickupSelf;
    private EletricConnection elConnection;

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
        pickupSelf = GetComponent<Pickup>();
    }

    private void OnEnable()
    {
        pickupSelf.OnManipulateAddListener(OnPicked);
        pickupSelf.OnReleaseAddListener(OnReleased);
    }

    private void OnDisable()
    {
        pickupSelf.OnManipulateRemoveListener(OnPicked);
        pickupSelf.OnReleaseRemoveListener(OnReleased);
    }

    private void OnPicked(VRInteraction caller)
    {
        if (connected && OnDisconnected != null)
        {
            OnDisconnected(elConnection);
        }
        connected = false;
        elConnection = null;
    }

    private void OnReleased(VRInteraction caller)
    {
        //It doesn't happen often, so not worthy assigning a layer for collision checking
        RaycastHit[] hits = Physics.BoxCastAll(pickupSelf.pickupT.position, pickupSelf.coll.bounds.extents*1.5f, Vector3.forward, Quaternion.identity, 50);

        if (hits.Length > 0)
        {
            RaycastHit hit = System.Array.Find(hits, h => h.collider.CompareTag("EletricConnection"));
            if (hit.transform != null)
            {
                EletricConnection elConnection = hit.transform.GetComponent<EletricConnection>();
                connected = true;
                this.elConnection = elConnection;

                pickupSelf.rby.isKinematic = true;
                pickupSelf.coll.enabled = false;

                if (OnConnected != null)
                {
                    OnConnected(elConnection);
                }
            }
        }
    }
}
