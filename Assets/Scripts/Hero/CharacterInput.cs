using UnityEngine;
using UnityEngine.Events;

public class CharacterInput : ReflectionEventsHandlerSubscriber
{
    [Header("Camera reference")]
    public Camera mainCamera;
   
    private Character2DMovementController movementController;
    private CharacterManager characterManager;
    private BasicMovement basicMovement;

    //Triggers
    private IInteractable interactableTriggerWithinRange;
    private bool onLadder;

    //Events
    private UnityEvent CharacterMirrored;

    protected override void Awake()
	{
        base.Awake();
        basicMovement = GetComponent<BasicMovement>();
        movementController = GetComponent<Character2DMovementController>();
        characterManager = GetComponent<CharacterManager>();

        // listen to some events for illustration purposes
        movementController.OnControllerCollidedEvent += OnControllerCollider;
        movementController.OnTriggerEnterEvent += OnTriggerEnterEvent;
        movementController.OnTriggerExitEvent += OnTriggerExitEvent;
    }

	void OnControllerCollider( RaycastHit2D hit )
	{
        if (hit.normal.y == 1f)
        {
            return;
        }
	}

	void OnTriggerEnterEvent(Collider2D collider)
	{
        if (collider.CompareTag("Interactable"))
        {
            interactableTriggerWithinRange = collider.GetComponent<IInteractable>();
        }
        else if (collider.CompareTag("Ladder"))
        {
            onLadder = true;
        }
	}

	void OnTriggerExitEvent(Collider2D collider)
	{
		if (collider.CompareTag("Interactable"))
        {
            interactableTriggerWithinRange = null;
        }
        else if (collider.CompareTag("Ladder"))
        {
            onLadder = false;
        }
	}

	// the Update loop contains a very simple example of moving the character around and controlling the animation
	protected override void OnUpdate()
	{
        if(GameManager.Instane.CurrentGameState == GameState.GameOver || GameManager.Instane.CurrentGameState == GameState.Pause)
        {
            return;
        }


		if(movementController.IsGrounded && basicMovement.OnLadder)
        {
            basicMovement.velocity.y = 0;
        }

        //rolling blocks almost any other actions
        if (Input.GetKey(KeyCode.D))
        {
            basicMovement.SetNormalizedHorizontalSpeed(1);
            if (basicMovement.characterSprite.flipX)
            {
                MirrorCharacter();
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            basicMovement.SetNormalizedHorizontalSpeed(-1);
            if (!basicMovement.characterSprite.flipX)
            {
                MirrorCharacter();
            }
        }
        else
        {
            basicMovement.SetNormalizedHorizontalSpeed(0);
        }

        if (onLadder)
        {
            basicMovement.MarkOnLadder();
            if (Input.GetKey(KeyCode.W))
            {
                basicMovement.ClimbLadderUp();
                basicMovement.ClimbLadderUp();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                movementController.IngnoreOneWayPlatform();
                basicMovement.ClimbLadderDown();
            }
            else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                basicMovement.StopClimbing();
            }

        }
        else
        {
            basicMovement.MarkNotOnLadder();
        }

        if (Input.GetMouseButton(0))
        {
            if (characterManager.CurrentStamina >= 0)
            {
                characterManager.Shoot();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            characterManager.ToggleTopLight();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            basicMovement.Run();
        }
        else
        {
            basicMovement.StopRun();
        }

        if (interactableTriggerWithinRange != null && Input.GetKeyDown(KeyCode.E))
        {
            interactableTriggerWithinRange.Interact();
        }

        // we can only jump while grounded or on ladder
        if ((movementController.IsGrounded || onLadder) && Input.GetKeyDown(KeyCode.Space))
        {
            basicMovement.Jump();
        }
        basicMovement.ApplyVelocity();
	}

    private void MirrorCharacter()
    {
        basicMovement.characterSprite.flipX = !basicMovement.characterSprite.flipX;
        CharacterMirrored?.Invoke();
    }

    public void CharacterMirroredAddListener(UnityAction newListener)
    {
        if(CharacterMirrored == null)
        {
            CharacterMirrored = new UnityEvent();
        }
        CharacterMirrored.AddListener(newListener);
    }
}
