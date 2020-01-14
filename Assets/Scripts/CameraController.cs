using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float verticalDistance = 1.3f;
    public float horizontalDistance = 0f;
    
    // LateUpdate is called after Update in each frame
    void LateUpdate()
    {
        // anchor camera's position to player's
        transform.position = new Vector3(
            player.position.x + horizontalDistance, 
            player.position.y + verticalDistance, 
            transform.position.z
        );
    }

    public void cameraShake()
    {

    }
}