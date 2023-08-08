using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Must be used to change scenes
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Removed start and update to create new method for start menu controls
    public void PlayGame()
    {
        //Loads next scene in queue
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}