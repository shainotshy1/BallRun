using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class WelcomeBall : MonoBehaviour
{
    [SerializeField] float jumpVelocity;
    [SerializeField] TextMeshProUGUI totalScore;
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
        totalScore.text = $"Total Score: {PlayerPrefs.GetInt("TotalScore")}";
        if (transform.position.y<=initalY)
        {
            rigidBody.velocity = Vector3.up * jumpVelocity;
        }
    }
}
