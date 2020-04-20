using UnityEngine;

public class SunAltar : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject lightToActivate = null;

    private bool interacted;

    public void Interact()
    {
        if (!interacted)
        {
            GameManager.Instane.RestoreGlobalLight(MarkAsInteracted);
        }
    }

    private void MarkAsInteracted()
    {
        interacted = true;
        lightToActivate.SetActive(true);
        GameManager.Instane.CharacterManager.characterLinesController.AddNewLine("Praise the sun!");
    }
}
