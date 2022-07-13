using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManage : MonoBehaviour
{
    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B
    //�C���X�^���X�𐶐�
    private static PlayerWeaponManage _instance;
    //�C���X�^���X���͓ǂݎ���p
    public static PlayerWeaponManage Instance
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
    private PlayerWeaponManage() { }

    //�e�Ɋւ��Ă̓I�u�W�F�N�g�v�[���H���g�p����
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
    }

    void Start()
    {
        _nomalShotBullet = new GameObject[_maxBullet];
    }

    void Update()
    {
        
    }
}
