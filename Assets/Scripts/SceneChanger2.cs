using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger2 : MonoBehaviour
{
    [SerializeField] private string newLevel;
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject lever2;

    private Animator animator;
    private bool unlockDoor;
    private bool useDoor;
    private bool prevState1;
    private bool prevState2;

    void Start()
    {
        animator = GetComponent<Animator>();
        unlockDoor = false;
        useDoor = false;
        prevState1 = false;
        prevState2 = false;
    }

    void Update()
    {
        if (lever.GetComponent<Animator>().GetBool("leverFlipped")) 
            unlockDoor = true;
        else
            unlockDoor = false;
        prevState1 = unlockDoor;

        if (lever2.GetComponent<Animator>().GetBool("leverFlipped"))
            unlockDoor = true;
        else
            unlockDoor = false;
        prevState2 = unlockDoor;

        if (Input.GetButtonDown("Submit"))
            useDoor = true;
        else
            useDoor = false;

        unlockDoor = prevState1 ^ prevState2;
        animator.SetBool("openDoor", unlockDoor);

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (useDoor && unlockDoor)
            {
                SceneManager.LoadScene(newLevel);
            }
        }
    }
}
