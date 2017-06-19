using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer_Controller : MonoBehaviour {

    [SerializeField] private Transform playerT;

    public void RequestMovement(Vector3 position)
    {
        playerT.transform.position = position;
    }
}
