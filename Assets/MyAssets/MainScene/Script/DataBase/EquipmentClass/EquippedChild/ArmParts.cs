using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmParts : Equipment
{
    //コンストラクタ
    public ArmParts(
        EquipmentManager.EquipmentID id,
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
        AttackType attackType = AttackType.ERROR
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
    }

    public enum AttackType
    {
        Nomal,
        Unexpected,//テスト用。仮の値です。(つまり未想定)

        AttackType_END,
        ERROR,
    }

    AttackType _myAttackType;

    public static AttackType Get_AttackType(string str)
    {
        switch (str)
        {
            case "NomalShot": return AttackType.Nomal;

            case "Unexpected (未想定)":Debug.Log("テスト用の武器が読み込まれました。"); return AttackType.Unexpected;
        }

        Debug.LogError("不正な値です。");
        return AttackType.ERROR;
    }
}
