using UnityEngine;

public class NameTag : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        FindCamera();
    }

    void Update()
    {
        // if the camera is not found, keep trying to find it
        if (cameraTransform == null)
        {
            FindCamera();
        }
    }

    void LateUpdate()
    {
        // find the camera before making the name tag face it
        if (cameraTransform != null)
        {
            transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
        }
    }

    void FindCamera()
    {
        // find the camera
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
            //Debug.Log("Camera found for Nametag");
        }
    }
}