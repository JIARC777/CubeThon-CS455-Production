using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Grab a reference to the player's position
    public Transform playerTransform;
    // offset parameter for camera - set in inspector
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        // snap camera position to offset of player's position
        transform.position = playerTransform.position + offset;
    }
}
