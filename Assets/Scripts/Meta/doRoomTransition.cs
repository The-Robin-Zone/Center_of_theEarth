using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doRoomTransition : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player")) 
        {
            // get the gamemanager and make it restart the game
            FindObjectOfType<StateManager>().finishLevel();

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
