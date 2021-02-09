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
    public class SoundController : Singleton<SoundController>
    {
        [SerializeField]
        private AudioEntry[] _audioEntries;
        private Dictionary<string, AudioSource> _audioSources = new Dictionary<string, AudioSource>();

        protected override void Awake()
        {
            base.Awake();
            InitAudioSources();
        }


        // ========================== Init ============================


        private void InitAudioSources()
        {
            if (_audioEntries == null || _audioEntries.Length == 0)
                return;

            GameObject gObj;
            AudioSource aSource;
            foreach (var audioClip in _audioEntries)
            {
                gObj = new GameObject(audioClip.audioClip.name);
                gObj.transform.parent = transform;

                aSource = gObj.AddComponent<AudioSource>();
                aSource.clip = audioClip.audioClip;
                aSource.volume = audioClip.volume;
            }

            AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();

            foreach (var audioSource in audioSources)
                _audioSources.Add(audioSource.clip.name, audioSource);
        }


        // ========================== Methods ============================

        public static void Play(string audioName, bool loop = false, bool canRestart = true)
        {
            if (!Instance._audioSources.ContainsKey(audioName))
            {
                GameDebug.LogWarning($"Audio {audioName} not found!!!", util.LogType.Audio);
                return;
            }

            if (Instance._audioSources[audioName].isPlaying)
            {
                GameDebug.Log($"Audio {audioName} is already playing!!!", util.LogType.Audio);
                return;
            }

            GameDebug.Log($"Start Playing: {audioName}", util.LogType.Audio);

            Instance._audioSources[audioName].loop = loop;
            Instance._audioSources[audioName].Play();
        }

        public static void FadeOut(string audioName, float duration = 1f, Action callback = null)
        {
            if (!Instance._audioSources.ContainsKey(audioName))
            {
                GameDebug.LogWarning($"Audio {audioName} not found!!!", util.LogType.Audio);
                return;
            }

            GameDebug.Log($"Fading in: {audioName}", util.LogType.Audio);

            //Instance._audioSources[audioName].FadeOut(duration, callback);
            Instance.StartCoroutine(FadeOutCore(Instance._audioSources[audioName], duration, callback));
        }

        private static System.Collections.IEnumerator FadeOutCore(AudioSource a, float duration, Action callback = null)
        {
            float startVolume = a.volume;

            while (a.volume > 0)
            {
                a.volume -= startVolume * Time.deltaTime / duration;
                yield return new WaitForEndOfFrame();
            }

            a.Stop();
            a.volume = startVolume;

            callback?.Invoke();
        }

        public static void FadeIn(string audioName, float duration = 1f, Action callback = null)
        {
            if (!Instance._audioSources.ContainsKey(audioName))
            {
                GameDebug.LogWarning($"Audio {audioName} not found!!!", util.LogType.Audio);
                return;
            }

            GameDebug.Log($"Fading in: {audioName}", util.LogType.Audio);

            //Instance._audioSources[audioName].FadeIn(duration, callback);
            Instance.StartCoroutine(FadeInCore(Instance._audioSources[audioName], duration, callback));
        }

        private static System.Collections.IEnumerator FadeInCore(AudioSource a, float duration, Action callback = null)
        {
            float finalVolume = a.volume;
            float startVolume = a.volume = 0;

            while (a.volume < finalVolume)
            {
                a.volume += finalVolume * Time.deltaTime / duration;
                yield return new WaitForEndOfFrame();
            }

            a.volume = finalVolume;

            callback?.Invoke();
        }

        public static void Stop(string audioName)
        {
            if (!Instance._audioSources.ContainsKey(audioName))
            {
                GameDebug.LogWarning($"Audio {audioName} not found!!!", util.LogType.Audio);
                return;
            }

            GameDebug.Log($"Stopped Playing: {audioName}", util.LogType.Audio);

            Instance._audioSources[audioName].Stop();
        }

        // ========================== OnValidate - Inspector usability ============================
        private void OnValidate()
        {
            foreach (var audioEntry in _audioEntries)
            {
                audioEntry.name = audioEntry.audioClip == null ? "" : audioEntry.audioClip.name;
            }
        }
    }

    [System.Serializable]
    public class AudioEntry
    {
        [HideInInspector]
        public string name;

        public AudioClip audioClip;
        [Range(0f, 1f)] public float volume = 1f;
    }
}