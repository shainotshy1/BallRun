using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerCollisionHandler: MonoBehaviour
{
    [SerializeField] float skinYPos;
    [SerializeField] float skinScale;

    [HideInInspector] public bool playerGrounded;
    public ScoreHandler scoreHandler;

    private void Start()
    {
        scoreHandler = new ScoreHandler(0);
        playerGrounded = false;

        GameObject selectedSkin = Instantiate(SkinSelectionHandler.skinTypes[PlayerPrefs.GetInt("SelectedSkinIndex")],new Vector3(0,skinYPos,0), Quaternion.Euler(0,0,0), transform);
        selectedSkin.transform.localScale = new Vector3(skinScale, skinScale, skinScale);
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
            playerControls.PlayerDeath();
        }
    }
}
