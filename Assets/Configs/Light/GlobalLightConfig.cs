using UnityEngine;

[CreateAssetMenu(fileName = "GlobalLightConfig", menuName = "ScriptableObject/Config/GlobalLightConfig")]
public class GlobalLightConfig : ScriptableObject
{
    public float maxLightIntensity;
    [Tooltip("Value, where game will be considered over")]
    public float minLightIntensity;
    [Tooltip("Seconds for light to gor from max to min")]
    public float timeToGoOut;
    [Tooltip("Amount of seconds, when light will be at max intencity")]
    public float postRestorationFullIntencityDuration;
    [Tooltip("How much light intensity will be restored by 1 second")]
    public float lightRestorationSpeed;
}
