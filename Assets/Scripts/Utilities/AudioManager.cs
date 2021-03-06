using System;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class AudioManager : MonoBehaviour
    {
        [Serializable]
        public class Sound
        {
            public string name;
            public AudioClip clip;
            [Range(0f, 1f)]
            public float volume;
            [Range(.1f, 3f)]
            public float pitch;
            public bool loop;

            [HideInInspector]
            public AudioSource source;

        }

        public Sound[] sounds;
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            foreach (var sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        private void Start()
        {
            Play("bgmusic");
        }

        public void Play(string soundName)
        {
            var sound = sounds.Where(s => s.name == soundName).DefaultIfEmpty(null).First();
            // var sound = Array.Find(sounds, s => s.name == soundName);
            sound?.source?.Play();
        }

        public void SetVolume(string soundName, float volume)
        {
            var sound = Array.Find(sounds, s => s.name == soundName);
            sound.source.volume = volume;
        }
    }
}