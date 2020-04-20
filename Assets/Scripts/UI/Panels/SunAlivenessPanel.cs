using UnityEngine;
using UnityEngine.UI;

public class SunAlivenessPanel : MonoBehaviour
{
    [SerializeField] private GlobalLightController globalLightController = null;

    [SerializeField] private Image sunAlivenessBar = null;

    [SerializeField] private Color maxColor = Color.white;
    [SerializeField] private Color minColor = Color.white;


    private void Awake()
    {
        globalLightController.OnLightChanged += ChangeSunAliveness;
    }

    private void ChangeSunAliveness()
    {
        sunAlivenessBar.fillAmount = globalLightController.LightPercentage;
        sunAlivenessBar.color = Color.Lerp(minColor, maxColor, sunAlivenessBar.fillAmount);

    }

}
