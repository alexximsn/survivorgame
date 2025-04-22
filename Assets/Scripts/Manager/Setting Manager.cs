using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Tabsil.Sijil;
using UnityEngine.Networking;
using System;
public class SettingManager : MonoBehaviour, IWantToBeSaved
{
    [Header(" Elements ")]
    [SerializeField] private Button sfxButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button privacyPolicyButton;

    [SerializeField] private Button creditsButton;
    [SerializeField] private GameObject creditsPanel;

    [Header(" Settings ")]
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    [Header(" Data ")]
    private bool sfxState;
    private bool musicState;

    [Header(" Actions ")]
    public static Action<bool> onSFXStateChanged;
    public static Action<bool> onMusicStateChanged;

    private void Awake()
    {
        sfxButton.onClick.RemoveAllListeners();
        sfxButton.onClick.AddListener(SFXButtonCallback);

        musicButton.onClick.RemoveAllListeners();
        musicButton.onClick.AddListener(MusicButtonCallback);

        privacyPolicyButton.onClick.RemoveAllListeners();
        privacyPolicyButton.onClick.AddListener(PrivacyPolicyButtonCallback);


        creditsButton.onClick.RemoveAllListeners();
        creditsButton.onClick.AddListener(CreditsButtonCallback);
    }



    // Start is called before the first frame update
    void Start()
    {
        HideCreditsPanel();

        onSFXStateChanged?.Invoke(sfxState);
        onMusicStateChanged?.Invoke(musicState);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void SFXButtonCallback()
    {
        sfxState = !sfxState;

        UpdateSFXVisuals();

        Save();

        // Trigger an action with the new sfx state
        onSFXStateChanged?.Invoke(sfxState);
    }

    private void UpdateSFXVisuals()
    {
        if (sfxState)
        {
            sfxButton.image.color = onColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            sfxButton.image.color = offColor;
            sfxButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }


    private void MusicButtonCallback()
    {
        musicState = !musicState;

        UpdateMusicVisuals();

        Save();

        // Trigger an action with the new music state
        onMusicStateChanged?.Invoke(musicState);
    }

    private void UpdateMusicVisuals()
    {
        if (musicState)
        {
            musicButton.image.color = onColor;
            musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "ON";
        }
        else
        {
            musicButton.image.color = offColor;
            musicButton.GetComponentInChildren<TextMeshProUGUI>().text = "OFF";
        }
    }

    private void PrivacyPolicyButtonCallback()
    {
        Application.OpenURL("https://www.bilibili.com");
    }

    public void CreditsButtonCallback()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }

    public void Load()
    {
        sfxState = true;
        musicState = true;

        if (Sijil.TryLoad(this, "sfx", out object sfxStateObject))
            sfxState = (bool)sfxStateObject;

        if (Sijil.TryLoad(this, "music", out object musicStateObject))
            musicState = (bool)musicStateObject;

        UpdateSFXVisuals();
        UpdateMusicVisuals();
        onMusicStateChanged?.Invoke(musicState);
    }

    public void Save()
    {
        Sijil.Save(this, "sfx", sfxState);
        Sijil.Save(this, "music", musicState);
    }
}
