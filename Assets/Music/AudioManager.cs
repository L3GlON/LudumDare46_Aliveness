using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager instance;

    public static AudioManager Instane
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }

    #endregion

    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioSource audioEffectsSource = null;

    private void Awake()
    {
        GameManager.Instane.onGameOver += StopMusic;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void ShootEffect()
    {
        audioEffectsSource.Play();
    }

}
