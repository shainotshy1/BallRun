using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerCollisionHandler: MonoBehaviour
{
    [HideInInspector] public bool playerGrounded;
    public ScoreHandler scoreHandler;

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
        if (collision.gameObject.tag == "Ground")
        {
            playerGrounded = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && PlayerControls.collisionsEnabled)
        {
            PlayerControls playerControls = FindObjectOfType<PlayerControls>();
            ScoreHandler.AddScoreToTotal();
            playerControls.PlayerDeath();
        }
    }
}
