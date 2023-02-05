using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] AudioClip NormalMusic;
    [SerializeField] AudioClip WaveMusic;
    [SerializeField] AudioClip AmbientSound;

    [SerializeField] AudioSource AmbientSoundSource;
    [SerializeField] AudioSource MusicSource;
    [SerializeField] GameOverHandler GameOverHandler;

    // Start is called before the first frame update
    private void Start()
    {
        GameOverHandler.OnGameOver += StopMusic;
        AmbientSoundSource.clip = AmbientSound;
        AmbientSoundSource.Play();
        MusicSource.clip = NormalMusic;
        MusicSource.Play();
    }
    public void PlayWaveMusic()
    {
        MusicSource.clip = WaveMusic;
        MusicSource.Play();
    }

    public void PlayNormalMusic()
    {
        MusicSource.clip = NormalMusic;
        MusicSource.Play();
    }

    public void StopMusic()
    {
        MusicSource.Stop();
    }
}
