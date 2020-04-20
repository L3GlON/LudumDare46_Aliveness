using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStatsConfig", menuName = "ScriptableObject/Config/CharacterStatsConfig" )]
public class CharacterStatsConfig : ScriptableObject
{
    public float maxStamina;
    public float maxHealth;

    public float basicStaminaRestorationPerSecond;
    public float staminaRestorationCooldown;

    public float timeToReloadSpell;
    public float spellStaminaCost;
    public float afterDamageImmunityDuration;
}
