using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem
{
    #region Fields
    private readonly AudioMixer _audioMixer;
    private const string MASTER = "Master";
    #endregion

    public AudioSystem(AudioMixer audioMixer)
    {
        _audioMixer = audioMixer;
    }

    #region Public API
    public void SetMasterVolumeFromSlider(float value)
    {
        _audioMixer.SetFloat(MASTER, Mathf.Log10(value) * 20.0f);
    }

    public void SetMasterVolume(float value)
    {
        _audioMixer.SetFloat(MASTER, value);
    }

    public float GetMasterVolume()
    {
        _audioMixer.GetFloat(MASTER, out float value);
        return value;
    }
    #endregion
}
