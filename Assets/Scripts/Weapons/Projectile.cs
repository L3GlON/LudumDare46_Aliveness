using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections.Generic;

public class Projectile : ReflectionEventsHandlerSubscriber
{
    [SerializeField] private ProjectileConfig projectileConfig = null;
    [SerializeField] private SpriteRenderer projectileSpriteRenderer = null;
    [SerializeField] private Light2D lightSource = null;
    
    private string tagToDamage;
    private List<string> tagsToIgnore = new List<string>();

    private Rigidbody2D projectileRigidbody;

    private float liveTime;
    private bool destroying;

    protected override void Awake()
    {
        base.Awake();
        tagsToIgnore.Add("Projectile");
    }

    protected override void OnUpdate()
    {
        liveTime += Time.deltaTime;
        if (liveTime >= projectileConfig.timeToLive && !destroying)
        {
            FadeProjectile();
        }
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(destroying)
        {
            return;
        }

        if (collider.CompareTag(tagToDamage))
        {
            collider.GetComponent<IDamageable>().TakeDamage(projectileConfig.damage);
            DestroyWithExplosion();
        }
        else if(!tagsToIgnore.Contains(collider.tag))
        {
            DestroyWithExplosion();
        }    
    }

    public void SetProjectile(Vector2 _direction, string _tagToDamage, List<string> _tagsToIgnore)
    {
        tagToDamage = _tagToDamage;

        tagsToIgnore.AddRange(_tagsToIgnore);

        projectileRigidbody = GetComponent<Rigidbody2D>();
        projectileRigidbody.velocity = _direction * projectileConfig.speed;
    }

    private void FadeProjectile()
    {
        destroying = true;
        projectileSpriteRenderer.DOFade(0, 0.15f).onComplete += delegate { Destroy(gameObject); } ;
        DOTween.To(() => lightSource.intensity, x => lightSource.intensity = x, 0, 0.15f);
    }

    private void DestroyWithExplosion()
    {
        destroying = true;
        projectileRigidbody.velocity = Vector2.zero;
        transform.DOScale(2, 0.05f);
        DOTween.To(() => lightSource.intensity, x => lightSource.intensity = x, 2.3f, 0.05f).onComplete += delegate { Destroy(gameObject); };
    }
}
