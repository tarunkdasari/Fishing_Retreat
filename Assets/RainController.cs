using UnityEngine;

public class RainController : MonoBehaviour
{
    [Header("Assign your rain ParticleSystem here")]
    public ParticleSystem rainSystem;

    [Header("Keyboard settings")]
    public KeyCode toggleKey = KeyCode.R;

    private bool isRaining = false;

    void Update()
    {
        // Toggle with keyboard key
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleRain();
        }
    }

    // This method can also be hooked to a UI Button
    public void ToggleRain()
    {
        if (isRaining)
        {
            rainSystem.Stop();
        }
        else
        {
            rainSystem.Play();
        }

        isRaining = !isRaining;
    }
}
