using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //<===== �V���O���g���֌W =====>//
    private static GameManager _instance;
    /// <summary> GameManager�N���X�̗B��̃C���X�^���X </summary>
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
    //�v���C�x�[�g�ȃR���X�g���N�^
    private GameManager() { }

    /// <summary> �������ɐ����������ǂ�����\���ϐ��B </summary>
    bool _isInitialized = false;

    void Awake()
    {
        if( _isInitialized = Initialized())
        {
            Debug.Log($"�������ɐ������܂����B : �I�u�W�F�N�g��{gameObject.name}");
        }
        else
        {
            Debug.LogError($"�������Ɏ��s���܂����B : �I�u�W�F�N�g��{gameObject.name}");
        }
    }
    void Start()
    {

    }
    void Update()
    {

    }

    //<===== privete�����o�[�֐� =====>//
    /// <summary> �������֐� </summary>
    /// <returns> �������ɐ��������� true ��Ԃ��B </returns>
    bool Initialized()
    {
        //�����C���X�^���X���ݒ肳��Ă��Ȃ������玩�g��������
        if (_instance == null)
        {
            _instance = this;
        }
        //�������ɑ��݂���ꍇ�́A���̃I�u�W�F�N�g��j������B
        else if (_instance != null)
        {
            Destroy(this);
        }
        //���̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C����Ȃ��悤�ɂ���B
        DontDestroyOnLoad(gameObject);

        return true;
    }
}
