using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EletricConnection : MonoBehaviour {

    [SerializeField] private bool _isInput = false;
    [HideInInspector] public float voltage = 0f;
    [HideInInspector] public float current = 0f;

    public bool isInput { get { return _isInput; } }

    private void Start()
    {
        gameObject.tag = "EletricConnection";
    }
}
