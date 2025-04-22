using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public bool IsSFXOn { get; private set; }
    public bool IsMusicOn { get; private set; }

    [SerializeField] private AudioSource musicSource;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SettingManager.onSFXStateChanged += SFXStateChangedCallback;
        SettingManager.onMusicStateChanged += MusicStateChangedCallback;
    }

    private void OnDestroy()
    {
        SettingManager.onSFXStateChanged -= SFXStateChangedCallback;
        SettingManager.onMusicStateChanged -= MusicStateChangedCallback;
    }

    private void MusicStateChangedCallback(bool musicState)
    {
        IsMusicOn = musicState;
        if (musicState)
            musicSource.Play();
        else
            musicSource.Stop();
    }

    private void SFXStateChangedCallback(bool sfxState)
    {
        IsSFXOn = sfxState;
    }
}
