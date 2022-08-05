using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmParts : Equipment
{
    //�R���X�g���N�^
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
        Unexpected,//�e�X�g�p�B���̒l�ł��B(�܂薢�z��)

        AttackType_END,
        ERROR,
    }

    AttackType _myAttackType;

    public static AttackType Get_AttackType(string str)
    {
        switch (str)
        {
            case "NomalShot": return AttackType.Nomal;

            case "Unexpected (���z��)":Debug.Log("�e�X�g�p�̕��킪�ǂݍ��܂�܂����B"); return AttackType.Unexpected;
        }

        Debug.LogError("�s���Ȓl�ł��B");
        return AttackType.ERROR;
    }
}
