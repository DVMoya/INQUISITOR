using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public GameObject[] levels;

    private System.Random random = new System.Random();
    
    public GameObject gm;
    [HideInInspector] public GameObject[] posibleLevels;
    [SerializeField] private int xPosMod    = 0;
    [SerializeField] private int levelSize  = -20;
    [SerializeField] private int delay      = 10;
    private int index;

    private GameObject previouslyGenerated;
    private GameObject currentLevel;

    public void CreateLevel()
    {
        previouslyGenerated = currentLevel;

        if (posibleLevels.Length == 0) { posibleLevels = levels; }

        // Select a random level
        index = random.Next(posibleLevels.Length);

        // GeneratedCodeAttribute random level
        Vector3 position = Vector3.zero;
        position.x = xPosMod;
        currentLevel = Instantiate(posibleLevels[index], position, posibleLevels[index].transform.rotation);
        currentLevel.SetActive(true);
        currentLevel.SendMessage("SetGm", gm);

        // Get rid of generated level in array, to avoid generating the same level repeatedly
        posibleLevels = posibleLevels.Where((e, i) => i != index).ToArray();

        //increase positon displacement for the next level generated
        xPosMod += levelSize;

        // Get rid of the previous instance generated after a delay
        DestroyPreviousLevel(true);
    }

    public void DestroyPreviousLevel(bool wait)
    {
        if (wait)
        {
            StartCoroutine(DestroyPreviousLevel(previouslyGenerated, wait));
        }
        else
        {
            StartCoroutine(DestroyPreviousLevel(currentLevel, wait));
        }
    }

    IEnumerator DestroyPreviousLevel(GameObject destroyMe, bool wait)
    {
        if (destroyMe != null) {
            if(wait) { yield return new WaitForSeconds(delay); }    // give the player some time to get out of danger, only if he managed to kill all enemies
            destroyMe.SendMessage("Disappear");     // disappear animation

            yield return new WaitForSeconds(destroyMe.GetComponent<Level>().Duration);  // wait for the disapearing animation to finish
            Destroy(destroyMe);                                                         // destroy object in 4 seconds
        }
    }
}
