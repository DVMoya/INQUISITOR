using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject LevelController;

    void Start()
    {
        LevelController.SendMessage("CreateLevel");
    }

    private void Update()
    {

        /*
         This code must be romved, I'm only using it to test
         the level genration. Well, it doesn't relly have to be 
         removed, just change the condition looking for a key 
         input for something else.
        */
        if (Input.GetKeyDown(KeyCode.P)) {
            LevelController.SendMessage("CreateLevel");
        }
    }
}
