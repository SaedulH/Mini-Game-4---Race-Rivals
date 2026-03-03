using System;
using AudioSystem;
using UnityEngine;
using Utilities;

public class EffectsHandler : MonoBehaviour
{
    [field: Header("Effects")]

    [field: Header("Exhaust")]
    [SerializeField] private ParticleSystem _exhaustEffect;
    private EffectRate _exhaustEffectRate = EffectRate.None;

    [field: Header("Offroad")]
    [SerializeField] private ParticleSystem[] _wheelOffRoadEffects;
    [SerializeField] private TerrainEffectPreset[] _terrainEffectPresets;
    [SerializeField] private TerrainType[] _currentWheelTerrains = new TerrainType[4];
    [SerializeField] private AudioData _offRoadAudio;
    private AudioEmitter _offRoadEmitter;

    [field: Header("Drift")]
    [SerializeField] private ParticleSystem _rightDriftSmokeEffect;
    [SerializeField] private ParticleSystem _leftDriftSmokeEffect;
    private EffectRate _driftSmokeEffectRate = EffectRate.None;
    private bool _driftSmokePlaying = false;

    [SerializeField] private TrailRenderer[] _wheelTrailEffects;
    private bool _frontTrailsEmitting = false;
    private bool _backTrailsEmitting = false;

    [field: Header("Audio")]
    [SerializeField] private AudioData _idleAudio;
    [SerializeField] private AudioData _accelerateAudio;
    [SerializeField] private AudioData _deccelerateAudio;
    private AudioEmitter _engineEmitter;

    [SerializeField] private AudioData _brakeAudio;
    private AudioEmitter _brakeEmitter;

    [SerializeField] private AudioData _driftAudio;
    private AudioEmitter _driftEmitter;

    [SerializeField] private AudioData _wallCollisionAudio;
    [SerializeField] private AudioData _vehicleCollisionAudio;
    private float _timeSinceLastCollisionEffect = 0f;

    private VehicleStats _vehicleStats;

    private void FixedUpdate()
    {
        _timeSinceLastCollisionEffect += Time.fixedDeltaTime;
    }

    public void SetupEffects(VehicleStats vehicleStats)
    {
        _timeSinceLastCollisionEffect = 0f;
        _exhaustEffectRate = EffectRate.None;
        _driftSmokeEffectRate = EffectRate.None;

        _driftSmokePlaying = false;
        _leftDriftSmokeEffect.Stop();
        _rightDriftSmokeEffect.Stop();

        foreach (ParticleSystem offRoadEffect in _wheelOffRoadEffects)
        {
            if (offRoadEffect != null)
            {
                offRoadEffect.Stop();
            }
        }

        _frontTrailsEmitting = false;
        _backTrailsEmitting = false;
        foreach (TrailRenderer trail in _wheelTrailEffects)
        {
            trail.emitting = false;
        }

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

    #region Trail Effects

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

    public void PlayDriftEffects(bool play, bool isDrifting = false)
    {
        PlayDriftAudio(play);
        PlayDriftSmokeEffect(play);
        PlayDriftTrailEffect(play, isDrifting);
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

    public void PlayDriftTrailEffect(bool play, bool isDrifting)
    {
        if (play) 
        {
            if (!_frontTrailsEmitting && isDrifting)
            {
                _frontTrailsEmitting = true;

                _wheelTrailEffects[0].emitting = true;
                _wheelTrailEffects[1].emitting = true;
            }
            if (!_backTrailsEmitting)
            {
                _backTrailsEmitting = true;

                _wheelTrailEffects[2].emitting = true;
                _wheelTrailEffects[3].emitting = true;
            }
        }
        else if (!play)
        {
            if (_frontTrailsEmitting)
            {
                _frontTrailsEmitting = false;
                _wheelTrailEffects[0].emitting = false;
                _wheelTrailEffects[1].emitting = false;
            }
            if (_backTrailsEmitting)
            {
                _backTrailsEmitting = false;

                _wheelTrailEffects[2].emitting = false;
                _wheelTrailEffects[3].emitting = false;
            }
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

    #region OffRoad Effects
    public void PlayOffRoadEffects(int wheelIndex, bool play = true, TerrainType terrain = 0)
    {
        ParticleSystem wheelEffect = _wheelOffRoadEffects[wheelIndex];
        if (wheelEffect == null)
        {
            return;
        }

        if (play && terrain != TerrainType.Road)
        {
            if (!wheelEffect.isPlaying || _currentWheelTerrains[wheelIndex] != terrain)
            {
                PlayTerrainEffect(wheelIndex, terrain, wheelEffect);
            }
        }
        else
        {
            if (wheelEffect.isPlaying)
            {
                _wheelOffRoadEffects[wheelIndex].Stop();
            }
        }

        _currentWheelTerrains[wheelIndex] = terrain;
    }

    private void PlayTerrainEffect(int wheelIndex, TerrainType terrain, ParticleSystem wheelEffect)
    {
        if (_currentWheelTerrains[wheelIndex] == terrain)
        {
            wheelEffect.Play();
            return;
        }

        TerrainEffectPreset terrainEffect = _terrainEffectPresets[(int)terrain];

        var main = wheelEffect.main;
        main.startColor = terrainEffect.startColor;
        main.startSize = terrainEffect.startSize;
        main.startLifetime = terrainEffect.lifetime;

        var emission = wheelEffect.emission;
        emission.rateOverTimeMultiplier = terrainEffect.emissionRate;

        var textureSheet = wheelEffect.textureSheetAnimation;
        for (int i = 0; i < terrainEffect.effectSpriteSheet.Length; i++)
        {
            textureSheet.SetSprite(i, terrainEffect.effectSpriteSheet[i]);
        }
        wheelEffect.Play();
        return;
    }

    public void PlayOffRoadAudio(float offRoadFactor, bool isMovingFastEnough)
    {
        if (_offRoadAudio == null) return;

        if (_driftEmitter != null)
        {
            if (offRoadFactor > 0 && !_driftEmitter.IsPlaying() && isMovingFastEnough)
            {
                //_driftEmitter.WithVolume(offRoadFactor).Play();
            }
            else if (offRoadFactor <= 0 && _driftEmitter.IsPlaying() || !isMovingFastEnough)
            {
                _driftEmitter.Stop();
            }
        }
        else if (offRoadFactor > 0 && isMovingFastEnough)
        {
            _driftEmitter = AudioManager.Instance.CreateAudioBuilder()
                .WithParent(transform)
                .WithFadeIn()
                .WithLoop()
                .WithVolume(offRoadFactor)
                .Play(_driftAudio);
        }
    }

    #endregion

    private void SetEmissionRate(ParticleSystem particleSystem, float clampedRate)
    {
        var emission = particleSystem.emission;
        if (emission.rateOverTimeMultiplier == clampedRate) return;

        while (emission.rateOverTimeMultiplier != clampedRate)
        {
            emission.rateOverTimeMultiplier = Mathf.MoveTowards(emission.rateOverTimeMultiplier, clampedRate, Time.deltaTime * Constants.EMISSION_MOVE_TOWARDS_RATE);
            if (Mathf.Approximately(emission.rateOverTimeMultiplier, clampedRate))
            {
                emission.rateOverTimeMultiplier = clampedRate;
            }
        }
    }

    public void PlayCollisionEffects(float speed, bool isAnotherVehicle)
    {
        if (_timeSinceLastCollisionEffect < Constants.COLLISION_EFFECT_COOLDOWN_TIME)
        {
            return;
        }
        _timeSinceLastCollisionEffect = 0f;

        float relativeSpeed = Mathf.InverseLerp(0, _vehicleStats.TopSpeed, speed);
        float volume = relativeSpeed * Constants.COLLISION_VOLUME_COEFFICIENT;
        float duration = relativeSpeed * Constants.COLLISION_DURATION_COEFFICIENT;
        float intensity = relativeSpeed * Constants.COLLISION_INTENSITY_COEFFICIENT;

        //Debug.Log($"Speed: {speed}, Relative Speed: {relativeSpeed}, Volume: {volume}, Intensity: {intensity}, Duration: {duration}");

        PlayCollisionAudio(volume, isAnotherVehicle);
        CameraShake.Instance.ShakeCamera(intensity, duration);
    }

    private void PlayCollisionAudio(float volume, bool isAnotherVehicle)
    {
        AudioData collisionAudio = isAnotherVehicle ? _vehicleCollisionAudio : _wallCollisionAudio;
        if (collisionAudio == null) return;

        AudioManager.Instance.CreateAudioBuilder()
            .WithParent(transform)
            .WithRandomPitch()
            .WithVolume(volume)
            .Play(collisionAudio);
    }
}
