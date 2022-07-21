using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �����̊��N���X:�e�����N���X�͂��̃N���X���p������ </summary>
abstract public class Equipment
{
    /// <summary> �����̃R���X�g���N�^ </summary>
    /// <param name="id"> ID </param>
    /// <param name="type"> Type (���) </param>
    /// <param name="name"> ���O </param>
    /// <param name="rarity"> ���A���e�B </param>
    /// <param name="specialEffects"> ������ʂ̌��ʗ� </param>
    /// <param name="endurance_RiseValue"> ���݂̑ϋv�� </param>
    /// <==�ȉ�������==>
    /// <param name="maxHealthPoint_RiseValue"> �ő�̗� </param>
    /// <param name="maxStamina_RiseValue"> �ő�X�^�~�i </param>
    /// <param name="offensivePower_ShortDistance_RiseValue"> �ߋ����U���� </param>
    /// <param name="offensivePower_LongDistance_RiseValue"> �������U���� </param>
    /// <param name="defensePower_RiseValue"> �h��� </param>
    /// <param name="moveSpeed_RiseValue"> �ړ����x </param>
    /// <param name="seriesName"> �V���[�Y�� </param>
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

    /// <summary> �p�[�c�̃^�C�v </summary>
    public enum EquipmentType
    {
        /// <summary> �r </summary>
        ARM_PARTS,
        /// <summary> �� </summary>
        FOOT_PARTS,
        /// <summary> �� </summary>
        HEAD_PARTS,
        /// <summary> ���� </summary>
        TORSO_PARTS,

        PARTS_TYPE_END
    }

    /// <summary> �p�[�c�̃��A���e�B </summary>
    public enum EquipmentRarity { A, B, C, D, E, ERROR }

    /// <summary> ���̑�����ID </summary>
    public EquipmentManager.EquipmentID _myID { get; }
    /// <summary> ���̑����̎�� </summary>
    public EquipmentType _myType { get; }
    /// <summary> ���̑����̃��A���e�B </summary>
    public EquipmentRarity _myRarity { get; }
    /// <summary> ���̑����̖��O </summary>
    public string _myName { get; }

    /// <summary> ���̑����̓�����ʂ̌��ʗ� </summary>
    public float _specialEffects { get; }
    /// <summary> ���̑����̍ő�ϋv�� </summary>
    public float _endurance_RiseValue { get; }
    /// <summary> ������ </summary>
    public string _explanatoryText { get; }

    //�K�v�ȃp�����[�^
    /// <summary> �ő�̗͂̏㏸�l </summary>
    public float _maxHealthPoint_RiseValue { get; }
    /// <summary> �ő�X�^�~�i�̏㏸�l </summary>
    public float _maxStamina_RiseValue { get; }
    /// <summary> �ߋ����U���̍U���͂̏㏸�l </summary>
    public float _offensivePower_ShortDistance_RiseValue { get; }
    /// <summary> �������U���̍U���͂̏㏸�l </summary>
    public float _offensivePower_LongDistance_RiseValue { get; }
    /// <summary> �h��͂̏㏸�l </summary>
    public float _defensePower_RiseValue { get; }
    /// <summary> �ړ����x�̏㏸�l </summary>
    public float _moveSpeed_RiseValue { get; }
    /// <summary> �V���[�Y��(���ꖼ�Ȃ瓯���V���[�Y�B) </summary>
    public string _seriesName { get; }
}

