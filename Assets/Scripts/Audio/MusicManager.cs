using System.Collections;
using AudioSystem;
using UnityEngine;
using Utilities;

public class MusicManager : NonPersistentSingleton<MusicManager>
{
    [field: SerializeField] public AudioData DefaultBGM { get; set; }

    private AudioEmitter _currentEmitter;
    private AudioEmitter _nextEmitter;
    private void Start()
    {
        PlayMusic(DefaultBGM);
    }

    public void PlayMusic(AudioData audioData)
    {
        StartCoroutine(PlayMusicCoroutine(audioData));
    }

    public void StopMusic()
    {
        StartCoroutine(StopMusicCoroutine());
    }

    public IEnumerator PlayMusicCoroutine(AudioData audioData)
    {
        if (_currentEmitter != null)
        {
            _currentEmitter.LerpToStop();
            while (_currentEmitter.IsPlaying())
            {
                yield return null;
            }
        }

        _nextEmitter = AudioManager.Instance.CreateAudioBuilder()
            .WithLoop()
            .WithFadeIn()
            .WithParent(transform)
            .Play(audioData);

        _currentEmitter = _nextEmitter;
        _nextEmitter = null;
    }

    public IEnumerator StopMusicCoroutine()
    {
        if (_currentEmitter != null)
        {
            _currentEmitter.LerpToStop();
            while (_currentEmitter.IsPlaying())
            {
                yield return null;
            }
        }

        _currentEmitter = null;
    }
}
