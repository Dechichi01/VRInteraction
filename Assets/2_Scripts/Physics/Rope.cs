using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rope : MonoBehaviour {

    [SerializeField] protected Rigidbody startRb;
    [SerializeField] protected Rigidbody endRb;
    [SerializeField] private float ropeDrag = 0;
    [SerializeField] private float ropeAngularDrag = 0.1f;

    protected Rigidbody[] ropeChilds;
    protected Collider[] ropeColliders;

    protected virtual void Start()
    {
        if (startRb.transform.parent != transform || endRb.transform.parent != transform)
        {
            Debug.LogError("Start and End of rope should be direct childs of the Rope game object.");
            return;
        }

        Transform[] childs = GetComponentsInChildren<Transform>();
        childs = childs.Where(c => c.parent == transform && c != startRb.transform && c != endRb.transform).ToArray();

        int childCount = childs.Length;
        Rigidbody prevRb;

        AddJoint(childs[0].gameObject, startRb);
        for (int i = 1; i < childCount; i++)
        {
            prevRb = childs[i - 1].GetComponent<Rigidbody>();
            AddJoint(childs[i].gameObject, prevRb);
        }
        prevRb = childs.Last().GetComponent<Rigidbody>();

        AddJoint(endRb.gameObject, prevRb);

        CashChilds();
        SetRopeParams();
    }

    private void AddJoint(GameObject obj, Rigidbody connectBody)
    {
        HingeJoint joint = obj.AddComponent<HingeJoint>();
        joint.connectedBody = connectBody;
        joint.useSpring = true;
        joint.enableCollision = true;
    }

    public void SetRopeParams()
    {
        foreach (var rb in ropeChilds)
        {
            rb.drag = ropeDrag;
            rb.angularDrag = ropeAngularDrag;
        }
    }

    public void SetColliders(bool value)
    {
        foreach (var c in ropeColliders)
        {
            c.enabled = value;
        }
    }

    private void CashChilds()
    {
        ropeChilds = GetComponentsInChildren<Rigidbody>();

        ropeColliders = GetComponentsInChildren<Collider>();
        Collider[] parentColliders = GetComponents<Collider>();
        if (parentColliders.Length > 0)
        {
            ropeColliders = ropeColliders.Where(c => !System.Array.Exists(parentColliders, pc => c == pc)).ToArray();
        }
    }
}
