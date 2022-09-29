using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOperator : MonoBehaviour
{
    //今、再生されているサウンド
    [SerializeField] AudioSource _beforeSound;
    //新しく再生するサウンド
    AudioSource _newSound;

    void Start()
    {
        _newSound = GetComponent<AudioSource>();
    }

    //古いBGMを止める
    void StopBGM()
    {
        //古いBGMが再生されていれば止める
        if (_beforeSound.isPlaying)
        {
            _beforeSound.Stop();
        }
    }

    //新しいBGMを再生する
    void PlayBGM()
    {
        //新しいBGMが再生されていなければ再生する
        if (!_newSound.isPlaying)
        {
            _newSound.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopBGM();
            PlayBGM();
        }
    }
}
