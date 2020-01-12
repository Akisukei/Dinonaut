using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject hotZone;
    [SerializeField] private GameObject first_lever;
    [SerializeField] private GameObject first_door;
    [SerializeField] private float t;
    [SerializeField] private float timeToReachPlayer;

    private CharacterMovement2D characterMovement2D;

    private float lastHeight = -9;
    private Vector3 hotZonePosition;

    void Awake()
    {
        characterMovement2D = player.GetComponent<CharacterMovement2D>();
    }

    // Use this for initialization
    void Start()
    {
        if (hotZone != null)
        {
            hotZonePosition = hotZone.transform.position;
        }
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (characterMovement2D.onGround)
        {
            if (lastHeight < player.transform.position.y && Mathf.Abs(player.transform.position.y - lastHeight) > 1)
            {
                // Player reached a higher ground.
                lastHeight = player.transform.position.y;
                //Debug.Log(lastHeight);
                if(hotZone != null)
                {
                    // Move hot zone up
                    hotZonePosition.y = lastHeight - 1.25f;
                    hotZone.transform.position = hotZonePosition;
                }
                
            }
        }
        if (first_lever != null && first_lever.GetComponent<Animator>().GetBool("leverFlipped"))
        {
            first_door.SetActive(false);
        }
    }
}
