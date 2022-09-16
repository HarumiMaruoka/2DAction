using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �萔���W�߂��N���X
/// </summary>
public class Constants
{
    /// <summary> ���r��\���萔�B </summary>
    public const int LEFT_ARM = 0;
    /// <summary> �E�r��\���萔�B </summary>
    public const int RIGHT_ARM = 1;
    /// <summary> �r�ȊO��\���萔�B </summary>
    public const int NOT_ARM = -1;
    /// <summary> �ω�����`�悷�� </summary>
    public const bool DRAW_AMPLITUDE = true;
    /// <summary> �ω�����\�����Ȃ� </summary>
    public const bool NOT_DRAW_AMPLITUDE = false;

    //�����֌W�̃N���X�Ŏg�p����萔
    public const int PLAYER_NAME_DRAW_AREA = 0;
    public const int EQUIPMENT_TYPE_DRAW_AREA = 0;
    public const int MAX_HP_DRAW_AREA = 1;
    public const int MAX_STAMINA_TYPE_DRAW_AREA = 2;
    public const int SHORT_RANGE_ATTACK_POWER_DRAW_AREA = 3;
    public const int LONG_RANGE_ATTACK_POWER_DRAW_AREA = 4;
    public const int DEFENSE_POWER_DRAW_AREA = 5;
    public const int MOVE_SPEED_DRAW_AREA = 6;
    public const int DIFFICULT_TO_BLOW_OFF_DRAW_AREA = 7;

    /// <summary> Fire�{�^�����u�Ԃ������s���邱�Ƃ�\���l�B</summary>
    public const bool ON_FIRE_PRESS_TYPE_MOMENT = true;
    /// <summary> Fire�{�^���������Ă���ԁA�p�����Ď��s���邱�Ƃ�\���l�B </summary>
    public const bool ON_FIRE_PRESS_TYPE_CONSECUTIVELY = false;

    //������\���萔
    public const float UP = 1f;
    public const float DOWN = -1f;
    public const float RIGHT = 1f;
    public const float LEFT = -1;

    public const string PLAYER_TAG_NAME = "Player";

    /// <summary> �A�j���[�V�����ʏ�Đ����̃X�s�[�h </summary>
    public const float NOMAL_ANIM_SPEED = 1f;
    /// <summary> �A�j���[�V�����t�Đ����̃X�s�[�h </summary>
    public const float REVERSE_PLAYBACK_ANIM_SPEED = -1f;
    /// <summary> �A�j���[�V�����ꎞ��~���̃X�s�[�h </summary>
    public const float PAUSE_ANIM_SPEED = 0f;
}
