using UnityEngine;
using Valve.VR; // Include SteamVR namespace

public class TeleportFloors : MonoBehaviour
{
    public SteamVR_Action_Boolean triggerAction; // SteamVR Trigger action
    public float fadeDuration = 1.0f; // Duration for fade effect
    public float floorHeight = 3.0f; // Height difference between floors

    private int triggerCount = 0;
    private float lastTriggerTime;
    private float doublePressTimeWindow = 0.5f; // Time window to detect multiple presses
    private int currentFloor = 0; // Track the current floor the player is on

    void Start()
    {
        // Ensure the trigger action is properly set
        if (triggerAction == null)
        {
            Debug.LogError("Trigger action not set! Please assign a SteamVR Boolean action.");
        }
    }

    void Update()
    {
        // SteamVR trigger detection
        if (triggerAction.GetStateDown(SteamVR_Input_Sources.Any))
        {
            HandleTriggerPress();
        }

        // Keyboard input detection for testing
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TeleportToFloor(1); // Teleport to first floor when '1' key is pressed
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TeleportToFloor(2); // Teleport to second floor when '2' key is pressed
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TeleportToFloor(3); // Teleport to third floor when '3' key is pressed
        }
    }

    void HandleTriggerPress()
    {
        float currentTime = Time.time;

        // Count the number of trigger presses within the defined time window
        if (currentTime - lastTriggerTime > doublePressTimeWindow)
        {
            triggerCount = 1; // Reset if too much time has passed since last press
        }
        else
        {
            triggerCount++; // Increment the count for consecutive presses
        }

        lastTriggerTime = currentTime;

        // Choose floor based on the number of trigger presses
        if (triggerCount == 1)
        {
            TeleportToFloor(1); // First floor
        }
        else if (triggerCount == 2)
        {
            TeleportToFloor(2); // Second floor
        }
        else if (triggerCount == 3)
        {
            TeleportToFloor(3); // Third floor
        }
    }

    void TeleportToFloor(int floorNumber)
    {
        // Start fading to black
        SteamVR_Fade.Start(Color.black, fadeDuration);

        // After fading out, teleport and fade back in
        Invoke(nameof(PerformTeleport), fadeDuration);

        // Set the target floor number
        currentFloor = floorNumber;
    }

    void PerformTeleport()
    {
        // Calculate new Y position based on floor height
        Vector3 newPosition = transform.position;
        newPosition.y = (currentFloor - 1) * floorHeight;

        // Apply the new Y position (teleport upwards to the selected floor)
        transform.position = newPosition;

        // Fade back in
        SteamVR_Fade.Start(Color.clear, fadeDuration);
    }
}
