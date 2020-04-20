using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class CharacterManager : ReflectionEventsHandlerSubscriber, IDamageable
{
    public CharacterStatsConfig statsConfig;

    public CharacterLinesController characterLinesController;

    //Stamina
    public float MaxStamina { get { return statsConfig.maxStamina; } }

    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } private set { currentStamina = value; onStaminaChanged?.Invoke(); } }
    private float staminaRestorationCooldown;

    //Health
    public float MaxHealth { get { return statsConfig.maxHealth; } }

    private float currentHealth;
    public float CurrentHealth { get { return currentHealth; } private set { currentHealth = value; onHealthChanged?.Invoke(); } }

    private bool immuneToDamage;

    [SerializeField] private BasicMovement basicMovement = null;
    [SerializeField] private CharacterInput characterInput = null;

    //Projectile
    [SerializeField] private Transform projectileSpawnPoint = null;
    [SerializeField] private Projectile projectilePrefab = null;
    private float spellReloadTimer;

    //Top Light
    [SerializeField] private GameObject topLightObject = null;

    //Events
    public Action onStaminaChanged;
    public Action onHealthChanged;
    public Action onCharacterDied;

    protected override void Awake()
    {
        base.Awake();
        CurrentStamina = MaxStamina;
        CurrentHealth = MaxHealth;

        characterInput.CharacterMirroredAddListener(MirrorProjectileSpawnPoint);

        Invoke("SayFirstCharacterLine", 1f);
    }

    private void SayFirstCharacterLine()
    {
        characterLinesController.AddNewLine("Sun is dying! I must prevent the worst!");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(CurrentStamina < 100 && staminaRestorationCooldown <= 0)
        {
            CurrentStamina += statsConfig.basicStaminaRestorationPerSecond * Time.deltaTime;
            if(CurrentStamina >= 100)
            {
                CurrentStamina = 100;
            }
        }
        else if(staminaRestorationCooldown >= 0)
        {
            staminaRestorationCooldown -= Time.deltaTime;
        }

        if(spellReloadTimer >= 0)
        {
            spellReloadTimer -= Time.deltaTime;
        }
    }

    public void ToggleTopLight()
    {
        topLightObject.SetActive(!topLightObject.activeSelf);
    }


    public void ReduceStamina(float amountToUse)
    {
        if (CurrentStamina > 0)
        {
            CurrentStamina -= amountToUse;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
            staminaRestorationCooldown = statsConfig.staminaRestorationCooldown;
        }
    }

    public void RestoreHealth()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if(immuneToDamage)
        { return; }

        CurrentHealth -= damage;

        if(CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            ActivateDamageImmunity();
        }
    }


    private void ActivateDamageImmunity()
    {
        immuneToDamage = true;

        SpriteRenderer characterSprite = basicMovement.characterSprite;

        characterSprite.DOFade(0.2f, statsConfig.afterDamageImmunityDuration/4).SetLoops(4, LoopType.Yoyo).onComplete += delegate { immuneToDamage = false; } ;
    }

    private void Die()
    {
        //death_animation
        onCharacterDied?.Invoke();
        Destroy(gameObject);
    }

    public void Shoot()
    {
        if (spellReloadTimer <= 0 && CurrentStamina > 0)
        { 
            ReduceStamina(statsConfig.spellStaminaCost);

            if (!basicMovement.characterSprite.flipX)
            {
                Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).SetProjectile(Vector2.right, "Enemy", new List<string>() { "Player", "Ladder" });
            }
            else
            {
                Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).SetProjectile(Vector2.left, "Enemy", new List<string>() { "Player", "Ladder" });
            }
            spellReloadTimer = statsConfig.timeToReloadSpell;

            AudioManager.Instane.ShootEffect();
        }
    }


    private void MirrorProjectileSpawnPoint()
    {
        projectileSpawnPoint.transform.localPosition = new Vector3(-projectileSpawnPoint.transform.localPosition.x, projectileSpawnPoint.transform.localPosition.y, projectileSpawnPoint.transform.localPosition.z);
    }

}
