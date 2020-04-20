using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
using DG.Tweening;

public class GlobalLightController : ReflectionEventsHandlerSubscriber
{
    [SerializeField] private GlobalLightConfig globalLightConfig = null;

    [SerializeField] private Light2D globalLightSource = null;

    public float LightPercentage { get { return (globalLightSource.intensity - globalLightConfig.minLightIntensity) / (globalLightConfig.maxLightIntensity - globalLightConfig.minLightIntensity); } }


    private float timerToEndLight;
    private float postRestorationTimer;


    public Action onLightGoneOff;
    public Action onLightRestored;
    public Action OnLightChanged;

    private bool restorationInProgress;


    protected override void Awake()
    {
        base.Awake();
    }


    protected override void OnUpdate()
    {
        if(restorationInProgress)
        {
            SetLightIntencity(globalLightSource.intensity + Time.deltaTime * globalLightConfig.lightRestorationSpeed);

            if(globalLightSource.intensity >= globalLightConfig.maxLightIntensity)
            {
                restorationInProgress = false;
            }
            return;
        }

        if(GameManager.Instane.CurrentGameState == GameState.Win)
        {
            return;
        }

        if (postRestorationTimer >= 0)
        {
            postRestorationTimer -= Time.deltaTime;
        }
        else
        {
            if (globalLightSource.intensity >= 0.5f)
            {
                timerToEndLight += Time.deltaTime * 1.5f;
            }
            else
            {
                timerToEndLight += Time.deltaTime / 1.5f;
            }

            SetLightIntencity(Mathf.Lerp(globalLightConfig.minLightIntensity, globalLightConfig.maxLightIntensity, 1 - timerToEndLight / globalLightConfig.timeToGoOut));

            if (globalLightSource.intensity <= globalLightConfig.minLightIntensity)
            {
                onLightGoneOff?.Invoke();
            }
        }
    }

    private void SetLightIntencity(float targetIntencity)
    {
        globalLightSource.intensity = targetIntencity;
        OnLightChanged?.Invoke();
    }


    public void DescreaseTheLight()
    {
        if (timerToEndLight < globalLightConfig.timeToGoOut * 0.7f)
        {
            DOTween.To(() => timerToEndLight, x => timerToEndLight = x, globalLightConfig.timeToGoOut * 0.7f, 1f);
        }
    }

    public void RestoreTheLight()
    {
        onLightRestored?.Invoke();
        timerToEndLight = 0;
        postRestorationTimer = globalLightConfig.postRestorationFullIntencityDuration;

        restorationInProgress = true;
    }
}
