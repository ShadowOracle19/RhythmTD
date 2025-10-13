using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    // Code from Sasquatch B Studios @ https://youtu.be/DU7cgVsU2rM?si=9FW1TVGFbVxKq9-X&t=262

    public static SoundEffectsManager instance;

    [SerializeField] private AudioSource soundEffectObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
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
    }

    public void PlayRandSound(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
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
