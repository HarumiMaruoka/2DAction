using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �����̊��N���X:�e�����N���X�͂��̃N���X���p������ </summary>
abstract public class Equipment
{
    /// <summary>
    /// �����̃R���X�g���N�^
    /// </summary>
    /// <param name="id"> ID </param>
    /// <param name="type"> Type </param>
    /// <param name="name"> ���O </param>
    /// <param name="specialEffects"> ������ʂ̌��ʗ�(��?) </param>
    /// <param name="maxHealthPoint_RiseValue"> �ő�̗͂̑����� </param>
    /// <param name="maxStamina_RiseValue"> �ő�X�^�~�i�̑����� </param>
    /// <param name="offensivePower_ShortDistance_RiseValue"> �ߋ����U���͂̑����� </param>
    /// <param name="offensivePower_LongDistance_RiseValue">�������U���͂̑�����</param>
    /// <param name="defensePower_RiseValue"> �h��͂̑����� </param>
    /// <param name="moveSpeed_RiseValue"> �ړ����x�̏㏸�� </param>
    /// <param name="endurance_RiseValue"> �ϋv�� </param>
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

    /// <summary> ������ID </summary>
    public enum EquipmentID
    {
        EQUIPMENT_ID_01,

        EQUIPMENT_ID_END
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
    public enum EquipmentRarity { A,B,C,D,E}

    /// <summary> ���̑�����ID </summary>
    public EquipmentID _myID { get; }
    /// <summary> ���̑����̎�� </summary>
    public EquipmentType _myType { get; }
    /// <summary> ���̑����̃��A���e�B </summary>
    public EquipmentRarity _myRarity { get; }
    /// <summary> ���̑����̖��O </summary>
    public string _myName { get; }
    /// <summary> ���̑����̓�����ʂ̌��ʗ� </summary>
    public float _specialEffects { get; }

    //�K�v�ȃp�����[�^
    public float _maxHealthPoint_RiseValue { get; }//�ő�̗͂̏㏸�l
    public float _maxStamina_RiseValue { get; }//�ő�X�^�~�i�̏㏸�l
    public float _offensivePower_ShortDistance_RiseValue { get; }//�ߋ����U���̍U���͂̏㏸�l
    public float _offensivePower_LongDistance_RiseValue { get; }// �������U���̍U���͂̏㏸�l
    public float _defensePower_RiseValue { get; }//�h��͂̏㏸�l
    public float _moveSpeed_RiseValue { get; }//�ړ����x�̏㏸�l

    public float _endurance_RiseValue { get; }//���̑����̌��݂̑ϋv��


    //����Ȃ��Ǝv�����̂ň�U�R�����g�A�E�g

    //�e�l��Setter,Getter
    ///// <summary> ������ID </summary>
    //public EquipmentID MyID { get => _myID; }
    ///// <summary> �����̎�� </summary>
    //public EquipmentType MyType { get => _myType; }
    ///// <summary> �����̖��O </summary>
    //public string MyName { get => _myName; }
    ///// <summary> ������ʂ̌��ʗ� </summary>
    //public float SpecialEffects { get => _specialEffects; }


    ///// <summary> �ő�̗͂̏㏸�l </summary>
    //public float MaxHealthPoint_RiseValue { get => _maxHealthPoint_RiseValue;}
    ///// <summary> �ő�X�^�~�i�̏㏸�l </summary>
    //public float MaxStamina_RiseValue { get => _maxStamina_RiseValue;}
    ///// <summary> �ߋ����U���̍U���͂̏㏸�l </summary>
    //public float OffensivePower_ShortDistance_RiseValue { get => _offensivePower_ShortDistance_RiseValue;}
    ///// <summary> �������U���̍U���͂̏㏸�l </summary>
    //public float OffensivePower_LongDistance_RiseValue { get => _offensivePower_LongDistance_RiseValue;}
    ///// <summary> �h��͂̏㏸�l </summary>
    //public float DefensePower { get => _defensePower_RiseValue;}
    ///// <summary> �ړ����x�̏㏸�l </summary>
    //public float MoveSpeed_RiseValue { get => _moveSpeed_RiseValue;}

    ///// <summary> ���̑����̌��݂̑ϋv�� </summary>
    //public float Endurance { get => _endurance_RiseValue;}
}

