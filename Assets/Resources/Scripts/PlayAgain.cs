using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartNewGame()
    {
        GameplayBehavior.level = 0;
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
