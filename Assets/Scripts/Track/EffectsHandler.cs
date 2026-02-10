using System;
using AudioSystem;
using UnityEngine;

public class EffectsHandler : MonoBehaviour
{
    [field: Header("Effects")]
    [SerializeField] private ParticleSystem _exhaustEffect;
    [SerializeField] private ParticleSystem _driftSmokeEffect;
    [SerializeField] private TrailRenderer _leftDriftTrailEffect;
    [SerializeField] private TrailRenderer _rightDriftTrailEffect;
    private bool _driftTrailEmitting = false;

    [field: Header("Audio")]
    private AudioEmitter _engineEmitter;
    [SerializeField] private AudioData _idleAudio;
    [SerializeField] private AudioData _accelerateAudio;
    [SerializeField] private AudioData _deccelerateAudio;

    private AudioEmitter _brakeEmitter;
    [SerializeField] private AudioData _brakeAudio;

    private AudioEmitter _driftEmitter;
    [SerializeField] private AudioData _driftAudio;

    #region Engine Effects



    #endregion

    #region Exhaust Effect
    public void PlayExhaustEffect(bool play = true)
    {
        if (play && !_exhaustEffect.isPlaying)
        {
            _exhaustEffect.Play();
        }
        else if (!play && _exhaustEffect.isPlaying)
        {
            _exhaustEffect.Stop();
        }
    }

    public void SetExhaustRate(float rate)
    {
        if (!_exhaustEffect.isPlaying) return;

        if(_exhaustEffect.emission.rateOverTimeMultiplier != rate)
        {
            var emission = _exhaustEffect.emission;
            emission.rateOverTimeMultiplier = rate;
        }
    }

    #endregion

    #region Drift Effects
    public void PlayDriftSmokeEffect(bool play = true)
    {
        if (play && !_driftSmokeEffect.isPlaying)
        {
            _driftSmokeEffect.Play();
        }
        else if (!play && _driftSmokeEffect.isPlaying)
        {
            _driftSmokeEffect.Stop();
        }
    }

    public void PlayDriftTrailEffect(bool play = true)
    {
        if (play && !_driftTrailEmitting)
        {
            _driftTrailEmitting = true;
            _leftDriftTrailEffect.emitting = true;
            _rightDriftTrailEffect.emitting = true;
        }
        else if(!play && _driftTrailEmitting)
        {
            _driftTrailEmitting = false;
            _leftDriftTrailEffect.emitting = false;
            _rightDriftTrailEffect.emitting = false;
        }
    }

    public void PlayDriftAudio(bool play = true)
    {
        if (_driftEmitter != null)
        {
            if (play)
            {
                _driftEmitter.Play();
            }
            else
            {
                _driftEmitter.Stop();
            }
        }
        else if (play)
        {
            _driftEmitter = AudioManager.Instance.CreateAudioBuilder()
                .WithParent(transform)
                .WithFadeIn()
                .WithLoop()
                .Play(_driftAudio);
        }
    }
    #endregion

    public void PlayBrakeAudio(bool play = true)
    {
        if (_brakeAudio != null)
        {
            if (play)
            {
                _brakeEmitter.Play();
            }
            else
            {
                _brakeEmitter.Stop();
            }
        }
        else if (play)
        {
            _brakeEmitter = AudioManager.Instance.CreateAudioBuilder()
                .WithParent(transform)
                .WithFadeIn()
                .WithLoop()
                .Play(_brakeAudio);
        }
    }
}