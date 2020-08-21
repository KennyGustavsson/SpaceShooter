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

        [NonSerialized] private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance != null) Destroy(gameObject);

            Instance = this;

            _audioDic = new Dictionary<int, AudioClip>();

            foreach (var sound in SFXs)
            {
                _audioDic.Add(sound.ID, sound.sound);
            }

            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.outputAudioMixerGroup = mixer;
            _audioSource.volume = 0.8f;
            _audioSource.spatialBlend = 0.5f;
        }

        public void PlayAudioAtLocation(int ID, Vector3 location)
        {
            if (!_audioDic.ContainsKey(ID)) return;

            transform.position = location;

            _audioSource.clip = _audioDic[ID];
            _audioSource.Play();
        }

        public void MasterVolume(float sliderValue)
        {
            mixer.audioMixer.SetFloat("Master", sliderValue);
        }
    }
}
