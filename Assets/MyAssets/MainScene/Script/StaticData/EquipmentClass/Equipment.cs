using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Equipment
{
    //υΜξκNX:eυNXΝ±ΜNXπp³·ι
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

    //el
    float _offensivePower;//UΝ
    float _defensePower;//hδΝ
    float _endurance;//ΟvΝ

    //elΜSetter,Getter
    float OffensivePower { get => _offensivePower; set => _offensivePower = value; }
    float DefensePower { get => _defensePower; set => _defensePower = value; }
    float Endurance { get => _endurance; set => _endurance = value; }
}
