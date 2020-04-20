using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAltarMain : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject lightToActivate = null;

    private bool interacted;

    public void Interact()
    {
        if (!interacted)
        {
            GameManager.Instane.RestoreGlobalLight(ActivateMainAltar);
        }
    }

    private void ActivateMainAltar()
    {
        interacted = true;
        lightToActivate.SetActive(true);
        GameManager.Instane.WinGame();
    }
}
