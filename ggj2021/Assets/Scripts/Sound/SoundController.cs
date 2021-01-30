/*
 * Created by Hugo Uchoas Borges <hugouchoas@outlook.com>
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using util;

namespace sound
{
    [RequireComponent(typeof(AudioListener))]
    public class SoundController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] _audioClipEntries;
        private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

        private void Awake()
        {
            SoundController.instance = this;
            InitAudioSources();
        }


        // ========================== Init ============================


        private void InitAudioSources()
        {
            if (_audioClipEntries == null || _audioClipEntries.Length == 0)
                return;

            GameObject gObj;
            AudioSource aSource;
            foreach (var audioClip in _audioClipEntries)
            {
                gObj = new GameObject(audioClip.name);
                gObj.transform.parent = transform;

                aSource = gObj.AddComponent<AudioSource>();
                aSource.clip = audioClip;
            }

            AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();

            foreach (var audioSource in audioSources)
                _audioSources.Add(audioSource.clip.name, audioSource);
        }


        // ========================== Methods ============================

        public static void Play(string audioName, bool loop = false)
        {
            if (!Instance._audioSources.ContainsKey(audioName))
            {
                GameDebug.LogWarning($"Audio {audioName} not found!!!", util.LogType.Audio);
                return;
            }

            Instance._audioSources[audioName].loop = loop;
            Instance._audioSources[audioName].Play();
        }

        public static void Stop(string audioName)
        {
            if (!Instance._audioSources.ContainsKey(audioName))
            {
                GameDebug.LogWarning($"Audio {audioName} not found!!!", util.LogType.Audio);
                return;
            }

            Instance._audioSources[audioName].Stop();
        }

        // ========================== Singleton ============================

        private static SoundController instance;
        public static SoundController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(nameof(SoundController)).AddComponent<SoundController>();
                }
                return instance;
            }
        }
    }
}