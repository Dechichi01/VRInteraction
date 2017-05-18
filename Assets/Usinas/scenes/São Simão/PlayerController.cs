using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

    CharacterController playerController;

    public float moveTowardsTime = 3f;
	// Use this for initialization
	void Start () {
        playerController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        playerController.SimpleMove(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")));
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(MoveTowards(transform.position, transform.position + Vector3.up * 3f + Vector3.forward * 5));
	}

    IEnumerator MoveTowards(Vector3 start, Vector3 end)
    {
        float percent = 0;
        float moveTowardsSpeed = 1/moveTowardsTime;

        float sign = 1;
        Vector3 nextPos = Vector3.Slerp(start, end, 0.2f);
        if ((nextPos - transform.position).y < 0)
            sign = -1;

        while (percent<=1)
        {
            percent += (Time.deltaTime * moveTowardsSpeed);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 0.4f);
            for (int i = 0; i < colliders.Length; i++)
            {
                Debug.Log(colliders[i].name);
            }
            nextPos = Vector3.Slerp(start, end, percent);
            nextPos.y = sign * nextPos.y;           
            transform.position = nextPos;
            yield return null;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("trigger");
    }
}
