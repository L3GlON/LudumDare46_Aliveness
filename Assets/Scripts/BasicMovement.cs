using UnityEngine;

public class BasicMovement : ReflectionEventsHandlerSubscriber
{
    // movement config
    public MovementConfig movementConfig;

    public SpriteRenderer characterSprite;

    private Character2DMovementController movementController;
    private CharacterManager characterManager;

    [HideInInspector] public Vector3 velocity;
    private float gravity;
    private float jumpVelocity;
    private float velocityXSmoothing;

    private float normalizedHorizontalSpeed = 0;
    //statuses
    private bool isRunning;
    public bool IsRunning { get { return isRunning; } }

    public bool OnLadder { get; private set; }
    private bool applyGravity;


    protected override void Awake()
    {
        base.Awake();
        movementController = GetComponent<Character2DMovementController>();
        characterManager = GetComponent<CharacterManager>();
        velocity = new Vector3();

        gravity = -2 * movementConfig.jumpHeight / Mathf.Pow(movementConfig.timeToJumpApex, 2);
        
        jumpVelocity = Mathf.Abs(gravity * movementConfig.timeToJumpApex);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public void ApplyVelocity()
    {
        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        float targetVelocityX;
        
        if (isRunning)
        {
            targetVelocityX = normalizedHorizontalSpeed * movementConfig.heroSprintSpeed;
        }
        else
        {
            targetVelocityX = normalizedHorizontalSpeed * movementConfig.heroWalkingSpeed;
        }

        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, movementController.IsGrounded ? movementConfig.accelerationTimeGrounded : movementConfig.accelerationTimeAirborne);
        velocity.x = targetVelocityX;


        // apply gravity before moving

        if (applyGravity)
        {
            velocity.y += gravity * Time.deltaTime;
        }


        movementController.Move(velocity * Time.deltaTime);

        if (velocity.x >= 0.01f || velocity.x <= -0.01f)
        {
            float staminaCost = 0;
            if (isRunning)
            {
                staminaCost = movementConfig.sprintStaminaCostPerSecond * Time.deltaTime;
            }

            if (staminaCost != 0)
            {
                characterManager.ReduceStamina(staminaCost);
            }
        }
        // grab our current _velocity to use as a base for all calculations
        velocity = movementController.velocity;
    }

    public void Jump()
    {
        if (characterManager.CurrentStamina > 0)
        {
            velocity.y = jumpVelocity;
            applyGravity = true;
            characterManager.ReduceStamina(movementConfig.jumpingStaminaCost);
        }
    }

    public void ClimbLadderUp()
    {
        velocity.y = movementConfig.ladderClimbingSpeed;
    }

    public void ClimbLadderDown()
    {
        velocity.y = -movementConfig.ladderClimbingSpeed;
    }

    public void StopClimbing()
    {
        velocity.y = 0;
    }

    public void SetNormalizedHorizontalSpeed(float value)
    {
        normalizedHorizontalSpeed = value;
    }

    public void Run()
    {
        if(characterManager == null)
        {
            isRunning = true;
        }
        else if (characterManager.CurrentStamina > 0)
        {
            isRunning = true;
        }
        else
        {
            StopRun();
        }
    }

    public void StopRun()
    {
        isRunning = false;
    }

    public void MarkOnLadder()
    {
        OnLadder = true;
        if (velocity.y <= 0)
        {
            velocity.y = 0;
            applyGravity = false;
        }
    }

    public void MarkNotOnLadder()
    {
        OnLadder = false;
        applyGravity = true;
    }

}
