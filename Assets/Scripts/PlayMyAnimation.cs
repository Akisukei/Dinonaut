using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMyAnimation : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;
    private bool leverFlipped;

    private void Awake()
    {
        leverFlipped = false;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Animator>().GetBool("isKicking"))
            {
                if (leverFlipped)
                    leverFlipped = false;
                else
                    leverFlipped = true;
                myAnimationController.SetBool("leverFlipped", leverFlipped);
            }
        }
    }
}
