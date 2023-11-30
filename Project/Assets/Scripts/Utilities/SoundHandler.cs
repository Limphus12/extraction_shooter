using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace com.limphus.utilities
{
    [System.Serializable]
    public struct SoundStruct
    {
        public AudioClip[] clips;

        public float maxDistance;
        public float pitch;

        public Transform transform;
    }

    public class SoundHandler : MonoBehaviour
    {
        [Header("Attributes - Sound")]
        [Space]
        [SerializeField] protected AudioMixerGroup audioMixerGroup;

        protected void PlayRandomSound(SoundStruct sounds)
        {
            if (sounds.clips.Length == 0) return;

            //pick a random sound out of the array, and play one shot of it...
            PlayOneShotSound(sounds.clips[Random.Range(0, sounds.clips.Length)], sounds.transform.position, sounds.maxDistance, sounds.pitch);
        }

        protected void PlayRandomSound(SoundStruct sounds, Vector3 position)
        {
            if (sounds.clips.Length == 0) return;

            //pick a random sound out of the array, and play one shot of it, with a position override
            PlayOneShotSound(sounds.clips[Random.Range(0, sounds.clips.Length)], position, sounds.maxDistance, sounds.pitch);
        }

        protected void PlaySound(AudioClip clip, Vector3 position)
        {
            if (clip == null)
            {
                Debug.LogWarning("No Audio Clip found! Assign an Audio Clip!");
                return;
            }

            //instance of an object, attach an audio source & play our clip!
            GameObject soundGameObject = new GameObject(clip.name);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            //assign to the correct audio mixer and play!
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.PlayOneShot(clip);
            audioSource.spatialBlend = 1f; //3D

            //oh and make sure we're placed wherever this object is.
            soundGameObject.transform.position = position;

            //make sure to destroy the sound game object after the sound is done!
            Destroy(soundGameObject, clip.length);
        }

        protected void PlaySound(AudioClip clip, Vector3 position, Transform parent)
        {
            if (clip == null)
            {
                Debug.LogWarning("No Audio Clip found! Assign an Audio Clip!");
                return;
            }

            //instance of an object, attach an audio source & play our clip!
            GameObject soundGameObject = new GameObject(clip.name);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            //assign to the correct audio mixer and play!
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.PlayOneShot(clip);
            audioSource.spatialBlend = 1f; //3D

            //oh and make sure we're placed wherever this object is.
            soundGameObject.transform.position = position;
            soundGameObject.transform.parent = parent;

            //make sure to destroy the sound game object after the sound is done!
            Destroy(soundGameObject, clip.length);
        }

        protected void PlayLoopingSound(AudioSource audioSource, AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("No Audio Clip found! Assign an Audio Clip!");
                return;
            }

            else if (clip == audioSource.clip) return;

            //attach the clip to the audio source
            audioSource.clip = clip;

            audioSource.Play();
        }

        protected void RemoveSound(AudioSource audioSource)
        {
            if (!audioSource) return;

            audioSource.clip = null;
        }

        protected void PlayOneShotSound(AudioClip clip, Vector3 position, float maxDistance)
        {
            if (clip == null)
            {
                Debug.LogWarning("No Audio Clip found! Assign an Audio Clip!");
                return;
            }

            //instance of an object, attach an audio source & play our clip!
            GameObject soundGameObject = new GameObject(clip.name);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            //assign to the correct audio mixer and play!
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.PlayOneShot(clip);
            //audioSource.spatialBlend = 1f; //3D
            audioSource.maxDistance = maxDistance;

            //oh and make sure we're placed in the correct position
            if (position != null) soundGameObject.transform.position = position;

            //make sure to destroy the sound game object after the sound is done!
            Destroy(soundGameObject, clip.length);
        }

        protected void PlayOneShotSound(AudioClip clip, Vector3 position, float maxDistance, float volume)
        {
            if (clip == null)
            {
                Debug.LogWarning("No Audio Clip found! Assign an Audio Clip!");
                return;
            }

            //instance of an object, attach an audio source & play our clip!
            GameObject soundGameObject = new GameObject(clip.name);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            //assign to the correct audio mixer and play!
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.PlayOneShot(clip);
            //audioSource.spatialBlend = 1f; //3D
            audioSource.maxDistance = maxDistance;
            audioSource.volume = volume;

            //oh and make sure we're placed in the correct position
            if (position != null) soundGameObject.transform.position = position;

            //make sure to destroy the sound game object after the sound is done!
            Destroy(soundGameObject, clip.length);
        }
    }
}