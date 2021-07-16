using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WelcomeBall : MonoBehaviour
{
    [SerializeField] float jumpVelocity;
    Rigidbody rigidBody;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rigidBody.velocity = Vector3.up*jumpVelocity;
    }
    private void Update()
    {
        if (transform.position.y<=0)
        {
            rigidBody.velocity = Vector3.up * jumpVelocity;
        }
    }
}
