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
    private VehicleStats _vehicleStats;

    public void SetupEffects(VehicleStats vehicleStats)
    {
        _exhaustEffectRate = EffectRate.None;
        _driftSmokeEffectRate = EffectRate.None;

        _driftSmokePlaying = false;
        _leftDriftSmokeEffect.Stop();
        _rightDriftSmokeEffect.Stop();

        _driftTrailEmitting = false;
        _leftDriftTrailEffect.emitting = false;
        _rightDriftTrailEffect.emitting = false;

        _vehicleStats = vehicleStats;
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
                return _vehicleStats.LowExhaustRate;

            case EffectRate.High:
                return _vehicleStats.HighExhaustRate;

            default:
                return rate;
        }
    }

    private EffectRate SetExhaustRange(float exhaustRate)
    {
        if (_exhaustEffectRate != EffectRate.None && exhaustRate < _vehicleStats.LowExhaustRange)
        {
            return EffectRate.None;
        }
        else if (_exhaustEffectRate != EffectRate.Low &&
            (exhaustRate > _vehicleStats.LowExhaustRange && exhaustRate < _vehicleStats.HighExhaustRange))
        {
            return EffectRate.Low;
        }
        if (_exhaustEffectRate != EffectRate.High && exhaustRate > _vehicleStats.HighExhaustRange)
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
                return _vehicleStats.LowDriftRate;

            case EffectRate.High:
                return _vehicleStats.HighDriftRate;

            default:
                return rate;
        }
    }

    private EffectRate SetDriftRange(float driftRate)
    {
        if (_driftSmokeEffectRate != EffectRate.None && driftRate < _vehicleStats.LowDriftRange)
        {
            return EffectRate.None;
        }
        else if (_driftSmokeEffectRate != EffectRate.Low &&
            (driftRate > _vehicleStats.LowDriftRange && driftRate < _vehicleStats.HighDriftRange))
        {
            return EffectRate.Low;
        }
        if (_driftSmokeEffectRate != EffectRate.High && driftRate > _vehicleStats.HighDriftRange)
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
        var emission = particleSystem.emission;
        if (emission.rateOverTimeMultiplier == clampedRate) return;
        
        while(emission.rateOverTimeMultiplier != clampedRate)
        {
            emission.rateOverTimeMultiplier = Mathf.MoveTowards(emission.rateOverTimeMultiplier, clampedRate, Time.deltaTime * Constants.EMISSION_MOVE_TOWARDS_RATE);
            if(Mathf.Approximately(emission.rateOverTimeMultiplier, clampedRate))
            {
                emission.rateOverTimeMultiplier = clampedRate;
            }
        }   
    }
}