using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEmitter : MonoBehaviour
    {
        public AudioData Data { get; private set; }
        public LinkedListNode<AudioEmitter> Node { get; set; }

        AudioSource audioSource;
        private Coroutine currentCoroutine;

        private bool isFadingOut;

        void Awake()
        {
            audioSource = gameObject.GetOrAdd<AudioSource>();
        }

        public void Initialize(AudioData data)
        {
            Data = data;
            audioSource.clip = data.clip;
            audioSource.outputAudioMixerGroup = data.mixerGroup;
            audioSource.loop = data.loop;
            audioSource.playOnAwake = data.playOnAwake;

            audioSource.mute = data.mute;
            audioSource.bypassEffects = data.bypassEffects;
            audioSource.bypassListenerEffects = data.bypassListenerEffects;
            audioSource.bypassReverbZones = data.bypassReverbZones;

            audioSource.priority = data.priority;
            audioSource.volume = data.volume;
            audioSource.pitch = data.pitch;
            audioSource.panStereo = data.panStereo;
            audioSource.spatialBlend = data.spatialBlend;
            audioSource.reverbZoneMix = data.reverbZoneMix;
            audioSource.dopplerLevel = data.dopplerLevel;
            audioSource.spread = data.spread;

            audioSource.minDistance = data.minDistance;
            audioSource.maxDistance = data.maxDistance;

            audioSource.ignoreListenerVolume = data.ignoreListenerVolume;
            audioSource.ignoreListenerPause = data.ignoreListenerPause;

            audioSource.rolloffMode = data.rolloffMode;
        }

        #region Playback

        public void Play(bool retain = false)
        {
            StopCurrentCoroutine();
            isFadingOut = false;

            audioSource.Play();

            if (!retain)
            {
                currentCoroutine = StartCoroutine(WaitForEnd());
            }
        }

        public void Resume(float duration = 0f)
        {
            if (audioSource.isPlaying) return;

            audioSource.Play();

            if (duration > 0f)
            {
                currentCoroutine = StartCoroutine(FadeVolume(0f, Data.volume, duration));
            }
        }

        public void Pause()
        {
            StopCurrentCoroutine();
            audioSource.Pause();
            isFadingOut = false;
        }

        public void Stop(bool retain = false)
        {
            StopCurrentCoroutine();
            audioSource.Stop();
            isFadingOut = false;
            if (!retain)
            {
                AudioManager.Instance.ReturnToPool(this);
            }
        }

        #endregion

        #region Fading

        public void FadeToVolume(float target, float duration = 0.2f)
        {
            isFadingOut = false;
            StopCurrentCoroutine();
            currentCoroutine = StartCoroutine(FadeVolume(audioSource.volume, target, duration));
        }

        public void FadeToPause(float duration = 0.5f)
        {
            if (isFadingOut) return;

            isFadingOut = true;

            StopCurrentCoroutine();
            currentCoroutine = StartCoroutine(FadeOutThen(duration, Pause));
        }

        public void FadeToStop(float duration = 0.5f, bool retain = false)
        {
            if (isFadingOut) return;

            isFadingOut = true;

            StopCurrentCoroutine();
            currentCoroutine = StartCoroutine(FadeOutThen(duration, () => Stop(retain)));
        }
         
        private IEnumerator FadeOutThen(float duration, System.Action onComplete)
        {
            yield return FadeVolume(audioSource.volume, 0f, duration);
            onComplete?.Invoke();
        }

        private IEnumerator FadeVolume(float start, float target, float duration)
        {
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, target, time / duration);
                yield return null;
            }

            audioSource.volume = target;
        }

        #endregion

        #region Helpers

        private IEnumerator WaitForEnd()
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            Stop();
        }

        private void StopCurrentCoroutine()
        {
            if (currentCoroutine == null) return;

            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        public bool IsPlaying() => audioSource.isPlaying;
        public float GetVolume() => audioSource.volume;

        #endregion

        #region Modifiers

        public void WithRandomPitch(float min = -0.1f, float max = 0.1f)
        {
            audioSource.pitch += Random.Range(min, max);
        }

        public void WithAdditivePitch(float pitch)
        {
            audioSource.pitch += pitch;
        }

        public void WithPitch(float pitch)
        {
            audioSource.pitch = pitch;
        }

        public void WithLoop()
        {
            audioSource.loop = true;
        }

        public void WithReverb(float min = -0.1f, float max = 0.1f)
        {
            audioSource.reverbZoneMix += Random.Range(min, max);
        }

        public void WithVolume(float target, bool fade = false, float duration = 0.1f)
        {
            if (!fade)
            {
                audioSource.volume = target;
                return;
            }
            StopCurrentCoroutine();
            currentCoroutine = StartCoroutine(FadeVolume(0f, target, duration));
        }

        #endregion
    }
}