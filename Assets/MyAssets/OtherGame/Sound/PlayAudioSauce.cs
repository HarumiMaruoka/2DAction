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

    /// <summary> �ݒ肳�ꂽ�T�E���h�t�@�C�����Đ� </summary>
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
