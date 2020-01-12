using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingFloor : MonoBehaviour
{
    [SerializeField] private Vector3 target;
    [SerializeField] private float timeToReachTarget;

    float t;
    Vector3 startPosition;
    Vector3 tempPosition;

    bool isOnPlatform;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        startPosition = transform.position;

        timeToReachTarget = 3;

        isOnPlatform = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnPlatform)
        {
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, target, t);
            tempPosition = transform.position;
        }
        else if (!isOnPlatform && startPosition != transform.position)
        {
            //t += Time.deltaTime / timeToReachTarget;
            //transform.position = Vector3.Lerp(tempPosition, startPosition, t);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(transform);
            isOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.SetParent(null);
            isOnPlatform = false;
        }
    }
}
