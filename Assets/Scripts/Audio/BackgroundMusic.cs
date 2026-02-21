using AudioSystem;
using Utilities;

public class BackgroundMusic : PersistentSingleton<BackgroundMusic>
{
    public AudioData BGM;
    private AudioEmitter _emitter;

    private void Start()
    {
        if(_emitter != null) return;
        _emitter = AudioManager.Instance.CreateAudioBuilder()
            .WithLoop()
            .WithVolume(0.1f)
            .WithParent(transform)
            .Play(BGM);
    }

    public void SetVolume(float volume)
    {
        _emitter.WithVolume(volume);
    }

    public void PlayNewBackgroundMusic(AudioData audioData)
    {
        if (_emitter != null)
        {
            _emitter.Stop();
            _emitter = AudioManager.Instance.CreateAudioBuilder()
                .WithLoop()
                .WithVolume(0.1f)
                .WithParent(transform)
                .Play(audioData);
        }
    }
}
