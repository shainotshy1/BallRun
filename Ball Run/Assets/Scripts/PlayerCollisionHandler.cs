using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCollisionHandler : MonoBehaviour
{
    [HideInInspector] public bool playerGrounded;

    ScoreHandler scoreHandler;
    private void Start()
    {
        scoreHandler = new ScoreHandler(0);
        playerGrounded = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        playerGrounded = collision.gameObject.tag == "Ground";
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            playerGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            PlayerControls playerControls = FindObjectOfType<PlayerControls>();
            ScoreHandler.AddScoreToTotal();
            playerControls.PlayerDeath();
        }
    }
}
