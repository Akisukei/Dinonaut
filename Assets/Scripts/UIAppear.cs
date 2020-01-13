using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAppear : MonoBehaviour
{
    private GameObject pause;
    [SerializeField] private GameObject uiObject;
    // Start is called before the first frame update
    void Start()
    {
        pause = uiObject;
        uiObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            uiObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            uiObject.SetActive(false);
        }
    }
}
