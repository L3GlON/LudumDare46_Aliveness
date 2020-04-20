using UnityEngine;

public class PauseScreen : Screen
{
    public override ScreenType ScreenType => ScreenType.Pause;

    public void ResumePressed()
    {
        GameManager.Instane.Unpause();
    }

    public void ExitPressed()
    {
        GameManager.Instane.ExitGame();
    }
}
