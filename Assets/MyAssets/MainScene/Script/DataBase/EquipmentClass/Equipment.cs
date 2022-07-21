using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 装備の基底クラス:各装備クラスはこのクラスを継承する </summary>
abstract public class Equipment
{
    /// <summary> 装備のコンストラクタ </summary>
    /// <param name="id"> ID </param>
    /// <param name="type"> Type (種類) </param>
    /// <param name="name"> 名前 </param>
    /// <param name="rarity"> レアリティ </param>
    /// <param name="specialEffects"> 特殊効果の効果量 </param>
    /// <param name="endurance_RiseValue"> 現在の耐久力 </param>
    /// <==以下増加量==>
    /// <param name="maxHealthPoint_RiseValue"> 最大体力 </param>
    /// <param name="maxStamina_RiseValue"> 最大スタミナ </param>
    /// <param name="offensivePower_ShortDistance_RiseValue"> 近距離攻撃力 </param>
    /// <param name="offensivePower_LongDistance_RiseValue"> 遠距離攻撃力 </param>
    /// <param name="defensePower_RiseValue"> 防御力 </param>
    /// <param name="moveSpeed_RiseValue"> 移動速度 </param>
    /// <param name="seriesName"> シリーズ名 </param>
    public Equipment(EquipmentManager.EquipmentID id, EquipmentType type, string name,
        EquipmentRarity rarity, float specialEffects = 0f, float endurance_RiseValue = 0f,
        float maxHealthPoint_RiseValue = 0f, float maxStamina_RiseValue = 0f,
        float offensivePower_ShortDistance_RiseValue = 0f, float offensivePower_LongDistance_RiseValue = 0f,
        float defensePower_RiseValue = 0f, float moveSpeed_RiseValue = 0f,
        string explanatoryText = "",
        string seriesName = "")
    {
        _myID = id;
        _myType = type;
        _myName = name;
        _myRarity = rarity;

        _specialEffects = specialEffects;
        _endurance_RiseValue = endurance_RiseValue;
        _explanatoryText = explanatoryText;
        _seriesName = seriesName;

        _maxHealthPoint_RiseValue = maxHealthPoint_RiseValue;
        _maxStamina_RiseValue = maxStamina_RiseValue;
        _offensivePower_ShortDistance_RiseValue = offensivePower_ShortDistance_RiseValue;
        _offensivePower_LongDistance_RiseValue = offensivePower_LongDistance_RiseValue;
        _defensePower_RiseValue = defensePower_RiseValue;
        _moveSpeed_RiseValue = moveSpeed_RiseValue;
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
    public enum EquipmentRarity { A, B, C, D, E, ERROR }

    /// <summary> この装備のID </summary>
    public EquipmentManager.EquipmentID _myID { get; }
    /// <summary> この装備の種類 </summary>
    public EquipmentType _myType { get; }
    /// <summary> この装備のレアリティ </summary>
    public EquipmentRarity _myRarity { get; }
    /// <summary> この装備の名前 </summary>
    public string _myName { get; }

    /// <summary> この装備の特殊効果の効果量 </summary>
    public float _specialEffects { get; }
    /// <summary> この装備の最大耐久力 </summary>
    public float _endurance_RiseValue { get; }
    /// <summary> 説明文 </summary>
    public string _explanatoryText { get; }

    //必要なパラメータ
    /// <summary> 最大体力の上昇値 </summary>
    public float _maxHealthPoint_RiseValue { get; }
    /// <summary> 最大スタミナの上昇値 </summary>
    public float _maxStamina_RiseValue { get; }
    /// <summary> 近距離攻撃の攻撃力の上昇値 </summary>
    public float _offensivePower_ShortDistance_RiseValue { get; }
    /// <summary> 遠距離攻撃の攻撃力の上昇値 </summary>
    public float _offensivePower_LongDistance_RiseValue { get; }
    /// <summary> 防御力の上昇値 </summary>
    public float _defensePower_RiseValue { get; }
    /// <summary> 移動速度の上昇値 </summary>
    public float _moveSpeed_RiseValue { get; }
    /// <summary> シリーズ名(同一名なら同じシリーズ。) </summary>
    public string _seriesName { get; }
}

