using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] AudioClip NormalMusic;
    [SerializeField] AudioClip WaveMusic;
    [SerializeField] AudioClip AmbientSound;

    [SerializeField] AudioSource AmbientSoundSource;
    [SerializeField] AudioSource MusicSource;
    [SerializeField] GameOverHandler GameOverHandler;

    [Inject] private WaveController _waveController;

    // Start is called before the first frame update
    private void Start()
    {
        GameOverHandler.OnGameOver += StopMusic;
        AmbientSoundSource.clip = AmbientSound;
        AmbientSoundSource.Play();
        MusicSource.clip = NormalMusic;
        MusicSource.Play();

        _waveController.StartingWaveEvent += PlayWaveMusic;
        _waveController.EndWaveEvent += PlayNormalMusic;
    }
    
    private void PlayWaveMusic()
    {
        MusicSource.clip = WaveMusic;
        MusicSource.Play();
    }

    private void PlayNormalMusic()
    {
        MusicSource.clip = NormalMusic;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}
