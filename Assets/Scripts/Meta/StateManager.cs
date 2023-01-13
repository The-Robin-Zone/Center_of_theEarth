using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }
    public int currentLevel = 0;

    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject creditsMenu;
    public GameObject pauseButton;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }


    public void Start() {
        DontDestroyOnLoad(gameObject);
    }

    public void startNextLevel() {
        currentLevel += 1;
        if (currentLevel+1 == SceneManager.sceneCountInBuildSettings)
        {
            creditsMenu.SetActive(true);
        }
        startLevel(currentLevel);
    }

    public void startCurrentLevel()
    {
        startLevel(currentLevel);
    }

    private void startLevel(int levelIndex) {
        SceneManager.LoadScene(levelIndex);
        startLevelHelper();

        Global_Variables.ammo = 0;
    }

    private void startLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        startLevelHelper();
    }

    /// <summary>
    /// @description
    /// A private function which handles semantics which all versions of startLevel should perform
    /// </summary>
    private void startLevelHelper()
    {
        resetTimeScale();

        // hide any menus - we're playing the level now
        hideGameplayMenus();
    }

    public void restartLevel()
    {
        startLevel(SceneManager.GetActiveScene().name);

    }

    private void hideGameplayMenus()
    {
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
    }


    public void finishLevel() {
        Time.timeScale = 0;
        winMenu.SetActive(true);
    }

    public void exitGame() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void resetTimeScale()
    {
        Time.timeScale = 1;
    }

    public void returnToMainMenu() {
        resetTimeScale();
        hideGameplayMenus();
        creditsMenu.SetActive(false);
        Debug.Log("DID IT!");
        SceneManager.LoadScene(0);
    }
    
    public void setStateLoss() {
        Time.timeScale = 0;
        loseMenu.SetActive(true);
    }
}