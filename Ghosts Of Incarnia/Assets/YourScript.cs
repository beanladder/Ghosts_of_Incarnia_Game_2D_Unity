using UnityEngine;
using UnityEngine.UI;

public class YourScript : MonoBehaviour
{
    public Button yourButton; // Reference to your UI button

    void Start()
    {
        // Call this wherever you want to simulate the button click
        SimulateButtonClick();
    }

    void SimulateButtonClick()
    {
        // Check if the button reference is not null
        if (yourButton != null)
        {
            // Invoke the onClick event to simulate the button click
            yourButton.onClick.Invoke();
        }
        else
        {
            Debug.LogError("Button reference is null. Assign the button in the Unity Editor.");
        }
    }

    // Rest of your script...
}
