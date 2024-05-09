using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Character player;

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Slider>().value = (player._healthC / player.HealthM) * 100;
    }
}
