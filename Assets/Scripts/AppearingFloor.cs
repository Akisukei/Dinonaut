using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingFloor : MonoBehaviour
{
    private GameObject pause;
    [SerializeField] private GameObject tilemap;

    void Start()
    {
        pause = tilemap;
    }
    void Update()
    {
        if (GetComponent<Animator>().GetBool("leverFlipped"))
        {
            tilemap.SetActive(true);
        }
        else
        {
            tilemap.SetActive(false);
        }
    }
}
