using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KayakSeatTrigger : MonoBehaviour
{

    public GameObject xrOrigin;       // Your XR Rig
    public Transform seatPosition;    // Where the player should sit
    public KeyCode sitKey = KeyCode.G; // Key or button for sitting
    public InputHelpers.Button sitButton = InputHelpers.Button.PrimaryButton; // XR controller button
    private XRController leftController;
    private XRController rightController;

    private bool playerInZone = false;
    private bool isSeated = false;

    void Start()
    {
        // Try to find the controllers automatically
        leftController = GameObject.Find("LeftHand Controller")?.GetComponent<XRController>();
        rightController = GameObject.Find("RightHand Controller")?.GetComponent<XRController>();
    }



    void Update()
    {
        if (!playerInZone) return;

        bool pressed = CheckIfButtonPressed(leftController) || CheckIfButtonPressed(rightController) || Input.GetKeyDown(sitKey);

        if (pressed)
        {
            if (!isSeated)
            {
                SitOnKayak();
            }
            else
            {
                StandUp();
            }
        }
    }

    private bool CheckIfButtonPressed(XRController controller)
    {
        if (controller != null && controller.inputDevice.IsPressed(sitButton, out bool pressed))
            return pressed;
        return false;
    }

    private void SitOnKayak()
    {
        xrOrigin.transform.position = seatPosition.position;
        xrOrigin.transform.rotation = seatPosition.rotation;
        isSeated = true;
        Debug.Log("Sat on kayak!");
    }

    private void StandUp()
    {
        // Optionally move slightly up or back from kayak
        xrOrigin.transform.position += xrOrigin.transform.up * 0.5f;
        isSeated = false;
        Debug.Log("Stood up from kayak!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("Player near kayak.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera") || other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("Player left kayak area.");
        }
    }
}