using UnityEngine;
using TMPro;

public class GameOverScreen : Screen
{
    [SerializeField] private TextMeshProUGUI screenTitle = null;

    public override ScreenType ScreenType => ScreenType.GameOver;


    public void InitCharacterDied()
    {
        screenTitle.text = "YOU DIED";
    }

    public void InitSunDied()
    {
        screenTitle.text = "SUN DIED";
    }


    public void PlayAgainPressed()
    {
        GameManager.Instane.RestartGame();
    }

    public void ExitGamePressed()
    {
        GameManager.Instane.ExitGame();
    }


}
