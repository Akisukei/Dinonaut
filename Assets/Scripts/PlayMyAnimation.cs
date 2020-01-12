using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMyAnimation : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;
    private bool wasAlreadyFlipped;

    private void Awake()
    {
        wasAlreadyFlipped = false;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Animator>().GetBool("isKicking"))
            {
                myAnimationController.SetBool("leverFlipped", true);

                if (wasAlreadyFlipped)
                    wasAlreadyFlipped = false;
                else
                    wasAlreadyFlipped = true;

                myAnimationController.SetBool("wasAlreadyFlipped", wasAlreadyFlipped);
            }
        }
    }
}
