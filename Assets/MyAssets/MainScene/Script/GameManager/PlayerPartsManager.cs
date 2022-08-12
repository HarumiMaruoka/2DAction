using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPartsManager : PlayerPartsMavagerBase
{
    //=======�V���O���g���֌W=======//
    //�C���X�^���X�𐶐�
    private static PlayerPartsManager _instance;
    //�C���X�^���X���͓ǂݎ���p
    public static PlayerPartsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager._instance��null�ł��B");
            }
            return _instance;
        }
    }
    //�v���C�x�[�g�ȃR���X�g���N�^���`����
    private PlayerPartsManager() { }

    [Header("�����ł���p�[�c�̍ő吔"), SerializeField] int _maxHaveValue;
    /// <summary> �����ł���p�[�c�̍ő吔 </summary>
    public int MaxHaveValue { get => _maxHaveValue; }
    /// <summary> ���ݏ������Ă���p�[�c�̔z�� </summary>
    Equipment[] _havePartsList;

    //���� : �e�Ɋւ��Ă̓I�u�W�F�N�g�v�[�����g�p����B
    //�e�̊i�[��
    [Header("�e�̍ő吔"), SerializeField] int _maxBullet;
    GameObject[] _nomalShotBullet;



    private void Awake()
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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        _nomalShotBullet = new GameObject[_maxBullet];
    }

    void Update()
    {
        
    }
}
