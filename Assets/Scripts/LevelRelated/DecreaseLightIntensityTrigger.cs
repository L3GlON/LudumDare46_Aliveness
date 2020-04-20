using UnityEngine;

public class DecreaseLightIntensityTrigger : MonoBehaviour
{
    private bool triggered;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            triggered = true;
            GameManager.Instane.globalLightController.DescreaseTheLight();
        }
    }
}
