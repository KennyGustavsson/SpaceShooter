using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SS
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        public AudioMixerGroup mixer;

        [System.Serializable]
        public struct SFX
        {
            public int ID;
            public AudioClip sound;
        }
        [SerializeField] public SFX[] SFXs;

        private Dictionary<int, AudioClip> _audioDic;

        private int _audioChannels = 5;
        private Queue<AudioSource> _sources;      

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);

            Instance = this;

            // Creating Sound Dictionary
            _audioDic = new Dictionary<int, AudioClip>();          
            foreach (var sound in SFXs)
            {
                _audioDic.Add(sound.ID, sound.sound);
            }

            // Creating Audio Channels with AudioSources
            _sources = new Queue<AudioSource>();
            for(int i = 0; i <_audioChannels; i++)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = mixer;
                audioSource.volume = 0.8f;
                audioSource.spatialBlend = 0.5f;
                _sources.Enqueue(audioSource);
            }
        }

        public void PlayAudioAtLocation(int ID, Vector3 location)
        {
            if (!_audioDic.ContainsKey(ID)) return;

            transform.position = location;

            var source = _sources.Dequeue();
            source.clip = _audioDic[ID];
            source.Play();
            _sources.Enqueue(source);
        }

        public void MasterVolume(float sliderValue)
        {
            mixer.audioMixer.SetFloat("Master", sliderValue);
        }
    }
}
