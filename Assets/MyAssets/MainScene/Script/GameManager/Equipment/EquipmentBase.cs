using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentBase : MonoBehaviour
{
    public enum EquipmentID
    {
        ID_0,
        ID_1,

        ID_END,
    }

    //<=========== �K�v�Ȓl ===========>//
    Equipment[] _equipmentData = new Equipment[(int)EquipmentID.ID_END];

    //<======== �A�T�C�����ׂ��l ========>//


    //<===== �C���X�y�N�^����ݒ肷�ׂ��l =====>//


    /// <summary> �������N���X�̏������֐��B(�h����ŌĂяo���B) </summary>
    /// <returns> �������ɐ��������ꍇtrue�A���s������false��Ԃ��B </returns>
    protected bool Initialize_EquipmentBase()
    {

        return true;
    }

    /// <summary> csv�t�@�C������f�[�^��ǂݍ��ފ֐� </summary>
    /// <returns> �ǂݍ��񂾌��ʂ�Ԃ��B���s�����ꍇ��null��Ԃ��B </returns>
    Equipment[] OnLoad_EquipmentData_csv()
    {
        Equipment[] result = new Equipment[(int)EquipmentID.ID_END];

        return result;
    }

    /// <summary> json�t�@�C������f�[�^��ǂݍ��݁A�����o�[�ϐ��Ɋi�[���鏈���B </summary>
    public void OnLoad_EquipmentData_Json()
    {

    }

    /// <summary> �������Ă��鑕���̃f�[�^���Ajson�t�@�C���ɕۑ����鏈���B </summary>
    public void OnSave_EquipmentData_Json()
    {

    }
}
