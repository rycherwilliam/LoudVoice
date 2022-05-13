using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LV
{
    public class SoundHandler : MonoBehaviour
    {
        public AudioClip[] audioClips;
        private AudioSource audioSource;
        private Collider playerCollider;
        public int surfaceType;
        private void Awake()
        {
            playerCollider = GetComponent<Collider>();
            audioSource = GetComponent<AudioSource>();
        }

        public void HandleWalkSound()
        {
            HandleSurfaceType();
            AudioClip clip = GetRandomClip();
            audioSource.PlayOneShot(clip);            
        }


        private AudioClip GetRandomClip()
        {
            return surfaceType switch
            {
                0 => audioClips[Random.Range(0, 9)],
                1 => audioClips[Random.Range(10, 49)],
                _ => audioClips[Random.Range(0, audioClips.Length)],
            };
        }

        private void HandleSurfaceType()
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(transform.position, Vector3.down, out hitinfo))
            {                
                if (hitinfo.transform.tag == "Grass")
                {
                    surfaceType = 1;                    
                }
                else
                {
                    surfaceType = 0;
                }
            }
           
        }
    }
}