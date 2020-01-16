using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGameplay : MonoBehaviour
{
    public int score = 100;
    public Text scoreText;

    private bool inHotZone = false;

    private float nextActionTime = 0.0f;
    [SerializeField] private float period = 0.1f;
    [SerializeField] private int hotZoneDamage = 10;
    [SerializeField] private int lavaDamage = 50;

    private CharacterMovement2D characterMovement2D;

    private Vector3 respawnPosition;

    private Animator animator;

    // Called to init variables and set up components before Start()
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterMovement2D = GetComponent<CharacterMovement2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterMovement2D.onGround && !inHotZone)
        {
            respawnPosition = transform.position;
        }

        animator.SetBool("isDamaged", false);
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            // Run code below every 'period' seconds
            if (inHotZone)
            {
                TakeDamage(hotZoneDamage);
            }
        }

        scoreText.text = score.ToString();
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("HotZone"))
        {
            inHotZone = true;
        }

        if (collider.CompareTag("Lava"))
        {
            TakeDamage(lavaDamage);

            // Respawn. TODO: Add animation like 'blinking'
            Respawn();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("HotZone"))
        {
            inHotZone = false;
        }
    }

    void TakeDamage(int damage)
    {
        score -= damage;
        animator.SetBool("isDamaged", true);

        // If score reaches 0, it's game over
        if (score == 0)
            SceneManager.LoadScene("MenuScene");

    }

    void Respawn()
    {
        transform.position = respawnPosition;
    }
}
