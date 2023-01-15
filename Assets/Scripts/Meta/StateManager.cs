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
    public GameObject pauseMenu;
    public GameObject HUD;

    public bool didInitialMenuUpdate = false;

    public bool startedCurrentLevel = false;

    public AudioSource backgroundSound;

    private void Awake() {
        if (Instance != null && Instance != this) {
            // destroy the object which was using this script - the new GameManager using this attempt at a new instance of StateManager
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }

        // get the current scene's index for when we're testing and the manager wakes up in a room which isn't the menu
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        backgroundSound = GetComponent<AudioSource>();
        
    }


    public void Start() {
        DontDestroyOnLoad(gameObject);

        startLevelHelper();

        backgroundSound.volume = 0.8f;
    }

    public void Update()
    {
        if (!didInitialMenuUpdate)
        {
            didInitialMenuUpdate = true;
            //restartLevel();
        }
    }

    public void startNextLevel() {
        currentLevel += 1;
        if (currentLevel == SceneManager.sceneCountInBuildSettings-1)
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
    }

    private void startLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        startLevelHelper();
    }

    private bool isMenuScene(int levelIndex)
    {
        return (levelIndex == 0 || levelIndex == SceneManager.sceneCountInBuildSettings - 1);
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
        Global_Variables.ammo = 0;
        Global_Variables.life = Global_Variables.max_life;
        //int levelInd = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("In Helper scene index: " + currentLevel);
        HUD.SetActive(!isMenuScene(currentLevel));
        pauseButton.SetActive(!isMenuScene(currentLevel));
    }

    public void restartLevel()
    {
        startLevel(SceneManager.GetActiveScene().name);

    }

    private void hideGameplayMenus()
    {
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void gameFreeze()
    {
        Time.timeScale = 0;

        Global_Variables.gameFrozen = true;
    }

    public void finishLevel() {
        gameFreeze();
        winMenu.SetActive(true);
    }

    public void exitGame() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void resetTimeScale()
    {
        Time.timeScale = 1;
        Global_Variables.gameFrozen = false;
    }

    public void returnToMainMenu() {
        resetTimeScale();
        hideGameplayMenus();
        creditsMenu.SetActive(false);
        SceneManager.LoadScene(0);
    }
    
    public void setStateLoss() {
        gameFreeze();
        loseMenu.SetActive(true);
    }
}
