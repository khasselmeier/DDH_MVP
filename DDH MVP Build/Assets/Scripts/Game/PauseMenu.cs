using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    private CameraController cameraController;

    void Start()
    {
        // Find the camera with the "Main Camera" tag and get the CameraController script
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");

        if (mainCamera != null)
        {
            cameraController = mainCamera.GetComponent<CameraController>();

            /*if (cameraController == null)
            {
                Debug.Log("CameraController script not found on the Main Camera.");
            }*/
        }
    }

    void Update()
    {
        if (cameraController == null)
        {
            GameObject mainCamera = GameObject.FindWithTag("MainCamera"); // Try to find the camera again
            if (mainCamera != null)
            {
                cameraController = mainCamera.GetComponent<CameraController>();
                /*if (cameraController == null)
                {
                    Debug.Log("CameraController script not found on the Main Camera.");
                }*/
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        //re-enable camera movement
        if (cameraController != null)
        {
            cameraController.ToggleCameraMovement(true);
        }

        // hide and lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        // disable camera movement
        if (cameraController != null)
        {
            cameraController.ToggleCameraMovement(false);
        }

        // show and unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
