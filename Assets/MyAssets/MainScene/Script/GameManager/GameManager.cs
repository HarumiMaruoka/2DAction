using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B

    //�C���X�^���X�𐶐�
    private static GameManager _instance;

    //�C���X�^���X���擾
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    private void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("�Q�[���}�l�[�W���[��AudioSource Component���A�^�b�`���Ă�������!");
            }
        }
    }

    AudioSource _audioSource;

    /// <summary> BGM��؂�ւ��� </summary>
    void ChangeBGM()
    {
        //���̃V�[����BGM���擾
        AudioSource bgm = GameObject.Find("BGMManager").GetComponent<AudioSource>();
        if (bgm == null)
        {
            Debug.LogError("���̃G���A�ɂ́ABGM���ݒ肳��Ă��܂���!");
        }
    }
}
