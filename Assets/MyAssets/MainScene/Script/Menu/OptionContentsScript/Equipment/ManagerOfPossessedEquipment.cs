using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//�������Ă��鑕�����Ǘ�����N���X
public class ManagerOfPossessedEquipment : MonoBehaviour
{
    //<=========== �K�v�Ȓl ===========>//
    /// <summary> �����{�^���̔z�� </summary>
    GameObject[] _equipmentButton;

    //<======== �A�T�C�����ׂ��l ========>//
    /// <summary> �����{�^���̃v���n�u </summary>
    [Header("�����{�^���̃v���n�u"), SerializeField] GameObject _equipmentButtonPrefab;

    //<===== �C���X�y�N�^����ݒ肷�ׂ��l =====>//
    /// <summary> �v���C���[�������ł��鑕���̍ő吔 </summary>
    [Header("�v���C���[�������ł��鑕���̍ő吔"), SerializeField] int _maxHaveValue;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
