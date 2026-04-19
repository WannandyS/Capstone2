using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public bool isGameActive;

    void Start()
    {
        isGameActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryStage1()
    {
        SceneManager.LoadScene(1);
    }

    public void RetryStage2()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
