using UnityEngine;
using System;

public class CameraController : ReflectionEventsHandlerSubscriber
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	private Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private Character2DMovementController _playerController;
	private Vector3 _smoothDampVelocity;

	private Vector3 previousFramePosition;

	public Action<Vector3> onCameraMovedDelta;


	protected override void Awake()
	{
		base.Awake();
		transform = gameObject.transform;
		_playerController = target.GetComponent<Character2DMovementController>();
		previousFramePosition = transform.position;
		cameraOffset = target.position - transform.position;
	}
	
	
	protected override void OnLateUpdate()
	{
        if (!useFixedUpdate)
        {
            UpdateCameraPosition();
        }
	}


	protected override void OnFixedUpdate()
	{
        if (useFixedUpdate)
        {
            UpdateCameraPosition();
        }
	}


	void UpdateCameraPosition()
	{
		if( _playerController == null )
		{
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}

		onCameraMovedDelta?.Invoke(transform.position - previousFramePosition);
		previousFramePosition = transform.position;
	}
	
}
