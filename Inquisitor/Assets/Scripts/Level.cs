using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Threading;

public class Level : MonoBehaviour
{
    [SerializeField] private float scale;
    [SerializeField] private float duration;    // in seconds
    [SerializeField] private GameObject[] spawns;   // points in space where enemies can spawn

    public float Duration { get { return duration; } }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, duration);

        // spawn enemies
        GameObject.Find("LevelController").SendMessage("SpawnEnemies", spawns);
    }

    private void Disappear()
    {
        transform.DOScale(0, duration);
    }

    
}
