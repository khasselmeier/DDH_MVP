using UnityEngine;
using UnityEngine.UI;

public class CrosshairToggle : MonoBehaviour
{
    public Image crosshair; // Reference to the crosshair Image
    public Toggle crosshairToggle; // Reference to the UI Toggle
    private bool isCrosshairVisible = true; // Crosshair is visible by default

    private void Start()
    {
        // Ensure the crosshair is enabled at the start
        if (crosshair != null)
        {
            crosshair.gameObject.SetActive(isCrosshairVisible);
        }

        // Set the initial state of the toggle to match the crosshair visibility
        if (crosshairToggle != null)
        {
            crosshairToggle.isOn = isCrosshairVisible;
            crosshairToggle.onValueChanged.AddListener(OnToggleValueChanged); // Add listener for toggle changes
        }
    }

    // This method is called whenever the toggle value changes
    private void OnToggleValueChanged(bool isOn)
    {
        isCrosshairVisible = isOn; // Set visibility based on toggle state
        if (crosshair != null)
        {
            crosshair.gameObject.SetActive(isCrosshairVisible); // Set the crosshair visibility
            //Debug.Log($"Crosshair visibility: {isCrosshairVisible}"); // Debug message
        }
    }
}
