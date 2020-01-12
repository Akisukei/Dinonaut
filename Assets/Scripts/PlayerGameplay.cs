using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameplay : MonoBehaviour
{
    public int score;
    public Text scoreText;

    private bool inHotZone = false;

    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    private Animator animator;

    // Called to init variables and set up components before Start()
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 100;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isDamaged", false);
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            // Run code below every 'period' seconds
            if (inHotZone)
            {
                TakeDamage();
            }
        }

        scoreText.text = score.ToString();
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 11) // Layer 11 = HotZone
        {
            Debug.Log("COLLIDE WITH HOT ZONE");
            inHotZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 11)
        {
            inHotZone = false;
            animator.SetBool("isDamaged", false);
        }
    }

    void TakeDamage()
    {
        Debug.Log("Damage taken: " + score);
        score -= 10;
        animator.SetBool("isDamaged", true);
    }
}
