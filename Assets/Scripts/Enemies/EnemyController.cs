using UnityEngine;
using DG.Tweening;
using System;

public class EnemyController : ReflectionEventsHandlerSubscriber, IDamageable
{
    [Header("Movement")]
    public float defaultSpeed = 5f;

    public Transform[] waypoints;
    public float waypointStopTime;
    private float waypointStopCurrentCooldown;
    private int currentTargetWaypointIndex;

    private Vector2 currentMovementDirection;

    [Header("Player Spotting")]
    public float spottingRange = 6f;
    public float chasingSpottingRange = 9f;
    public LayerMask targetLayerMask;
    private Collider2D targetCollider;

    [Header("Attacking")]
    public float attackingDashSpeed = 15f;
    public float attackRange = 4f;
    public float attackChargeDuration = 0.4f;
    public float attackDashDuration = 1f;
    public float attackDashBasicDamage = 15f;
    private float attackTimer;
    private float attackChargeTimer;
    private Vector2 attackDirection;

    [Header("Vitals")]
    public float defaultHealth = 30;
    private float currentHealth;
    private float CurrentHealth { get { return currentHealth; } set { currentHealth = value; enemySprite.color = Color.Lerp(zeroHPColor, fullHPColor, currentHealth / defaultHealth); } }

    private float stunLockTimer;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer enemySprite = null;
    [SerializeField] private Color fullHPColor = Color.white;
    [SerializeField] private Color zeroHPColor = Color.white;
    [SerializeField] private float deathAnimationDuration = 0.5f;

    public EnemyState CurrentEnemyState { get; private set; }

    private Action onDeath;

    protected override void Awake()
    {
        base.Awake();
        CurrentEnemyState = EnemyState.Patrolling;
        currentHealth = defaultHealth;

        GameManager.Instane.onGameOver += delegate { CurrentEnemyState = EnemyState.StandStill; };
        onDeath += GameManager.Instane.CharacterManager.characterLinesController.ShowRandomEnemyLine;
    }

    protected override void OnFixedUpdate()
    {
        if(CurrentEnemyState == EnemyState.Dying)
        {
            return;
        }

        if (CurrentEnemyState == EnemyState.Chasing || CurrentEnemyState == EnemyState.AttackPreparation || CurrentEnemyState == EnemyState.Attaking)
        {
            targetCollider = Physics2D.OverlapCircle(transform.position, chasingSpottingRange, targetLayerMask);
        }
        else
        {
            targetCollider = Physics2D.OverlapCircle(transform.position, spottingRange, targetLayerMask);
            if (targetCollider != null)
            {
                ChangeEnemyState(EnemyState.Chasing);
            }
        }
    }

    protected override void OnUpdate()
    {
        if(CurrentEnemyState == EnemyState.StandStill)
        {
            //do nothing
        }
        else if(CurrentEnemyState == EnemyState.Patrolling)
        {
            ContinuePatrollingRoute();
        }
        else if(CurrentEnemyState == EnemyState.PatrollingStop)
        {
            StandingOnWaypoint();
        }
        else if(CurrentEnemyState == EnemyState.Chasing)
        {
            ChasePlayer();
        }
        else if(CurrentEnemyState == EnemyState.AttackPreparation)
        {
            ChargeAttack();
        }
        else if(CurrentEnemyState == EnemyState.Attaking)
        {
            AttackDash();
        }
        else if(CurrentEnemyState == EnemyState.StunLock)
        {
            StayInStun();
        }

    }

    private void ContinuePatrollingRoute()
    {
        currentMovementDirection = (waypoints[currentTargetWaypointIndex].position - transform.position).normalized;

        Move(currentMovementDirection, defaultSpeed);

        if(Vector2.Distance(transform.position, waypoints[currentTargetWaypointIndex].position) <= 0.2f)
        {
            WaypointStop();
        }

    }

    private void WaypointStop()
    {
        ChangeEnemyState(EnemyState.PatrollingStop);
        waypointStopCurrentCooldown = waypointStopTime;
    }

    private void StandingOnWaypoint()
    {
        waypointStopCurrentCooldown -= Time.deltaTime;
        if (waypointStopCurrentCooldown <= 0)
        {
            SetNextPatrollingWaypoint();
            ChangeEnemyState(EnemyState.Patrolling);
        }
    }

    private void SetNextPatrollingWaypoint()
    {
        if (currentTargetWaypointIndex == waypoints.Length - 1)
        {
            currentTargetWaypointIndex = 0;
        }
        else
        {
            currentTargetWaypointIndex++;
        }
    }

    private void ChasePlayer()
    {
        if(targetCollider == null)
        {
            ChangeEnemyState(EnemyState.Patrolling);
            return;
        }
        currentMovementDirection = (targetCollider.transform.position + 2 * Vector3.up - transform.position).normalized;
        Move(currentMovementDirection, defaultSpeed);

        if(Vector2.Distance(transform.position, targetCollider.transform.position) <= attackRange)
        {
            attackChargeTimer = 0;
            ChangeEnemyState(EnemyState.AttackPreparation);
        }
    }

    private void ChargeAttack()
    {
        attackChargeTimer += Time.deltaTime;
        if(attackChargeTimer >= attackChargeDuration)
        {
            if(targetCollider == null)
            {
                ChangeEnemyState(EnemyState.Chasing);
                return;
            }
            attackDirection = (targetCollider.transform.position + 2*Vector3.up - transform.position).normalized;
            attackTimer = 0;
            ChangeEnemyState(EnemyState.Attaking);
        }
    }

    private void AttackDash()
    {
        if (attackTimer <= attackDashDuration)
        {
            Move(attackDirection, attackingDashSpeed);
        }
        else
        {
            ChangeEnemyState(EnemyState.Chasing);
            return;
        }
        attackTimer += Time.deltaTime; 
    }

    private void ChangeEnemyState(EnemyState enemyState)
    {
        CurrentEnemyState = enemyState;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CurrentEnemyState != EnemyState.Dying)
        {
            collision.GetComponent<IDamageable>().TakeDamage(attackDashBasicDamage * (2 - GameManager.Instane.globalLightController.LightPercentage)); // full darkness = 2x damage
        }
    }

    private void Move(Vector2 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if(direction.x >= 0 && enemySprite.flipX)
        {
            enemySprite.flipX = false;
        }
        else if(direction.x < 0 && !enemySprite.flipX)
        {
            enemySprite.flipX = true;
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage * Mathf.Lerp(0.5f, 1, GameManager.Instane.globalLightController.LightPercentage); // full darkness = 50% damage resist

        if(CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            StunLock(0.15f);
        }
    }

    private void StunLock(float duration)
    {
        stunLockTimer = duration;
        ChangeEnemyState(EnemyState.StunLock);
    }

    private void StayInStun()
    {
        stunLockTimer -= Time.deltaTime;
        if(stunLockTimer <= 0)
        {
            ChangeEnemyState(EnemyState.Chasing);
        }
    }

    private void Die()
    {
        ChangeEnemyState(EnemyState.Dying);
        onDeath?.Invoke();
        enemySprite.DOFade(0, deathAnimationDuration).onComplete += () => Destroy(gameObject);
    }
}


public enum EnemyState
{
    StandStill,
    Patrolling,
    PatrollingStop,
    Chasing,
    AttackPreparation,
    Attaking,
    StunLock,
    Dying
}