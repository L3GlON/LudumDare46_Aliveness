using UnityEngine;

public class CharacterLineTrigger : MonoBehaviour
{
    public string characterLine;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !triggered)
        {
            triggered = true;

            GameManager.Instane.CharacterManager.characterLinesController.AddNewLine(characterLine);
        }
    }
}
