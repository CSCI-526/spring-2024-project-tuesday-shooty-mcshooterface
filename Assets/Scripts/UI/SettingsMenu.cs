using System;
using Scripts.Game;
using Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public const string MouseSensitivityKey = "MouseSensitivity";
    public const string MusicVolumeKey = "MusicVolume";
    public const string SfxVolumeKey = "SfxVolume";
    public Slider mouseSensitivitySlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public GameObject settingsPanel;

    void Awake()
    {
        PlayerPrefs.SetFloat(MouseSensitivityKey, 0.5f); // set default
        PlayerPrefs.SetFloat(MusicVolumeKey, 0.1f); // set default
        mouseSensitivitySlider.value = PlayerPrefs.GetFloat(MouseSensitivityKey, 0.5f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SfxVolumeKey, 0.5f);

        mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivityChanged);
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);

        GameManager.Instance.AudioManager.SetSfxVolume(PlayerPrefs.GetFloat(SfxVolumeKey, 0.5f));
        PlayerCharacterController.Instance.SetSensitivity(
            PlayerPrefs.GetFloat(MouseSensitivityKey, 0.5f)
        );
        MusicManager.Instance.SetMusicVolume(PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f));
    }

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    private void OnSfxVolumeChanged(float sfxVolume)
    {
        PlayerPrefs.SetFloat(SfxVolumeKey, sfxVolume);
        GameManager.Instance.AudioManager.SetSfxVolume(PlayerPrefs.GetFloat(SfxVolumeKey, 0.5f));
    }

    private void OnMusicVolumeChanged(float musicVolume)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
        MusicManager.Instance.SetMusicVolume(musicVolume);
    }

    private void OnMouseSensitivityChanged(float mouseSensitivity)
    {
        PlayerPrefs.SetFloat(MouseSensitivityKey, mouseSensitivity);
        PlayerCharacterController.Instance.SetSensitivity(
            PlayerPrefs.GetFloat(MouseSensitivityKey, 0.5f)
        );
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}
