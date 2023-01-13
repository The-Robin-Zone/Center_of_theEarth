using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public GameObject pausebutton;
    public GameObject resumebutton;
    public GameObject scope;

    private void Awake()
    {
        
    }

    private void Update()
    {

    }

    public void resetTimeScale()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pausebutton.SetActive(false);
        scope.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    //In case youre dead we need this option, Currently not in use
    public void PauseNoResume()
    {
        resumebutton.SetActive(false);
        pauseMenu.SetActive(true);
        resetTimeScale();
    }

    public void Resume()
    {
        pausebutton.SetActive(true);
        scope.SetActive(true);
        pauseMenu.SetActive(false);
        resetTimeScale();
    }

    public void Restart()
    {
        resetTimeScale();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

    public void Exit()
    {
        resetTimeScale();
        SceneManager.LoadScene(0);


        //Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
