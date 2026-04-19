using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    public CanvasGroup fadePanel;
    private AudioManager audio;

    private void Start()
    {
        audio = FindAnyObjectByType<AudioManager>();
    }

    public void PlayGame()
    {
        StartCoroutine(LoadStage1());

        audio.PlayButtonSound();
    }

    IEnumerator LoadStage1()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            fadePanel.alpha = i;
            yield return null;
        }

        SceneManager.LoadScene("Stage1");
    }

    public void ExitGame()
    {
        audio.PlayOtherButtonSound();
        Application.Quit();
    }
}
