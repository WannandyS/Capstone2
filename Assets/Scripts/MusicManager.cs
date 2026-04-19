using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioSource musicSource;

    public AudioClip mainMenuMusic;
    public AudioClip stage1Music;
    public AudioClip stage2Music;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            PlayMusic(mainMenuMusic);
        }
        else if (scene.name == "Stage1")
        {
            PlayMusic(stage1Music);
        }
        else if (scene.name == "Stage2")
        {
            PlayMusic(stage2Music);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}