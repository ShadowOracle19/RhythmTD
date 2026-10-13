using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//code from git-amend on YT: https://youtu.be/BgpqoRFCNOs?si=S3UL_Catkghz3pUw

/*
namespace AudioSystem {
    public class SoundManager : PersistentSingleton<SoundManager> {
        IObjectPool<SoundEmitter> soundEmitterPool;
        readonly List<SoundEmitter> activeSoundEmitters = new();
        public readonly Dictionary<SoundData, int> Counts = new();

        [SerializeField] SoundEmitter soundEmitterPrefab;
        [SerializeField] bool collectionCheck = true;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int maxPoolSize = 100;
        [SerializeField] int maxSoundInstances = 30;

        void Start() {
            InitalizePool();
        }

        public bool CanPlaySound(SoundData data) {
            return !Counts.TryGetValue(data, out var count) || count < maxSoundInstances;
        }

        public SoundEmitter Get() {
            return soundEmitterPool.Get();
        }

        public void ReturnToPool(SoundEmitter soundEmitter) {
            soundEmitterPool.Release(soundEmitter);
        }

        void OnDestroyPoolObject(SoundEmitter soundEmitter) {
            Destroy(soundEmitter.gameObject);
        }

        void OnReturnedToPool(SoundEmitter soundEmitter) {
            soundEmitter.gameObject.SetActive(false);
            activeSoundEmitters.Add(soundEmitter);
        }

        void OnTakeFromPool(SoundEmitter soundEmitter) {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitters.Add(soundEmitter);
        }
        
        SoundEmitter CreateSoundEmitter() {
            var soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        void InitalizePool() {
            soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                collectionCheck,
                defaultCapacity,
                maxPoolSize);
        }
    }
}
*/


