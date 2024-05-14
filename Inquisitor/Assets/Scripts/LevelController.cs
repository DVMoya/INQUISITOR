using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;

    public GameObject[] levels;

    private System.Random random = new System.Random();
    
    private GameObject gm;

    private NavMeshSurface surface;

    [HideInInspector] public GameObject[] posibleLevels;
    [HideInInspector] public int nEnemies = 0;          // number of enemies spawned/currently alive
    [SerializeField] private GameObject[] enemies;  // posible types of enemies that can spawn
    [SerializeField] private int xPosMod    = 0;
    [SerializeField] private int levelSize  = -20;
    [SerializeField] private int delay      = 10;
    private int index;

    private GameObject previouslyGenerated;
    private GameObject currentLevel;

    private System.Random rand = new System.Random();

    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
        gm = GameObject.Find("GameManager");
    }

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

        // Put the controller in the new island before generating the nav mesh
        transform.position = currentLevel.transform.position;

        // Clear and recreate nav mesh
        surface.BuildNavMesh();

        // Get rid of generated level in array, to avoid generating the same level repeatedly
        posibleLevels = posibleLevels.Where((e, i) => i != index).ToArray();

        //increase positon displacement for the next level generated
        xPosMod += levelSize;

        // Get rid of the previous instance generated after a delay
        DestroyPreviousLevel(true);
    }

    public IEnumerator SpawnEnemies(GameObject[] spawns)
    {
        yield return new WaitForSeconds(.1f);

        foreach (GameObject i in spawns)
        {
            Instantiate(enemies[rand.Next(enemies.Length)], i.transform);
            nEnemies++;
        }
    }

    private void EnemyDead()
    {
        nEnemies--;

        if (nEnemies == 0)
        {
            gm.SendMessage("StageComplete");
        }
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
