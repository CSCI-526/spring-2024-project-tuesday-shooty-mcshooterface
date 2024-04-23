using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    struct AudioSourceInfo
    {
        public string name;
        public AudioSource source;
    }

    public static MusicManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    List<AudioSourceInfo> audioList;

    Dictionary<string, AudioSourceInfo> audioDictionary = new Dictionary<string, AudioSourceInfo>();

    private static MusicManager instance;

    private AudioSource _audioSource;

    private void Awake()
    {
        foreach (AudioSourceInfo info in audioList)
        {
            audioDictionary.Add(info.name, info);
        }

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        _audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetMusicVolume(PlayerPrefs.GetFloat(SettingsMenu.MusicVolumeKey, 0.1f));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            audioDictionary["GameMusic"].source.Stop();
            audioDictionary["MenuMusic"].source.Play();
        }
        else if (scene.name == "ColinTest")
        {
            audioDictionary["MenuMusic"].source.Stop();
            audioDictionary["GameMusic"].source.Play();
        }
    }

    public void Play(string name)
    {
        if (audioDictionary.ContainsKey(name))
        {
            audioDictionary[name].source.Play();
        }
        else
            Debug.LogError("ERROR: Cannot find audio source of name " + name);
    }

    public void Stop(string name)
    {
        if (audioDictionary.ContainsKey(name))
        {
            audioDictionary[name].source.Stop();
        }
        else
            Debug.LogError("ERROR: Cannot find audio source of name " + name);
    }

    public void SetMusicVolume(float volume)
    {
        foreach (var audio in audioList)
        {
            audio.source.volume = volume;
        }
    }
}
