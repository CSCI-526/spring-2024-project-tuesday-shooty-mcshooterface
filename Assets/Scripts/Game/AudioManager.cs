using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    struct AudioSourceInfo
    {
        public string name;
        public AudioSource source;
    }

    [SerializeField]
    List<AudioSourceInfo> audioList;

    Dictionary<string, AudioSourceInfo> audioDictionary = new Dictionary<string, AudioSourceInfo>();

    private void Awake()
    {
        foreach (AudioSourceInfo info in audioList)
        {
            audioDictionary.Add(info.name, info);
        }
        SetSfxVolume(PlayerPrefs.GetFloat(SettingsMenu.SfxVolumeKey, 0.5f));
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

    public void SetSfxVolume(float volume)
    {
        foreach (var audio in audioList)
        {
            audio.source.volume = volume * PlayerPrefs.GetFloat(SettingsMenu.SfxVolumeKey, 0f);
        }
    }
}
