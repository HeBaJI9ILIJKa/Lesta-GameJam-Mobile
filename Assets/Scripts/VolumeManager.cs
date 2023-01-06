using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioSource _music;
    [SerializeField] private Slider _volumeSlider;

    private void Awake()
    {
        LoadVolume();
    }

    private void OnEnable()
    {
        _volumeSlider.onValueChanged.AddListener((_volumeSliderListener) => {
            OnVolumeChanged(_volumeSlider.value);
        });
    }

    void Start()
    {
        _volumeSlider.value = GameParameters.Volume;
        _music.volume = GameParameters.Volume;
    }

    private void OnVolumeChanged(float volume)
    {
        GameParameters.Volume = volume;
        _music.volume = volume;
        SaveVolume();
    }

    private void LoadVolume()
    {
        GameParameters.Volume = PlayerPrefs.GetFloat("volume", 0.4f);
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("volume", GameParameters.Volume);
    } 
}
