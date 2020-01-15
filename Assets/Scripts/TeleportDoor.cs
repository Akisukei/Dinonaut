using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private Transform teleport;
    [SerializeField] private GameObject lever;
    private Collider2D player;
    private Animator animator;
    private bool isDoorUnlocked;
    private bool canUseDoor;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        isDoorUnlocked = false;
        canUseDoor = false;
    }
    
    void Update()
    {
        if (lever.GetComponent<Animator>().GetBool("leverFlipped"))
            isDoorUnlocked = true;
        else
            isDoorUnlocked = false;
        animator.SetBool("openDoor", isDoorUnlocked);

        if (isDoorUnlocked && canUseDoor && Input.GetButtonDown("Submit"))
            Teleport();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canUseDoor = true;
            player = other;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canUseDoor = false;
        }
    }
    private void Teleport()
    {
        player.transform.position = teleport.position;
    }
}
