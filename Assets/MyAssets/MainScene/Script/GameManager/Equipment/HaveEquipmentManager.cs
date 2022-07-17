using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �����Ă��鑕�����Ǘ�����N���X </summary>
public class HaveEquipmentManager : MonoBehaviour
{
    //<======�V���O���g���p�^�[���֘A======>//
    private static HaveEquipmentManager _instance;
    public static HaveEquipmentManager Instance
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
    private HaveEquipmentManager() { }


    //<=========== �K�v�Ȓl ===========>//
    /// <summary> �������Ă��鑕���̔z�� </summary>
    GameObject[] _haveEquipments;

    //<======== �A�T�C�����ׂ��l ========>//


    //<===== �C���X�y�N�^����ݒ肷�ׂ��l =====>//
    /// <summary> �v���C���[�������ł��鑕���̍ő吔 </summary>
    [Header("�v���C���[�������ł��鑕���̍ő吔"), SerializeField] int _maxHaveValue;

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
        //���̃I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C���Ȃ��B
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
