using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioSauce : MonoBehaviour
{
    AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

    }


    void Update()
    {
        
    }

    /// <summary> 設定されたサウンドファイルを再生 </summary>
    public void PlayAudioSource()
    {
        _audioSource.Play();
    }

    public void PauseAudioSource()
    {
        _audioSource.Pause();
    }

    public void StopAudioSource()
    {
        _audioSource.Stop();
    }
}
