using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private Transform teleport;
    [SerializeField] private GameObject lever;

    private Animator animator;
    private bool unlockDoor;
    private bool useDoor;

    void Start()
    {
        animator = GetComponent<Animator>();
        unlockDoor = false;
        useDoor = false;
    }
    
    void Update()
    {
        if (lever.GetComponent<Animator>().GetBool("leverFlipped"))
            unlockDoor = true;
        else
            unlockDoor = false;

        if (Input.GetButtonDown("Submit"))
            useDoor = true;
        else
            useDoor = false;

        animator.SetBool("openDoor", unlockDoor);

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (useDoor && unlockDoor)
            {
                other.transform.position = teleport.position;
            }
        }
    }
}
