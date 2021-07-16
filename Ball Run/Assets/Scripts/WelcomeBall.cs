using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WelcomeBall : MonoBehaviour
{
    [SerializeField] float jumpVelocity;
    Rigidbody rigidBody;
    float initalY;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        initalY = transform.position.y;
        rigidBody.velocity = Vector3.up*jumpVelocity;
    }
    private void Update()
    {
        if (transform.position.y<=initalY)
        {
            rigidBody.velocity = Vector3.up * jumpVelocity;
        }
    }
}
