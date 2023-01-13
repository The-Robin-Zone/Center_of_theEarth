using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonHandle : MonoBehaviour
{
    int currentLevel = 0;

    public void startNextLevel() {
        StateManager _manager = FindObjectOfType<StateManager>();
        _manager.currentLevel = 0;
        _manager.startNextLevel();
    }

    //public void startLevel(string sceneName) {
    //    FindObjectOfType<StateManager>().startLevel(sceneName);
    //}

    public void exitGame()
    {
        FindObjectOfType<StateManager>().exitGame();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
