using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem; // for InputAction access

public class ControllerTracker : MonoBehaviour
{
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public Text debugText; // Assign a Text UI element in the Canvas

    private Vector3 lastLeftPos, lastRightPos;
    private Vector3 leftVel, rightVel;

    void Update()
    {
        if (!debugText) return;

        if (leftController)
        {
            Vector3 leftPos = leftController.positionAction.action.ReadValue<Vector3>();
            Quaternion leftRot = leftController.rotationAction.action.ReadValue<Quaternion>();
            leftVel = (leftPos - lastLeftPos) / Time.deltaTime;
            lastLeftPos = leftPos;

            debugText.text =
                $"Left\nPos: {leftPos}\nRot: {leftRot.eulerAngles}\nVel: {leftVel}";
        }

        if (rightController)
        {
            Vector3 rightPos = rightController.positionAction.action.ReadValue<Vector3>();
            Quaternion rightRot = rightController.rotationAction.action.ReadValue<Quaternion>();
            rightVel = (rightPos - lastRightPos) / Time.deltaTime;
            lastRightPos = rightPos;

            debugText.text +=
                $"\n\nRight\nPos: {rightPos}\nRot: {rightRot.eulerAngles}\nVel: {rightVel}";
        }
    }
}