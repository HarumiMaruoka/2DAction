using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Equipment
{
    //装備の基底クラス:各装備クラスはこのクラスを継承する
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

    //各値
    float _offensivePower;//攻撃力
    float _defensePower;//防御力
    float _endurance;//耐久力

    //各値のSetter,Getter
    float OffensivePower { get => _offensivePower; set => _offensivePower = value; }
    float DefensePower { get => _defensePower; set => _defensePower = value; }
    float Endurance { get => _endurance; set => _endurance = value; }
}
