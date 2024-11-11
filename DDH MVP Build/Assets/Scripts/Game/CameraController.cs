using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Look Sensitivity")]
    public float sensX = 500f;
    public float sensY = 500f;

    [Header("Clamping")]
    public float minY = -50f;
    public float maxY = 50f;

    private float rotX;
    private float rotY;

    private bool canMove = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Ensure the cursor is hidden
    }

    void LateUpdate()
    {
        if (!canMove) return;

        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        rotX += mouseX;
        rotY -= mouseY;

        rotY = Mathf.Clamp(rotY, minY, maxY);

        transform.localRotation = Quaternion.Euler(rotY, 0, 0);
        transform.parent.rotation = Quaternion.Euler(0, rotX, 0);
    }

    public void SetMouseSensitivity(float newSensX, float? newSensY = null)
    {
        sensX = newSensX; // Directly set the X sensitivity
        sensY = newSensY.HasValue ? newSensY.Value : sensX; // Use provided Y or default to X

        //Debug.Log($"Sensitivity updated to X: {sensX}, Y: {sensY}");
    }

    public void ToggleCameraMovement(bool isActive)
    {
        canMove = isActive;
    }
}