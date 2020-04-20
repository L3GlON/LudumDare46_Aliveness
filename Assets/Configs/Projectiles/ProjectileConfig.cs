using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfig", menuName = "ScriptableObject/Config/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    public float speed;
    public float damage;
    public float timeToLive;
}
