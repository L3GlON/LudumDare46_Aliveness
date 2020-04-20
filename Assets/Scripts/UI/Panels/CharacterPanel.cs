using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private CharacterManager characterManager = null;

    [Header("Health")]
    [SerializeField] private Image healthBar = null;
    [SerializeField] private Image healthBarBackground = null;
    [Header("Stamina")]
    [SerializeField] private Image staminaBar = null;
    [SerializeField] private Image staminaBarBackground = null;

    private bool healthBarIsFlickering;
    private bool staminaBarIsFlickering;

    private const float FLICKERING_PERIOD = 0.2f;

    private void Start()
    {
        characterManager.onHealthChanged += UpdateHealthBar;
        characterManager.onStaminaChanged += UpdateStaminaBar;

        UpdateHealthBar();
        UpdateStaminaBar();
    }


    private void UpdateHealthBar()
    {
        healthBar.fillAmount = characterManager.CurrentHealth / characterManager.MaxHealth;

        if (healthBar.fillAmount <= 0.33f && !healthBarIsFlickering)
        {
            EnableHealthFlickering();
        }
        else if (healthBar.fillAmount > 0.33f && healthBarIsFlickering)
        {
            StopHealthFlickering();
        }

    }

    private void UpdateStaminaBar()
    {
        staminaBar.fillAmount = characterManager.CurrentStamina / characterManager.MaxStamina;


        if (staminaBar.fillAmount <= 0.33f && !staminaBarIsFlickering)
        {
            EnableStaminaFlickering();
        }
        else if (staminaBar.fillAmount > 0.33f && staminaBarIsFlickering)
        {
            StopStaminaFlickering();
        }
    }

    private void EnableHealthFlickering()
    {
        healthBarIsFlickering = true;
        healthBar.DOColor(Color.red, FLICKERING_PERIOD).SetLoops(int.MaxValue, LoopType.Yoyo);
        healthBarBackground.DOColor(Color.red, FLICKERING_PERIOD).SetLoops(int.MaxValue, LoopType.Yoyo);
    }

    private void StopHealthFlickering()
    {
        healthBarIsFlickering = false;
        healthBar.DOKill();
        healthBar.color = Color.white;
        healthBarBackground.DOKill();
        healthBarBackground.color = Color.white;
    }

    private void EnableStaminaFlickering()
    {
        staminaBarIsFlickering = true;
        staminaBar.DOColor(Color.red, FLICKERING_PERIOD).SetLoops(int.MaxValue, LoopType.Yoyo);
        staminaBarBackground.DOColor(Color.red, FLICKERING_PERIOD).SetLoops(int.MaxValue, LoopType.Yoyo);
    }

    private void StopStaminaFlickering()
    {
        staminaBarIsFlickering = false;
        staminaBar.DOKill();
        staminaBar.color = Color.white;
        staminaBarBackground.DOKill();
        staminaBarBackground.color = Color.white;
    }
}
