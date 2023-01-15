using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
    public GameObject pauseMenu;
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

        Global_Variables.gameFrozen = false;
    }

    public void gameFreeze()
    {
        Time.timeScale = 0;

        Global_Variables.gameFrozen = true;
    }

    public void Pause()
    {
        pausebutton.SetActive(false);
        scope.SetActive(false);
        pauseMenu.SetActive(true);

        gameFreeze();
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
        StateManager _manager = GameObject.Find("GameManager").GetComponent<StateManager>();
        _manager.restartLevel();
    }

    

    public void Exit()
    {
        resetTimeScale();
        SceneManager.LoadScene(0);


        //Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
