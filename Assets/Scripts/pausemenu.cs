using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public GameObject resumebutton;
    public GameObject scope;

    private void Update()
    {
        if (Input.GetKeyDown("p") && Time.timeScale == 1)
        {
            Pause();
        }
        else if(Input.GetKeyDown("p") && Time.timeScale == 0)
        {
            Resume();
        }
    }

    public void Pause()
    {
        scope.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    //In case youre dead we need this option
    public void PauseNoResume()
    {
        resumebutton.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        scope.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public void Exit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
