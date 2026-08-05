using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Code from Sasquatch B Studios @ https://youtu.be/DU7cgVsU2rM?si=9FW1TVGFbVxKq9-X&t=262

    #region dont touch this
    private static AudioManager _instance;
    public static AudioManager instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("AudioManager is NULL");
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

    [Header("<b><size=15>Audio<b><size=15>")]
    [Line(255,255,255)]
    [SerializeField] private AudioSource soundEffectObject;

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (audioClip == null) {
            return;
        }

        AudioSource.PlayClipAtPoint(audioClip, Vector2.zero, volume);

        /*
        //spawn sound effect gameObject
        AudioSource audioSource = Instantiate(soundEffectObject, spawnTransform.position, Quaternion.identity);

        //assign audioClip
        audioSource.clip = audioClip;

        //assign audioSource volume
        audioSource.volume = volume;

        //play audioClip
        audioSource.Play();

        //get audioClip length
        float clipLength = audioSource.clip.length;

        //destroy audioClip after it finishes playing
        Destroy(audioSource.gameObject, clipLength);
        */  
    }

    public void PlayRandSound(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        if (audioClip == null) {
            return;
        }
        
        //assign a random index
        int rand = Random.Range(0, audioClip.Length);
        
        //spawn sound effect gameObject
        AudioSource audioSource = Instantiate(soundEffectObject, spawnTransform.position, Quaternion.identity);

        //assign audioClip
        audioSource.clip = audioClip[rand];

        //assign audioSource volume
        audioSource.volume = volume;

        //play audioClip
        audioSource.Play();

        //get audioClip length
        float clipLength = audioSource.clip.length;

        //destroy audioClip after it finishes playing
        Destroy(audioSource.gameObject, clipLength);
    }
}
