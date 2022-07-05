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

    //�e�Ɋւ��Ă̓I�u�W�F�N�g�v�[���H���g�p����
    //�e�̊i�[��
    [SerializeField] int _maxBullet { get; }

    GameObject[] _nomalShotBullet = new GameObject[_maxBullet];

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
        
    }

    void Update()
    {
        
    }
}
