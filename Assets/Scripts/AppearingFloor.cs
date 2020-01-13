using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingFloor : MonoBehaviour
{
    private GameObject pause;
    [SerializeField] private GameObject tilemap;
    [SerializeField] private GameObject tilewall;

    void Start()
    {
        pause = tilemap;
    }
    void Update()
    {
        if (GetComponent<Animator>().GetBool("leverFlipped"))
        {
            tilemap.SetActive(true);

            if (tilewall != null)
                tilewall.SetActive(false);
        }
        else
        {
            tilemap.SetActive(false);
        }
    }
}
