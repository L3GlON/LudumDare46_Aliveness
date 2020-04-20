using UnityEngine;

public class WinScreen : Screen
{
    public override ScreenType ScreenType => ScreenType.WinGame;

    public void PlayAgainPressed()
    {
        GameManager.Instane.RestartGame();
    }

    public void ExitPressed()
    {
        GameManager.Instane.ExitGame();
    }
}
