using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Game()
    {
        SceneManager.LoadScene("hlavnacast1");
        Time.timeScale = 1f;
    }
    public void Spinner()
    {
        SceneManager.LoadScene("toèka");
            Time.timeScale = 1f;
    }
    public void Shop()
    {
        SceneManager.LoadScene("shop");
        Time.timeScale = 1f;
    }



}
