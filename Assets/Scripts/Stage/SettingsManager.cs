using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    #region dont touch this
    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("SettingsManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion
    
    #region Variables
    //AUDIO
    [Header("<b><size=15>Audio<b><size=15>")] 
    [Line(255,255,255)]
    [Header("<b><size=15>Volume<b><size=15>")]
    [SerializeField] private AudioMixer masterAudioMixer;
    [Space(10)]
    [Range(0.0f, 1.0f)] 
    public float masterVolume = 0.7f;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [Space(10)]
    [Range(0.0f, 1.0f)] 
    public float musicVolume = 1.0f;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [Space(10)]
    [Range(0.0f, 1.0f)] 
    public float metronomeVolume = 1.0f;
    [SerializeField] private Slider metronomeVolumeSlider;
    [SerializeField] private TextMeshProUGUI metronomeVolumeText;
    public AudioSource metronomeTestSound;
    [Space(10)]
    [Range(0.0f, 1.0f)] 
    public float sfxVolume = 1.0f;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    public AudioSource sfxTestSound;
    [Space(10)]
    [Range(0.0f, 1.0f)] 
    public float hitSfxVolume = 1.0f;
    [SerializeField] private Slider hitSfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI hitSfxVolumeText;
    public AudioSource hitSfxTestSound;
    [Space(10)]
    [Range(0.0f, 1.0f)] 
    public float dialogueSfxVolume = 1.0f;
    [SerializeField] private Slider dialogueSfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI dialogueSfxVolumeText;
    public AudioSource dialogueSfxTestSound;
    
    //DISPLAY
    [Space(20)][Header("<b><size=15>Display<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] private FullScreenMode fullScreenMode;
    [SerializeField] private Vector2 resolution = new Vector2(1280, 720);

    //GAMEPLAY
    [Space(20)][Header("<b><size=15>Gameplay<b><size=15>")] 
    [Line(255,255,255)]
    public bool isGridWraparound = false;
    [Space(10)]
    [SerializeField] private Slider indicatorSpeedSlider;
    [SerializeField] private TextMeshProUGUI indicatorSpeedText;
    [Space(10)]
    public bool isDynamicMusicActive = true;
    [Space(10)][Header("<b><size=15>Latency<b><size=15>")]
    [SerializeField] private Slider audioLatencySlider;
    [SerializeField] private TextMeshProUGUI audioLatencyText;
    [Space(10)]
    [SerializeField] private Slider inputLatencySlider;
    [SerializeField] private TextMeshProUGUI inputLatencyText;

    //DIALOGUE
    [Space(20)][Header("<b><size=15>Dialogue<b><size=15>")] 
    [Line(255,255,255)]
    public float textSpeed = 0.05f;
    [SerializeField] private List<float> textSpeeds = new List<float>();
    public bool isAutoAdvanceText = false; //not implemented
    public bool isAlwaysPlayDialogue = false; //not implemented

    //CONTROLS
    /*
    [Space(20)][Header("<b><size=15>Controls<b><size=15>")] 
    [Line(255,255,255)]

    //ACCESSIBILITY
    [Space(20)][Header("<b><size=15>Accessibility<b><size=15>")] 
    [Line(255,255,255)]
    */

    //SCREENS
    [Space(20)][Header("<b><size=15>Screens<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] private GameObject settingsSelection;
    [SerializeField] private GameObject gameSettings;
    [SerializeField] private GameObject soundSettings;
    [SerializeField] private GameObject displaySettings;
    [SerializeField] private GameObject constrolsSettings;
    #endregion

    #region OnEnable
    private void OnEnable()
    {
        settingsSelection.SetActive(true);
        gameSettings.SetActive(false);
        soundSettings.SetActive(false);
        displaySettings.SetActive(false);
        constrolsSettings.SetActive(false);

        masterVolumeSlider.value = masterVolume;
    }
    #endregion

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        fullScreenMode = FullScreenMode.Windowed;
        resolution = new Vector2(1280, 720);
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        Latency();
        //SetGameResolution(resolution);
    }
    #endregion

    #region  Volume
    //Audio Setting Function
    public void SetVolume(int sliderNum)
    {
        switch (sliderNum)
        {
            case 0: //Master Volume
                masterAudioMixer.SetFloat("_masterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
                masterVolumeText.text = Mathf.RoundToInt(masterVolumeSlider.value * 100) + "%";
                masterVolume = masterVolumeSlider.value;
                break;
            case 1: //BGM Volume
                masterAudioMixer.SetFloat("_BGMVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
                musicVolumeText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100) + "%";
                musicVolume = musicVolumeSlider.value;
                break;
            case 2: //Metronome Volume
                masterAudioMixer.SetFloat("_metronomeVolume", Mathf.Log10(metronomeVolumeSlider.value) * 20);
                metronomeVolumeText.text = Mathf.RoundToInt(metronomeVolumeSlider.value * 100) + "%";
                metronomeVolume = metronomeVolumeSlider.value;
                metronomeTestSound.Play();
                break;
            case 3: //Other SFX Volume
                masterAudioMixer.SetFloat("_SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);
                sfxVolumeText.text = Mathf.RoundToInt(sfxVolumeSlider.value * 100) + "%";
                sfxVolume = sfxVolumeSlider.value;
                sfxTestSound.Play();
                break;
            case 4: //Hit SFX Volume
                masterAudioMixer.SetFloat("_hitSFXVolume", Mathf.Log10(hitSfxVolumeSlider.value) * 20);
                hitSfxVolumeText.text = Mathf.RoundToInt(hitSfxVolumeSlider.value * 100) + "%";
                hitSfxVolume = hitSfxVolumeSlider.value;
                hitSfxTestSound.Play();
                break;
            case 5: //Dialogue SFX Volume
                masterAudioMixer.SetFloat("_dialogueSFXVolume", Mathf.Log10(dialogueSfxVolumeSlider.value) * 20);
                dialogueSfxVolumeText.text = Mathf.RoundToInt(dialogueSfxVolumeSlider.value * 100) + "%";
                dialogueSfxVolume = dialogueSfxVolumeSlider.value;
                dialogueSfxTestSound.Play();
                break;
            default:
                break;
        } 
    }
    #endregion

    #region Latency
    void Latency()
    {
        GameManager.Instance.audioOffset = audioLatencySlider.value / 100;
        audioLatencyText.text = (audioLatencySlider.value).ToString() + "ms";

        GameManager.Instance.inputOffset = inputLatencySlider.value / 100;
        inputLatencyText.text = (inputLatencySlider.value).ToString() + "ms";
    }
    #endregion

    #region Text
    //Text Speed function
    public void HandleTextSpeedData(int num)
    {
        switch (num)
        {
            //Fast
            case 0:
                textSpeed = textSpeeds[0]; //0.01f
                break;
            //Medium
            case 1:
                textSpeed = textSpeeds[1]; //0.05f
                break;
            //Slow
            case 2:
                textSpeed = textSpeeds[2]; //0.001f
                break;

            default:
                break;
        }
    }
    #endregion

    #region Display
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
    #endregion
}
