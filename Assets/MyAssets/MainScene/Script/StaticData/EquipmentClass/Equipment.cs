using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Equipment
{
    //�����̊��N���X:�e�����N���X�͂��̃N���X���p������
    public enum EquipmentID
    {
        EQUIPMENT_ID_01,

        EQUIPMENT_ID_END
    }

    public enum EquipmentType
    {
        ARM_PARTS,
        FOOT_PARTS,
        HEAD_PARTS,
        TORSO_PARTS,

        PARTS_TYPE_END
    }

    EquipmentID _myID;
    EquipmentType _myType;

    //�e�l
    float _offensivePower;//�U����
    float _defensePower;//�h���
    float _endurance;//�ϋv��

    //�e�l��Setter,Getter
    float OffensivePower { get => _offensivePower; set => _offensivePower = value; }
    float DefensePower { get => _defensePower; set => _defensePower = value; }
    float Endurance { get => _endurance; set => _endurance = value; }
}
