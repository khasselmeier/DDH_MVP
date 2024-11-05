using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivityManager : MonoBehaviour
{
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText;
    private float mouseSensitivity = 200f;

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 200f);
        sensitivitySlider.value = mouseSensitivity;
        UpdateSensitivityText();
        ApplySensitivity(); // Apply the loaded sensitivity
    }

    public void OnSensitivitySliderChanged(float value)
    {
        //Debug.Log($"Slider value changed: {value}"); // Debugging line
        mouseSensitivity = value; // Update sensitivity based on slider
        ApplySensitivity();
        PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity); // Save new sensitivity value
        UpdateSensitivityText();
    }

    private void ApplySensitivity()
    {
        CameraController cameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            cameraController.SetMouseSensitivity(mouseSensitivity); // Pass the sensitivity value
        }
    }

    private void UpdateSensitivityText()
    {
        if (sensitivityText != null)
        {
            sensitivityText.text = $"Sens: {mouseSensitivity:F2}"; // Update text display
        }
    }
}