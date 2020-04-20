using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "ScriptableObject/Config/MovementConfig")]
public class MovementConfig : ScriptableObject
{
    [Header("Speed")]
    public float heroWalkingSpeed;
    public float heroSprintSpeed;
    public float ladderClimbingSpeed;
    [Header("Stamina Prices")]
    public float sprintStaminaCostPerSecond;
    public float jumpingStaminaCost;
    [Header("Jump")]
    public float jumpHeight;
    public float timeToJumpApex;
    [Header("Acceleration")]
    public float accelerationTimeAirborne;
    public float accelerationTimeGrounded;
}
