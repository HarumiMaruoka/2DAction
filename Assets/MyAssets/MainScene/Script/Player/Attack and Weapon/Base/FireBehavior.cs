using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�������ł���S�Ă̕���̊��N���X�B
/// </summary>
public abstract class FireBehavior : MonoBehaviour
{
    //<===== �����o�[�ϐ� =====>//
    /// <summary> 
    /// ���̓^�C�v : <br/>
    /// true�Ȃ牟�����̂݁Afalse�Ȃ牟���������Ǝ��s���邱�Ƃ�\���B <br/>
    /// </summary>
    protected bool _pressType;

    //<===== protected�����o�[�֐� =====>//
    /// <summary> ���r�ɑ������鏈���B </summary>
    public void SetEquip_LeftArm()
    {
        // ���̓^�C�v�ɉ����āA�Ή�������̃f���Q�[�g�ϐ��ɓo�^����B
        // ���Α��̃f���Q�[�g�ϐ��͔j������B
        if (_pressType)
        {
            NewPlayerAttack._playerLeftArmWeapon_Moment = OnFire_ThisWeapon;
            NewPlayerAttack._playerLeftArmWeapon_Consecutively = null;
        }
        else
        {
            NewPlayerAttack._playerLeftArmWeapon_Moment = null;
            NewPlayerAttack._playerLeftArmWeapon_Consecutively = OnFire_ThisWeapon;
        }
    }
    /// <summary> �E�r�ɑ������鏈���B </summary>
    public void SetEquip_RightArm()
    {
        if (_pressType)
        {
            NewPlayerAttack._playerRightArmWeapon_Moment = OnFire_ThisWeapon;
            NewPlayerAttack._playerRightArmWeapon_Consecutively = null;
        }
        else
        {
            NewPlayerAttack._playerRightArmWeapon_Moment = null;
            NewPlayerAttack._playerRightArmWeapon_Consecutively = OnFire_ThisWeapon;
        }
    }

    //<===== ���z�֐� =====>//
    /// <summary> ���������� </summary>
    /// <param name="pressType"> ���̓^�C�v : <br/>
    /// true�Ȃ牟�����̂݁Afalse�Ȃ牟���������Ǝ��s���邱�Ƃ�\���B <br/> </param>
    /// <returns> �������ɐ��������� true ,�����łȂ��ꍇ�� false ��Ԃ��B </returns>
    protected virtual bool Initialized(bool pressType)
    {
        _pressType = pressType;
        return true;
    }
    /// <summary> 
    /// �U������ : <br/>
    /// PlayerAttack�N���X�̃f���Q�[�g�ϐ��ɓo�^���A��������Ăяo���B<br/>
    /// </summary>
    protected virtual void OnFire_ThisWeapon() { }
}
