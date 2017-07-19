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

    private List<EletricConnection> nearbyConnections = new List<EletricConnection>();
    private EletricConnection closestConnection = null;

    private Action<EletricConnection> OnConnected;
    private Action<EletricConnection> OnDisconnected;

    private Coroutine lookForClosestCoR;

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

    private void Update()
    {
        Debug.Log(nearbyConnections.Count);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EletricConnection"))
        {
            EletricConnection elconnection = other.GetComponent<EletricConnection>();
            if (elconnection != null && elconnection != backConnection && !nearbyConnections.Contains(elconnection))
            {
                nearbyConnections.Add(elconnection);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EletricConnection"))
        {
            EletricConnection elconnection = other.GetComponent<EletricConnection>();
            if (elconnection != null)
            {
                nearbyConnections.Remove(elconnection);
            }
        }
    }

    private void FindClosest()
    {
        if (nearbyConnections.Count > 0)
        {
            IOrderedEnumerable<EletricConnection> connections = nearbyConnections.Where(e => e.isAvailable).OrderBy(e => (transform.position - e.transform.position).sqrMagnitude);
            EletricConnection elConnection = connections.Count() > 0 ? connections.First() : null;
            SetClosestConnection(elConnection);
        }
        else if (closestConnection != null)
        {
            SetClosestConnection(null);
        }
    }

    private void OnPicked(VRInteraction caller)
    {
        coll.enabled = false;
        lookForClosestCoR = StartCoroutine(LookForClosest());

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
        FindClosest();
        
        if (closestConnection != null)
        {
            connected = true;
            elConnection = closestConnection;
            elConnection.SetConnector(this);
            SetClosestConnection(null);
            SetBackConnection(elConnection.voltage, elConnection.current);

            if (OnConnected != null)
            {
                OnConnected(elConnection);
            }
        }

        if (!connected)
        {
            coll.enabled = true;
        }

        if (lookForClosestCoR != null)
        {
            StopCoroutine(lookForClosestCoR);
            lookForClosestCoR = null;
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

    private void SetClosestConnection(EletricConnection elConnection)
    {
        if (elConnection != closestConnection)
        {
            if (closestConnection != null)
            {
                closestConnection.SetConnectIndicator(false);
            }
            if (elConnection != null)
            {
                elConnection.SetConnectIndicator(true);
            }

            closestConnection = elConnection;
        }
    }

    private IEnumerator LookForClosest()
    {
        while(true)
        {
            FindClosest();
            yield return null;
        }
    }

}
