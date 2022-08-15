using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadScene(int scene)
    {
        GameManager.Instance.CheckTimeScale();
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
