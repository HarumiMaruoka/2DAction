using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 装備の基底クラス:各装備クラスはこのクラスを継承する </summary>
abstract public class Equipment
{
    /// <summary>
    /// 装備のコンストラクタ
    /// </summary>
    /// <param name="id"> ID </param>
    /// <param name="type"> Type </param>
    /// <param name="name"> 名前 </param>
    /// <param name="specialEffects"> 特殊効果の効果量(幅?) </param>
    /// <param name="maxHealthPoint_RiseValue"> 最大体力の増加量 </param>
    /// <param name="maxStamina_RiseValue"> 最大スタミナの増加量 </param>
    /// <param name="offensivePower_ShortDistance_RiseValue"> 近距離攻撃力の増加量 </param>
    /// <param name="offensivePower_LongDistance_RiseValue">遠距離攻撃力の増加量</param>
    /// <param name="defensePower_RiseValue"> 防御力の増加量 </param>
    /// <param name="moveSpeed_RiseValue"> 移動速度の上昇量 </param>
    /// <param name="endurance_RiseValue"> 耐久力 </param>
    public Equipment(EquipmentID id, EquipmentType type, string name, EquipmentRarity rarity, float specialEffects = 0f,
        float maxHealthPoint_RiseValue = 0f, float maxStamina_RiseValue = 0f,
        float offensivePower_ShortDistance_RiseValue = 0f, float offensivePower_LongDistance_RiseValue = 0f,
        float defensePower_RiseValue = 0f, float moveSpeed_RiseValue = 0f, float endurance_RiseValue = 0f)
    {
        _myID = id;
        _myType = type;
        _myName = name;
        _myRarity = rarity;

        _specialEffects = specialEffects;

        _maxHealthPoint_RiseValue = maxHealthPoint_RiseValue;
        _maxStamina_RiseValue = maxStamina_RiseValue;
        _offensivePower_ShortDistance_RiseValue = offensivePower_ShortDistance_RiseValue;
        _offensivePower_LongDistance_RiseValue = offensivePower_LongDistance_RiseValue;
        _defensePower_RiseValue = defensePower_RiseValue;
        _moveSpeed_RiseValue = moveSpeed_RiseValue;
        _endurance_RiseValue = endurance_RiseValue;
    }

    /// <summary> 装備のID </summary>
    public enum EquipmentID
    {
        EQUIPMENT_ID_01,

        EQUIPMENT_ID_END
    }

    /// <summary> パーツのタイプ </summary>
    public enum EquipmentType
    {
        /// <summary> 腕 </summary>
        ARM_PARTS,
        /// <summary> 足 </summary>
        FOOT_PARTS,
        /// <summary> 頭 </summary>
        HEAD_PARTS,
        /// <summary> 胴体 </summary>
        TORSO_PARTS,

        PARTS_TYPE_END
    }

    /// <summary> パーツのレアリティ </summary>
    public enum EquipmentRarity { A,B,C,D,E}

    /// <summary> この装備のID </summary>
    public EquipmentID _myID { get; }
    /// <summary> この装備の種類 </summary>
    public EquipmentType _myType { get; }
    /// <summary> この装備のレアリティ </summary>
    public EquipmentRarity _myRarity { get; }
    /// <summary> この装備の名前 </summary>
    public string _myName { get; }
    /// <summary> この装備の特殊効果の効果量 </summary>
    public float _specialEffects { get; }

    //必要なパラメータ
    public float _maxHealthPoint_RiseValue { get; }//最大体力の上昇値
    public float _maxStamina_RiseValue { get; }//最大スタミナの上昇値
    public float _offensivePower_ShortDistance_RiseValue { get; }//近距離攻撃の攻撃力の上昇値
    public float _offensivePower_LongDistance_RiseValue { get; }// 遠距離攻撃の攻撃力の上昇値
    public float _defensePower_RiseValue { get; }//防御力の上昇値
    public float _moveSpeed_RiseValue { get; }//移動速度の上昇値

    public float _endurance_RiseValue { get; }//この装備の現在の耐久力


    //いらないと思ったので一旦コメントアウト

    //各値のSetter,Getter
    ///// <summary> 自分のID </summary>
    //public EquipmentID MyID { get => _myID; }
    ///// <summary> 自分の種類 </summary>
    //public EquipmentType MyType { get => _myType; }
    ///// <summary> 自分の名前 </summary>
    //public string MyName { get => _myName; }
    ///// <summary> 特殊効果の効果量 </summary>
    //public float SpecialEffects { get => _specialEffects; }


    ///// <summary> 最大体力の上昇値 </summary>
    //public float MaxHealthPoint_RiseValue { get => _maxHealthPoint_RiseValue;}
    ///// <summary> 最大スタミナの上昇値 </summary>
    //public float MaxStamina_RiseValue { get => _maxStamina_RiseValue;}
    ///// <summary> 近距離攻撃の攻撃力の上昇値 </summary>
    //public float OffensivePower_ShortDistance_RiseValue { get => _offensivePower_ShortDistance_RiseValue;}
    ///// <summary> 遠距離攻撃の攻撃力の上昇値 </summary>
    //public float OffensivePower_LongDistance_RiseValue { get => _offensivePower_LongDistance_RiseValue;}
    ///// <summary> 防御力の上昇値 </summary>
    //public float DefensePower { get => _defensePower_RiseValue;}
    ///// <summary> 移動速度の上昇値 </summary>
    //public float MoveSpeed_RiseValue { get => _moveSpeed_RiseValue;}

    ///// <summary> この装備の現在の耐久力 </summary>
    //public float Endurance { get => _endurance_RiseValue;}
}

