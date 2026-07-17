using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//code from git-amend on YT: https://youtu.be/BgpqoRFCNOs?si=S3UL_Catkghz3pUw

namespace AudioSystem {
    [Serializable]
    public class SoundData {
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        public bool loop;
        public bool playOnAwake;
    }
}