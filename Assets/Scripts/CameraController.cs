using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public GameObject player;
    public Transform player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
        Debug.Log("offseT:" + offset);
        Debug.Log("before cam:" + transform.position);
    }

    // LateUpdate is called after Update in each frame
    void LateUpdate()
    {

        //transform.position = transform.position + offset;
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        Debug.Log("cam:" + transform.position);
    }
}