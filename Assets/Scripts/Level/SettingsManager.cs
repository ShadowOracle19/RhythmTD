using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")] 
    [SerializeField] private AudioMixer masterAudioMixer;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;

    [Header("Latency")]
    [SerializeField] private Slider audioLatencySlider;
    [SerializeField] private TextMeshProUGUI audioLatencyText;

    [SerializeField] private Slider inputLatencySlider;
    [SerializeField] private TextMeshProUGUI inputLatencyText;

    [Header("display")]
    [SerializeField] private FullScreenMode fullScreenMode;
    [SerializeField] private Vector2 resolution = new Vector2(1280, 720);

    [Header("Screens")]
    [SerializeField] private GameObject settingsSelection;
    [SerializeField] private GameObject gameSettings;
    [SerializeField] private GameObject soundSettings;
    [SerializeField] private GameObject displaySettings;
    [SerializeField] private GameObject constrolsSettings;


    private void OnEnable()
    {
        settingsSelection.SetActive(true);
        gameSettings.SetActive(false);
        soundSettings.SetActive(false);
        displaySettings.SetActive(false);
        constrolsSettings.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        fullScreenMode = FullScreenMode.Windowed;
        resolution = new Vector2(1280, 720);
    }

    // Update is called once per frame
    void Update()
    {
        Volume();
        Latency();
        //SetGameResolution(resolution);
    }

    //Audio Setting Function
    void Volume()
    {
        masterAudioMixer.SetFloat("_masterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
        masterVolumeText.text = Mathf.RoundToInt(masterVolumeSlider.value * 100) + "%";

        masterAudioMixer.SetFloat("_musicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
        musicVolumeText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100) + "%";

        masterAudioMixer.SetFloat("_SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);
        float sfxVolume = Mathf.RoundToInt(sfxVolumeSlider.value * 100);
        sfxVolumeText.text = sfxVolume + "%";
        

    }

    void Latency()
    {
        GameManager.Instance.audioOffset = audioLatencySlider.value / 100;
        audioLatencyText.text = (audioLatencySlider.value).ToString() + "ms";

        GameManager.Instance.inputOffset = inputLatencySlider.value / 100;
        inputLatencyText.text = (inputLatencySlider.value).ToString() + "ms";
    }

    //Text Speed function
    public void HandleTextSpeedData(int num)
    {
        switch (num)
        {
            //Medium
            case 0:
                GameManager.Instance.textSpeed = 0.01f;
                break;
            //Slow
            case 1:
                GameManager.Instance.textSpeed = 0.05f;
                break;
            //Fast
            case 2:
                GameManager.Instance.textSpeed = 0.001f;
                break;

            default:
                break;
        }
    }

    public void HandleScreenMode(int num)
    {
        switch (num)
        {
            //Windowed
            case 0:
                fullScreenMode = FullScreenMode.Windowed;
                
                SetGameResolution();
                break;
            //Borderless Windowed
            case 1:
                fullScreenMode = FullScreenMode.FullScreenWindow;
                SetGameResolution();
                break;
            //Fullscreen
            case 2:
                fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                SetGameResolution();
                break;
            default:
                break;
        }
    }

    //Video Setting functions
    public void HandleResolutionDropdownData(int num)
    {
        //1920x1080
        if(num == 1)
        {
            resolution = new Vector2(1920, 1080);
        }
        //1280x720
        else if (num == 0)
        {
            resolution = new Vector2(1280, 720);
        }
        //800x600
        else if (num == 2)
        {
            resolution = new Vector2(960, 540);
        }
        SetGameResolution();
    }

    void SetGameResolution()
    {
        Cursor.visible = false;
        Screen.SetResolution((int)resolution.x, (int)resolution.y, fullScreenMode);
    }
}
