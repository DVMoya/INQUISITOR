using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class Level : MonoBehaviour
{
    [SerializeField] private float scale;
    [SerializeField] private float duration;    // in seconds

    public float Duration { get { return duration; } }

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, duration);
    }

    private void Disappear()
    {
        transform.DOScale(0, duration);
    }
}
