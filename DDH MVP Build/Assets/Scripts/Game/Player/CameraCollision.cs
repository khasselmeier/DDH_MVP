/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Camera playerCamera;
    public Transform player;
    public float miniClipDistance = 0.1f; //min distance for the clipping plane

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward))
        {
            if (hit.collider.CompareTag("Wall")) //if theres a wall, adjust clipping plane
            {
                float distance = hit.distance;
                playerCamera.nearClipPlane = Mathf.Max(miniClipDistance, distance);
            }
        }
        else
        {
            playerCamera.nearClipPlane = miniClipDistance; //no wall hit, reset plane
        }
    }
}*/