using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmParts : Equipment
{
    //コンストラクタ
    public ArmParts(
        EquipmentDataBase.EquipmentID id,
        EquipmentType type,
        string name,
        EquipmentRarity rarity,
        float specialEffects = 0f,
        float endurance_RiseValue = 0f,
        float maxHealthPoint_RiseValue = 0f,
        float maxStamina_RiseValue = 0f,
        float offensivePower_ShortDistance_RiseValue = 0f,
        float offensivePower_LongDistance_RiseValue = 0f,
        float defensePower_RiseValue = 0f,
        float moveSpeed_RiseValue = 0f,
        float difficultToBlowOff = 0f,
        string explanatoryText = "",
        string seriesName = "",
        WeaponID attackType = WeaponID.ERROR,
        float attackPower = 0f
        ) : base(
            id,
            type,
            name,
            rarity,
            specialEffects,
            endurance_RiseValue,
            maxHealthPoint_RiseValue,
            maxStamina_RiseValue,
            offensivePower_ShortDistance_RiseValue,
            offensivePower_LongDistance_RiseValue,
            defensePower_RiseValue,
            moveSpeed_RiseValue,
            difficultToBlowOff,
            explanatoryText,
            seriesName
            )
    {
        _myAttackType = attackType;
        _myAttackPower = attackPower;
    }

    /// <summary> 武器の種類を表すenum </summary>
    public enum WeaponID
    {
        WeaponID_00_NomalShotAttack,
        WeaponID_01_NomalSlashingAttack,

        Unexpected,//テスト用。仮の値です。(つまり未想定)

        AttackType_END,
        ERROR,
    }
    /// <summary> 自身の武器の種類 </summary>
    WeaponID _myAttackType;
    /// <summary> 自身の攻撃力 </summary>
    public float _myAttackPower { get; }

    public static WeaponID Get_AttackType(string str)
    {
        switch (str)
        {
            case "NomalShot": return WeaponID.WeaponID_00_NomalShotAttack;
            case "NomalSlashing": return WeaponID.WeaponID_01_NomalSlashingAttack;

            case "Unexpected (未想定)": Debug.Log("テスト用の武器が読み込まれました。"); return WeaponID.Unexpected;
        }

        Debug.LogError("不正な値です。");
        return WeaponID.ERROR;
    }
}
