using AudioSystem;
using UnityEngine;
using Utilities;

public class EffectsHandler : MonoBehaviour
{
    [field: Header("Effects")]
    [SerializeField] private ParticleSystem _exhaustEffect;
    private EffectRate _exhaustEffectRate = EffectRate.None;

    [SerializeField] private ParticleSystem _leftDriftSmokeEffect;
    [SerializeField] private ParticleSystem _rightDriftSmokeEffect;
    private EffectRate _driftSmokeEffectRate = EffectRate.None;
    private bool _driftSmokePlaying = false;

    [SerializeField] private TrailRenderer _leftDriftTrailEffect;
    [SerializeField] private TrailRenderer _rightDriftTrailEffect;
    private bool _driftTrailEmitting = false;

    [field: Header("Audio")]
    [SerializeField] private AudioData _idleAudio;
    [SerializeField] private AudioData _accelerateAudio;
    [SerializeField] private AudioData _deccelerateAudio;
    private AudioEmitter _engineEmitter;

    [SerializeField] private AudioData _brakeAudio;
    private AudioEmitter _brakeEmitter;

    [SerializeField] private AudioData _driftAudio;
    private AudioEmitter _driftEmitter;


    private void Start()
    {
        _exhaustEffectRate = EffectRate.None;
        _driftSmokeEffectRate = EffectRate.None;

        _driftSmokePlaying = false;
        _leftDriftSmokeEffect.Stop();
        _rightDriftSmokeEffect.Stop();

        _driftTrailEmitting = false;
        _leftDriftTrailEffect.emitting = false;
        _rightDriftTrailEffect.emitting = false;
    }

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

        float clampedRate = GetClampedExhaustRate(rate);
        SetEmissionRate(_exhaustEffect, clampedRate);
    }

    private float GetClampedExhaustRate(float rate)
    {
        _exhaustEffectRate = SetExhaustRange(rate);
        switch (_exhaustEffectRate)
        {
            case EffectRate.None:
                return Constants.IDLE_EXHAUST_RATE;

            case EffectRate.Low:
                return Constants.LOW_EXHAUST_RATE;

            case EffectRate.High:
                return Constants.HIGH_EXHAUST_RATE;

            default:
                return rate;
        }
    }

    private EffectRate SetExhaustRange(float exhaustRate)
    {
        if (_exhaustEffectRate != EffectRate.None && exhaustRate < Constants.LOW_EXHAUST_RANGE)
        {
            return EffectRate.None;
        }
        else if (_exhaustEffectRate != EffectRate.Low &&
            (exhaustRate > Constants.LOW_EXHAUST_RANGE && exhaustRate < Constants.HIGH_EXHAUST_RANGE))
        {
            return EffectRate.Low;
        }
        if (_exhaustEffectRate != EffectRate.High && exhaustRate > Constants.HIGH_EXHAUST_RANGE)
        {
            return EffectRate.High;
        }

        return _exhaustEffectRate;
    }

    #endregion

    #region Drift Effects

    public void SetDriftRate(float rate)
    {
        float clampedRate = GetClampedDriftRate(rate);
        SetEmissionRate(_leftDriftSmokeEffect, clampedRate);
        SetEmissionRate(_rightDriftSmokeEffect, clampedRate);
    }

    private float GetClampedDriftRate(float rate)
    {
        _driftSmokeEffectRate = SetDriftRange(rate);
        switch (_driftSmokeEffectRate)
        {
            case EffectRate.None:
                return Constants.IDLE_DRIFT_RATE;

            case EffectRate.Low:
                return Constants.LOW_DRIFT_RATE;

            case EffectRate.High:
                return Constants.HIGH_DRIFT_RATE;

            default:
                return rate;
        }
    }

    private EffectRate SetDriftRange(float driftRate)
    {
        if (_driftSmokeEffectRate != EffectRate.None && driftRate < Constants.LOW_DRIFT_RANGE)
        {
            return EffectRate.None;
        }
        else if (_driftSmokeEffectRate != EffectRate.Low &&
            (driftRate > Constants.LOW_DRIFT_RANGE && driftRate < Constants.HIGH_DRIFT_RANGE))
        {
            return EffectRate.Low;
        }
        if (_driftSmokeEffectRate != EffectRate.High && driftRate > Constants.HIGH_DRIFT_RANGE)
        {
            return EffectRate.High;
        }

        return _driftSmokeEffectRate;
    }

    public void PlayDriftEffects(bool play = true)
    {
        PlayDriftAudio(play);
        PlayDriftSmokeEffect(play);
        PlayDriftTrailEffect(play);
    }

    public void PlayDriftSmokeEffect(bool play)
    {
        if (play && !_driftSmokePlaying)
        {
            _driftSmokePlaying = true;
            _leftDriftSmokeEffect.Play();
            _rightDriftSmokeEffect.Play();
        }
        else if (!play && _driftSmokePlaying)
        {
            _driftSmokePlaying = false;
            _leftDriftSmokeEffect.Stop();
            _rightDriftSmokeEffect.Stop();
        }
    }

    public void PlayDriftTrailEffect(bool play)
    {
        if (play && !_driftTrailEmitting)
        {
            _driftTrailEmitting = true;
            _leftDriftTrailEffect.emitting = true;
            _rightDriftTrailEffect.emitting = true;
        }
        else if (!play && _driftTrailEmitting)
        {
            _driftTrailEmitting = false;
            _leftDriftTrailEffect.emitting = false;
            _rightDriftTrailEffect.emitting = false;
        }
    }

    public void PlayDriftAudio(bool play)
    {
        if (_driftEmitter != null)
        {
            if (play && !_driftEmitter.IsPlaying())
            {
                _driftEmitter.Play();
            }
            else if (!play && _driftEmitter.IsPlaying())
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

    public void PlayBrakeAudio(bool play = true)
    {
        if (_brakeAudio != null)
        {
            if (play && !_brakeEmitter.IsPlaying())
            {
                _brakeEmitter.Play();
            }
            else if (!play && _brakeEmitter.IsPlaying())
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

    #endregion

    private void SetEmissionRate(ParticleSystem particleSystem, float clampedRate)
    {
        if (particleSystem.emission.rateOverTimeMultiplier != clampedRate)
        {
            var emission = particleSystem.emission;
            emission.rateOverTimeMultiplier = clampedRate;
        }
    }
}