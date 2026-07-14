using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConductorV2 : MonoBehaviour
{
    //Conductor instance
    public static ConductorV2 instance;
    void Awake()
    {
        instance = this;
    }

    public bool isInTestingEnvironment = false;

    public float bpm = 160;//song beats per minute
    public float crotchet;//Gives the time duration of a beat, calculated from the bpm
    public int songLoops = 0;
    public float songPosition;
    public float songPositionInBeats;//current song position in beats
    public float dspSongTime;//how many seconds have passed since the song started
    public AudioSource musicSource;

    //The number of beats in each loop
    public float beatsPerLoop;

    //the total number of loops (measures) completed since the looping clip first started
    /*
    public int completedLoops = 0;

    //The current position of the song within the loop in beats.
    public float loopPositionInBeats;

    //The current relative position of the song within the loop measured between 0 and 1.
    public float loopPositionInAnalog;
    */


    //Beat Thresholds
    //Note:
    //When checking beat threshold check late, miss, early, great, and then perfect
    [Header ("Beat Judgement Thresholds")]
    public float lateGreatBeatThreshold = 0.125f;
    public float lateBeatThreshold = 0.250f;  
    public float missBeatThreshold = 0.375f;
    public float earlyBeatThreshold = 0.625f;
    public float earlyGreatBeatThreshold = 0.750f;
    public float perfectBeatThreshold = 0.875f;
    public float maxBeatThreshold = 0.500f;

    [Header ("Beat Tracking")]
    public float beatDuration;
    public int numberOfBeats;

    public int beatTrack;

    public int measureTrack;

    public float _interval;

    public int lastInterval;

    //Dynamic Music tracks
    public AudioSource flats;
    public AudioSource major;
    public AudioSource allegro;
    public AudioSource trill;

    public AudioSource chromatic;
    public AudioSource poco;
    public AudioSource legato;
    public AudioSource forte;

    public AudioSource Tower9;
    public AudioSource Tower10;
    public AudioSource Tower11;
    public AudioSource Tower12;

    public AudioSource _ping;

    [Header("Events")]
    public List<UnityEvent> triggerEvent = new List<UnityEvent>();
    public List<UnityEvent> projectileEvent = new List<UnityEvent>();
    public List<UnityEvent> enemyEvent = new List<UnityEvent>();
    public List<UnityEvent> pickupEvent = new List<UnityEvent>(); 
    public List<UnityEvent> waveEvent = new List<UnityEvent>(); 

    public bool pauseConductor = false;
    public TextMeshProUGUI countInText;
    public bool countingIn = false;

    bool perfectBeatReset = false;

    
    public void CountUsIn(int _bpm)
    {
        pauseConductor = true;
        bpm = _bpm;
        songLoops = 0;
        //completedLoops = 0;
        numberOfBeats = 0;
        beatTrack = 0;
        measureTrack = 0;
        beatDuration = 0;
        countingIn = true;
        
        crotchet = 60 / bpm; //calculate the number of seconds in each beat

        DynamicSongInit(GameManager.Instance.currentEncounter.combatEncounter.dynamicSong);
        StartCoroutine(CountIn());
    }

    IEnumerator CountIn()
    {
        countInText.gameObject.SetActive(true);

        for (int i = 1; i <= 4; i++)
        {
            countInText.text = i.ToString();

            _ping.Play();

            yield return new WaitForSecondsRealtime(crotchet);
        }

        StartConductor();

        _ping.Play();

        yield return null;
    }

    public void StartConductor()
    {
        countingIn = false;
        //CombatManager.Instance.knockEmDead.SetActive(true);
        //CombatManager.Instance.knockEmDead.GetComponent<Animator>().SetTrigger("KnockEmDead");

        pauseConductor = false;
        countInText.gameObject.SetActive(false);

        //completedLoops = 0;
        numberOfBeats = 0;
        beatTrack = 1;
        beatDuration = 0;

        flats.time = 0;
        major.time = 0;
        allegro.time = 0;
        trill.time = 0;

        chromatic.time = 0;
        poco.time = 0;
        forte.time = 0;
        legato.time = 0;

        Tower9.time = 0;
        Tower10.time = 0;
        Tower11.time = 0;
        Tower12.time = 0;   

        if (GameManager.Instance.tutorialRunning)
        {
            //CombatManager.Instance.metronome.SetActive(true);
            CursorTD.Instance.movementSequence = true;
        }

        //Start the song
        //musicSource.Play();
    }

    public void DynamicSongInit(DynamicSongCreator song)
    {
        flats.volume = 0;
        major.volume = 0;
        allegro.volume = 0;
        trill.volume = 0;
        chromatic.volume = 0;
        poco.volume = 0;
        forte.volume = 0;
        legato.volume = 0;
        Tower9.volume = 0;
        Tower10.volume = 0; 
        Tower11.volume = 0; 
        Tower12.volume = 0; 


        flats.clip = song.flats;
        major.clip = song.major;
        allegro.clip = song.allegro;
        trill.clip = song.trill;
        chromatic.clip = song.chromatic;
        poco.clip = song.poco;
        forte.clip = song.forte;
        legato.clip = song.legato;
        Tower9.clip = song.tower9;
        Tower10.clip = song.tower10;
        Tower11.clip = song.tower11;
        Tower12.clip = song.tower12;

        if(song.chromatic == null)
        {
            chromatic.clip = null;
        }

        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        ////PAUSING
        //if (GameManager.Instance.isGamePaused)
        //{
        //    PauseMusic();
        //    return;
        //}
        //else
        //{
        //    ResumeMusic();
        //}

        if(!GameManager.Instance.isDynamicMusicActive)
        {
            flats.volume = 0.25f;
            major.volume = 0.25f;
            allegro.volume = 0.25f;
            trill.volume = 0.25f;
            chromatic.volume = 0.25f;
            poco.volume = 0.25f;
            forte.volume = 0.25f;
            legato.volume = 0.25f;
            Tower9.volume = 0.25f;
            Tower10.volume = 0.25f;
            Tower11.volume = 0.25f;
            Tower12.volume = 0.25f;
        }

        if (pauseConductor) return;

        //CONDUCTING
        Conduct();

        beatTrack = Mathf.Clamp(beatTrack, 0, 4);
    }

    private void FixedUpdate()
    {
        
    }

    public void Conduct()
    {
        //if the track assigned to the conductor music source is null, throw an error and return
        if (musicSource.clip == null) {
            Debug.Log("Music tracks not found. Please check the dynamic song assigned to the current encounter.");
            return;
        }
        
        songLoops = Mathf.FloorToInt(songPosition/musicSource.clip.length);

        //determine how many seconds since the song started
        Debug.Log((musicSource.time) - (songPosition % musicSource.clip.length));

        if (((musicSource.time) - (songPosition % musicSource.clip.length)) < 0)
        {
            songPosition += musicSource.time + (musicSource.clip.length - songPosition);
            Debug.Log("Modified time addition on loop");
        }
        else
        {
            songPosition += ((musicSource.time) - (songPosition % musicSource.clip.length));
            Debug.Log("Normal time addition");
        }
        
        //determine how many beats since the song started
        songPositionInBeats = (songPosition / crotchet) - GameManager.Instance.audioOffset;
        //songPositionInBeats = (musicSource.time / crotchet) - GameManager.Instance.audioOffset;
        //stagePositionInBeats = (songPosition / crotchet) - GameManager.Instance.audioOffset;

        if (songPositionInBeats >= numberOfBeats + 1 * 1)
        {
            numberOfBeats++;
            _ping.Play();
        }

        //beat duration is what you need to offset if you wanna change the "latency" of the input
        beatDuration = songPositionInBeats - numberOfBeats * 1;
        beatDuration = Mathf.Round(beatDuration * 100) * 0.01f;

        //this line adds the input offset if things break remove this
        beatDuration = Mathf.Abs(beatDuration - GameManager.Instance.inputOffset);

        beatDuration = Mathf.Clamp(beatDuration, 0, 1);

        //add a minus offset to this to offset beat events
        //_interval = musicSource.timeSamples / (musicSource.clip.frequency * crotchet);
        _interval = musicSource.timeSamples / (musicSource.clip.frequency * crotchet);
        TriggerBeatEvent(songPositionInBeats);
    }

    /*
    public bool InThreshHold()
    {
        if(beatDuration >= perfectBeatThreshold) //perfect
        {
            Debug.Log("Perfect Beat Hit");
            return true;
        }
        else if (beatDuration >= earlyGreatBeatThreshold) //great
        {
            Debug.Log("Great [Early] Beat Hit");
            return true;
        }
        else if (beatDuration >= earlyGreatBeatThreshold)
        {
            Debug.Log("Early Beat Hit");
            return true;
        }
        else if (beatDuration >= missBeatThreshold)
        {
            Debug.Log("Miss Beat Hit");
            return false;
        }
        else if (beatDuration >= lateBeatThreshold)
        {
            Debug.Log("Late Beat Hit");
            return true;
        }
        else if (beatDuration >= lateGreatBeatThreshold)
        {
            Debug.Log("Great [Late] Beat Hit");
            return true;
        }
        else
        {
            Debug.Log("Perfect Beat Hit");
            return true;
        }
    }
    */

    public void Beat()
    {
        if (beatTrack == 4)
        {
            measureTrack += 1;
            beatTrack = 0;
        }

        beatTrack += 1;
    }

    public void TriggerBeatEvent(float interval)
    {
        if(Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            foreach(UnityEvent _event in triggerEvent.ToArray())
            {
                _event.Invoke();
            }
            foreach (UnityEvent _event in projectileEvent.ToArray())
            {
                _event.Invoke();

            }
            foreach (UnityEvent _event in enemyEvent.ToArray())
            {
                _event.Invoke();
            }
            foreach (UnityEvent _event in pickupEvent.ToArray()) 
            {
                _event.Invoke();
            }
            foreach (UnityEvent _event in waveEvent.ToArray()) 
            {
                _event.Invoke();
            }
        }
    }

    public void PauseMusic()
    {
        musicSource.Pause();
        flats.Pause();
        major.Pause();
        allegro.Pause();
        trill.Pause();
        chromatic.Pause();
        poco.Pause();
        legato.Pause();
        forte.Pause();
        Tower9.Pause();
        Tower10.Pause();
        Tower11.Pause();
        Tower12.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
        flats.UnPause();
        major.UnPause();
        allegro.UnPause();
        trill.UnPause();
        chromatic.UnPause();
        poco.UnPause();
        legato.UnPause();
        forte.UnPause();  
        Tower9.UnPause();
        Tower10.UnPause();
        Tower11.UnPause();
        Tower12.UnPause();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        flats.Stop();
        major.Stop();
        allegro.Stop();
        trill.Stop();
        chromatic.Stop();
        poco.Stop();    
        legato.Stop();  
        forte.Stop();
        Tower9.Stop();
        Tower10.Stop();
        Tower11.Stop();
        Tower12.Stop();
    }

    public void PlayMusic()
    {
        flats.Play();
        major.Play();
        allegro.Play();
        trill.Play();
        chromatic.Play();
        poco.Play();
        legato.Play();
        forte.Play();
        Tower9.Play();
        Tower10.Play();
        Tower11.Play();
        Tower12.Play();

        //Debug.Log("music started");
    }
}

public enum _BeatResult
{
    nohit, late, miss, early, great, perfect
}

