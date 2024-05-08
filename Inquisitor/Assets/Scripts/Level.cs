using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject gm; // reference to th egmae manager
    public void SetGm(GameObject g) { gm = g; }
    [SerializeField] private float scale;
    [SerializeField] private float duration;    // in seconds
    [SerializeField] private GameObject[] spawns;   // points in space where enemies can spawn
    [SerializeField] private GameObject[] enemies;  // posible types of enemies that can spawn
    private System.Random rand = new System.Random();

    [HideInInspector] public int nEnemies;  // number of enemies spawned/currently alive

    public float Duration { get { return duration; } }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, duration);

        // spawn enemies
        foreach(GameObject i in spawns){
            Instantiate(enemies[rand.Next(enemies.Length)], i.transform);
            nEnemies++;
        }
    }

    private void Disappear()
    {
        transform.DOScale(0, duration);
    }

    private void EnemyDead() { 
        nEnemies--;

        if(nEnemies == 0)
        {
            gm.SendMessage("StageComplete");
        }
        
    }
}
