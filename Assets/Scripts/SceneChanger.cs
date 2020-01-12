using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string newLevel;
    [SerializeField] private Animator myAnimationController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (myAnimationController.GetBool("leverFlipped")))
        {
            SceneManager.LoadScene(newLevel);
        }
    }
}
