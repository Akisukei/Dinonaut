using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActions : MonoBehaviour
{
    private Animator animator;
    private bool isKicking;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isKicking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
            isKicking = true;
        else
            isKicking = false;

        animator.SetBool("isKicking", isKicking);
    }
    void fixedUpdate()
    {
        
    }

}
