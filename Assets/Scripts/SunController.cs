using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SunController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer deadSunSprite = null;
    [SerializeField] private Light2D deadSunLight = null;

    private Color deadSunCurrentColor;

    private void Awake()
    {
        GameManager.Instane.globalLightController.OnLightChanged += UpdateSun;
        deadSunCurrentColor = deadSunSprite.color;
    }

    private void UpdateSun()
    {
        float targetValue = 1 - GameManager.Instane.globalLightController.LightPercentage;

        deadSunCurrentColor.a = targetValue;
        deadSunLight.intensity = targetValue;

        deadSunSprite.color = deadSunCurrentColor;
    }
}
