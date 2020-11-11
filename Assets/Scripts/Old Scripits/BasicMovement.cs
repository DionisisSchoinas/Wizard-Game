using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Vector3 speedVector = new Vector3(-2f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(speedVector, ForceMode.Impulse);
    }
}
