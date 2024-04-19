using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    const int levelSize = -20;

    public GameObject[] levels;

    private System.Random random = new System.Random();
    private int xPosMod = 0;
    [HideInInspector] public GameObject[] posibleLevels;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel(); // Create the first island

        for (int i = 0; i < 7; i++) { CreateLevel(); }
    }

    public void CreateLevel()
    {
        if(posibleLevels.Length == 0) { posibleLevels = levels; }

        // Select a random level
        index = random.Next(posibleLevels.Length);

        // GeneratedCodeAttribute random level
        Vector3 position = Vector3.zero;
        position.x = xPosMod;
        Instantiate(posibleLevels[index], position, posibleLevels[index].transform.rotation);

        // Get rid of generated level in array, to avoid generating the same level repeatedly
        posibleLevels = posibleLevels.Where((e, i) => i != index).ToArray();
        Debug.Log(posibleLevels.Length);

        //increase positon displacement for the next level generated
        xPosMod += levelSize;
    }
}
