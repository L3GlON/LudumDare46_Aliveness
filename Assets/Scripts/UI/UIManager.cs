using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Screen[] screens = null;

    private Screen openedScreen;
    public Screen OpenScreenOfType(ScreenType screenType)
    {
        for(int i = 0; i < screens.Length; i++)
        {
            if(screens[i].ScreenType == screenType)
            {
                screens[i].Open();
                openedScreen = screens[i];
                return screens[i];
            }
        }

        return null;
    }

    public void CloseOpenedScreen()
    {
        openedScreen.Close();
    }
}
