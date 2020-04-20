using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterLines", menuName = "ScriptableObject/Config/CharacterLines")]
public class CharacterLines : ScriptableObject
{
    public string[] lines;


    public string GetRandomLine()
    {
        return lines[Random.Range(0, lines.Length)];
    }
}
