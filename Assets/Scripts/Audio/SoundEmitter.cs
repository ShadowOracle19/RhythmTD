using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//code from git-amend on YT: https://youtu.be/BgpqoRFCNOs?si=S3UL_Catkghz3pUw

/*
namespace AudioSystem {
    public class SoundEmitter : MonoBehavior {
        AudioSource audioSource;
        Coroutine playingCoroutine;

        void Awake() {
            audioSource = gameObject.GetOrAdd<AudioSource();
        }

        public void Play() {
            if (playingCoroutine != null) {
                StopCoroutine(playingCoroutine);
            }

            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        IEnumerator WaitForSoundToEnd() {
            yield return new WaitWhile(() => audioSource.isPlaying);
            SoundManager.Instance.ReturnToPool(this);
        }

        public void Stop() {
            if (playingCoroutine != null) {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }

            audioSource.Stop();
            SoundManager.Instance.ReturnToPool(this);
        }

        public void Initialize(SoundData data) {
            audioSource.clip = data.clip;
            audioSource.outputAudioMixerGroup = data.mixerGroup;
            audioSource.loop = data.loop;
            audioSource.playOnAwake = data.playOnAwake;
        }
    }
}
*/

