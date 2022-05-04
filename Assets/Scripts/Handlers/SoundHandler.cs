using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LV
{
    public class SoundHandler : MonoBehaviour
    {
        public AudioClip[] audioClips;
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void HandleWalkSound()
        {           
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);
        }


        private AudioClip GetRandomClip()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}