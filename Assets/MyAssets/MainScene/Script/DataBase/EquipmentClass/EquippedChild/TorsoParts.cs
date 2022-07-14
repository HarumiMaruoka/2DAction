using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorsoParts : Equipment
{
    //コンストラクタ
    TorsoParts(EquipmentID id, EquipmentType type, EquipmentRarity rarity, string name, float specialEffects = 0f,
        float maxHealthPoint_RiseValue = 0f, float maxStamina_RiseValue = 0f,
        float offensivePower_ShortDistance_RiseValue = 0f, float offensivePower_LongDistance_RiseValue = 0f,
        float defensePower_RiseValue = 0f, float moveSpeed_RiseValue = 0f, float endurance_RiseValue = 0f)
        : base(id, type, name, rarity, specialEffects, maxHealthPoint_RiseValue, maxStamina_RiseValue,
            offensivePower_ShortDistance_RiseValue, offensivePower_LongDistance_RiseValue,
            defensePower_RiseValue, moveSpeed_RiseValue, endurance_RiseValue)
    {

    }
}
