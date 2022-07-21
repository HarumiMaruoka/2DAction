using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootParts : Equipment
{
    //コンストラクタ
    public FootParts(
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
        string explanatoryText = "",
        string seriesName = ""
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
            explanatoryText,
            seriesName
            )
    {

    }
}
