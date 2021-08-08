using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    public static float playerMovementDistance = 0f;
    public static bool collisionsEnabled = true;

    [SerializeField] int initialReviveCost;
    [SerializeField] int reviveCostIncrement;
    [SerializeField] float jumpInitialVelocity;
    [SerializeField] float horizontalSpeed;
    [SerializeField] float revivalIndestructableTime;
    [SerializeField] float revivalCantAffordMessageTime;
    [SerializeField] InputAction movement;
    [SerializeField] TextMeshProUGUI reviveText;
    [SerializeField] GameObject reviveCanvas;
    [SerializeField] GameObject unableToAffordCanvas;

    int currentReviveCost;

    enum Position
    {
        Left, Middle, Right, Idle
    }
    bool isGrounded;
    bool controlsEnabled;
    Vector3 movementDirection = new Vector3(1, 0, 0);
    Position setPosition;
    Transform bodyTransform;
    Rigidbody rigidBody;
    PlayerCollisionHandler collisionHandler;
    
    private void Awake()
    {
        unableToAffordCanvas.SetActive(false);
    }
    private void Start()
    {
        PlatformHandler.pickupsBetweenGems = 0;
        currentReviveCost = initialReviveCost;
        setPosition = Position.Middle;
        controlsEnabled = true;
        ScoreHandler.ResetScore();

        foreach (Transform child in transform)
        {
            foreach (Transform grandChild in child)
            {
                if (grandChild.gameObject.tag == "JumpCenter")
                {
                    bodyTransform = grandChild;
                    rigidBody = grandChild.GetComponent<Rigidbody>();
                    collisionHandler = rigidBody.GetComponent<PlayerCollisionHandler>();
                }
            }
        }
        rigidBody.useGravity = true;
    }
    private void OnEnable()
    {
        movement.Enable();
    }
    private void OnDisable()
    {
        movement.Disable();
    }
    private void SetYVelocity(float velocity)
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, velocity, rigidBody.velocity.z);
    }
    private void ProcessInput()
    {
        ResetPhysics();
        if ((Input.GetKey(KeyCode.Space)||GetComponent<Swipe>().SwipeUp) && isGrounded)
        {
            SetYVelocity(jumpInitialVelocity);
        }
        if((Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.LeftShift) || GetComponent<Swipe>().SwipeDown) && !isGrounded){
            if (rigidBody.velocity.y>=-jumpInitialVelocity)
            {
                SetYVelocity(-jumpInitialVelocity);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || GetComponent<Swipe>().SwipeLeft)
        {
            if (setPosition == Position.Middle)
            {
                setPosition = Position.Left;
            }
            else if (setPosition == Position.Right)
            {
                setPosition = Position.Middle;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || GetComponent<Swipe>().SwipeRight)
        {
            if (setPosition == Position.Middle)
            {
                setPosition = Position.Right;
            }
            if (setPosition == Position.Left)
            {
                setPosition = Position.Middle;
            }
        }

        GoToPosition(setPosition);
    }
    private void GoToPosition(Position position)
    {
        if (position == Position.Middle)
        {
            float xPos = 0f;
            float zPos = 0f;

            if (movementDirection.x == 1 || movementDirection.x == -1)
            {
                xPos = 0;
            }
            else if (movementDirection.z == 1 || movementDirection.z == -1)
            {
                zPos = 0;
            }

            MoveChild(xPos,zPos);
        }
        if (position == Position.Left)
        {
            MoveChild(- playerMovementDistance * movementDirection.z, playerMovementDistance * movementDirection.x);
        }
        else if (position == Position.Right)
        {
            MoveChild(playerMovementDistance * movementDirection.z, -playerMovementDistance * movementDirection.x);
        }
    }
    private void MoveChild(float newX,float newZ)
    {
        Vector3 endPosition = new Vector3(newX, bodyTransform.transform.localPosition.y, newZ);
        bodyTransform.transform.localPosition = Vector3.Lerp(bodyTransform.transform.localPosition, endPosition, Time.deltaTime * horizontalSpeed);
    }
    public void PlayerDeath()
    {
        ResetReviveButton();
        PathHandler.pathRunning = false;
        GetComponent<SceneLoader>().SetGameOverActive(true);
        PausePhysics();
    }
    public void Revive()
    {
        int currentTotal = PlayerPrefs.GetInt("TotalGems");
        if (currentTotal >= currentReviveCost)
        {
            PathHandler.pathRunning = true;
            PlayerPrefs.SetInt("TotalGems", currentTotal - currentReviveCost);
            currentReviveCost += reviveCostIncrement;
            GetComponent<SceneLoader>().SetGameOverActive(false);
            ResetPhysics();
            StartCoroutine(RevivalCoroutine());
        }
        else
        {
            StartCoroutine(UnableToAffordCoroutine());
        }
    }
    public void ResetReviveButton()
    {
        unableToAffordCanvas.SetActive(false);
        reviveCanvas.SetActive(true);
        reviveText.text = $"Revive for {currentReviveCost}";
    }
    IEnumerator UnableToAffordCoroutine()
    {
        reviveCanvas.SetActive(false);
        unableToAffordCanvas.SetActive(true);
        float time = revivalCantAffordMessageTime;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        ResetReviveButton();
    }
    IEnumerator RevivalCoroutine()
    {
        collisionsEnabled = false;
        float time = revivalIndestructableTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        collisionsEnabled = true;
    }
    private void PausePhysics()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.useGravity = false;
        rigidBody.gameObject.GetComponent<SphereCollider>().enabled = false;
    }
    private void ResetPhysics()
    {
        rigidBody.useGravity = true;
        rigidBody.gameObject.GetComponent<SphereCollider>().enabled = true;
    }
    void Update()
    {
        isGrounded = collisionHandler.playerGrounded;
        ScoreHandler scoreHandler = new ScoreHandler();
        scoreHandler.DisplayScore();

        if (transform.position.y < -10)
        {
            PlayerDeath();
        }
        controlsEnabled = PathHandler.pathRunning;
        if (controlsEnabled) ProcessInput();
        else PausePhysics();
    }
}