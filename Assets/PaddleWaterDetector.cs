using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleWaterDetector : MonoBehaviour
{

    private bool isInWater = false;
    public float depth = 0f; // how far underwater the paddle is

    public float waterHeight = 0f; // set from PaddleMovement

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;

            // depth = water surface - paddle point height
            depth = Mathf.Max(0f, waterHeight - transform.position.y);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            depth = 0f;
        }
    }

    public bool IsInWater()
    {
        return isInWater;
    }
}
