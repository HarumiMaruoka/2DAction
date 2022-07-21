using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �����̏��������Ǘ�������N���X </summary>
public abstract class EquipmentBase : MonoBehaviour
{
    //<======= ���̃N���X�Ŏg�p����^ =======>//
    /// <summary> ������ID </summary>
    public enum EquipmentID
    {
        ID_0,
        ID_1,
        ID_2,
        ID_3,

        ID_END,
    }
    /// <summary> ���ݑ������Ă��鑕����\���\���� </summary>
    struct MyEquipped
    {
        Equipment _head;
        Equipment _torso;
        Equipment _arm;
        Equipment _foot;
    }
    struct HaveEquipped
    {
        public List<Equipment> _equipments;
    }

    //<=========== �K�v�Ȓl ===========>//
    /// <summary> �S�Ă̑����̏����ꎞ�ۑ����Ă����ϐ� </summary>
    Equipment[] _equipmentData;
    /// <summary> �������Ă��鑕���̃��X�g </summary>
    HaveEquipped _haveEquipment;
    /// <summary> ���ݑ������Ă��鑕�� </summary>
    MyEquipped _myEquipped;

    //<======== �A�T�C�����ׂ��l ========>//


    //<===== �C���X�y�N�^����ݒ肷�ׂ��l =====>//


    /// <summary> �������N���X�̏������֐��B(�h����ŌĂяo���B) </summary>
    /// <returns> �������ɐ��������ꍇtrue�A���s������false��Ԃ��B </returns>
    protected bool Initialize_EquipmentBase()
    {

        return true;
    }

    /// <summary> csv�t�@�C������A�S�Ă̑����̃f�[�^��ǂݍ��ފ֐� </summary>
    /// <returns> �ǂݍ��񂾌��ʂ�Ԃ��B���s�����ꍇ��null��Ԃ��B </returns>
    void OnLoad_EquipmentData_csv()
    {
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

    }

    /// <summary> �������Ă��鑕�����Ajson�t�@�C������f�[�^��ǂݍ��݁A�����o�[�ϐ��Ɋi�[���鏈���B </summary>
    public void OnLoad_EquipmentData_Json()
    {
        _haveEquipment._equipments = new List<Equipment>();
    }

    /// <summary> �������Ă��鑕���̃f�[�^���Ajson�t�@�C���ɕۑ����鏈���B </summary>
    public void OnSave_EquipmentData_Json()
    {

    }
}
