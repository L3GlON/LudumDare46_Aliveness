using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : ReflectionEventsHandlerSubscriber
{
    #region Singleton
    private static GameManager instance;

    public static GameManager Instane 
    { 
        get 
        { 
            if (instance == null) 
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        } 
    }

    #endregion

    public float timeInTotalDarknessBeforeGameOver;

    public GameState CurrentGameState { get; private set; }

    public GlobalLightController globalLightController = null;
    [SerializeField] private CharacterManager characterManager = null;
    public CharacterManager CharacterManager { get { return characterManager; } }
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private GameObject characterPanel = null;
    [SerializeField] private Image whiteFlash = null;
    [SerializeField] private GameObject sunIsAlmostDeadWarning = null;

    private float sunLastTimer;
    private bool sunAlmostDead;

    public Action onGameOver;
    public Action onWin;

    protected override void Awake()
    {
        base.Awake();
        CurrentGameState = GameState.Game;
        characterManager.onCharacterDied += GameOverCharacterDied;
        globalLightController.onLightGoneOff += StartSunDeathCountdown;
    }

    protected override void OnUpdate()
    {
        if(CurrentGameState == GameState.GameOver || CurrentGameState == GameState.Win)
        {
            return;
        }


        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(CurrentGameState == GameState.Game)
            {
                Pause();
            }
            else if(CurrentGameState == GameState.Pause)
            {
                Unpause();
            }
        }

        if(sunAlmostDead)
        {
            sunLastTimer -= Time.deltaTime;
            if(sunLastTimer <= 0)
            {
                GameOverSunDied();
            }
        }

    }

    public void RestoreGlobalLight(Action onSuccess)
    {
        if (characterManager.CurrentStamina > 0)
        {
            characterManager.ReduceStamina(characterManager.CurrentStamina);
            characterManager.RestoreHealth();
            globalLightController.RestoreTheLight();
            onSuccess?.Invoke();


            sunAlmostDead = false;
            sunIsAlmostDeadWarning.SetActive(false);
        }
    }

    public void WinGame()
    {
        CurrentGameState = GameState.Win;
        characterPanel.SetActive(false);
        whiteFlash.gameObject.SetActive(true);
        whiteFlash.DOFade(0, 4).SetDelay(1f).onComplete += delegate { uiManager.OpenScreenOfType(ScreenType.WinGame); };
        Destroy(characterManager.gameObject);
        sunIsAlmostDeadWarning.SetActive(false);

        onWin?.Invoke();
    }

    private void GameOverCharacterDied()
    {
        if(CurrentGameState == GameState.GameOver)
        {
            return;
        }

        CurrentGameState = GameState.GameOver;
        onGameOver?.Invoke();
        ((GameOverScreen)uiManager.OpenScreenOfType(ScreenType.GameOver)).InitCharacterDied();
    }

    private void GameOverSunDied()
    {
        if (CurrentGameState == GameState.GameOver)
        {
            return;
        }
        
        sunIsAlmostDeadWarning.SetActive(false);

        CurrentGameState = GameState.GameOver;
        onGameOver?.Invoke();
        ((GameOverScreen)uiManager.OpenScreenOfType(ScreenType.GameOver)).InitSunDied();
    }

    private void StartSunDeathCountdown()
    {
        if (!sunAlmostDead)
        {
            sunLastTimer = timeInTotalDarknessBeforeGameOver;
            sunAlmostDead = true;
            sunIsAlmostDeadWarning.SetActive(true);
        }
    }

    private void Pause()
    {
        uiManager.OpenScreenOfType(ScreenType.Pause);
        CurrentGameState = GameState.Pause;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        uiManager.CloseOpenedScreen();
        CurrentGameState = GameState.Game;
        Time.timeScale = 1;
    }


    public void RestartGame()
    {
        DOTween.KillAll();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}


public enum GameState
{
    Game,
    Pause,
    GameOver,
    Win
}