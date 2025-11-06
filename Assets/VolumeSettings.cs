
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;

    public void SetMusicVolume()
    {
        float volume = slider.value;
        mixer.SetFloat("master", Mathf.Log10(volume)*20);
    }
}
