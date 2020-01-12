using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private GameObject tilemap;
    [SerializeField] private GameObject floorTile;

    float t;
    Vector3 startPosition, startPosition2, startPosition3;
    Vector3 target;
    float timeToReachTarget;
  

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        startPosition = target = transform.position;
        startPosition2 = tilemap.transform.position;
        startPosition3 = floorTile.transform.position;

        timeToReachTarget = 3;
        target = new Vector3(26.4f, -40.49f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>().GetBool("leverFlipped") )
        {
            //&& moved == false
            t += Time.deltaTime / timeToReachTarget;

            Vector3 temp = new Vector3(0,-3,0);
            tilemap.transform.position = Vector3.Lerp(startPosition2, temp, t);
            floorTile.transform.position = Vector3.Lerp(startPosition3, temp, t);

            transform.position = Vector3.Lerp(startPosition, target, t);
        }
    }
}
