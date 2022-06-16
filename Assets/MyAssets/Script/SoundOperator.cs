using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOperator : MonoBehaviour
{
    //���A�Đ�����Ă���T�E���h
    [SerializeField] AudioSource _beforeSound;
    //�V�����Đ�����T�E���h
    AudioSource _newSound;

    void Start()
    {
        _newSound = GetComponent<AudioSource>();
    }


    void Update()
    {
        
    }

    //�Â�BGM���~�߂�
    void StopBGM()
    {
        //�Â�BGM���Đ�����Ă���Ύ~�߂�
        if (_beforeSound.isPlaying)
        {
            _beforeSound.Stop();
        }
    }

    //�V����BGM���Đ�����
    void PlayBGM()
    {
        //�V����BGM���Đ�����Ă��Ȃ���΍Đ�����
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
