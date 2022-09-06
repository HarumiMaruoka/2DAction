using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U������
/// </summary>
public class NewPlayerAttack : MonoBehaviour
{
    //<===== �����o�[�ϐ� =====>//

    /// <summary> <para>���r�ɑ������ꂽ����̍U�������B�f���Q�[�g�ϐ��B</para>
    /// <para>"���N���b�N���Ɉ�񂾂����s����B"</para> </summary>
    static public System.Action _playerLeftArmWeapon_Moment;
    /// <summary> <para>�E�r�ɑ������ꂽ����̍U�������B�f���Q�[�g�ϐ��B</para>
    /// <para>"�E�N���b�N���Ɉ�񂾂����s����B"</para> </summary>
    static public System.Action _playerRightArmWeapon_Moment;

    /// <summary> <para>���r�ɑ������ꂽ����̍U�������B�f���Q�[�g�ϐ��B</para>
    /// <para>"���N���b�N�������Ǝ��s����B"</para> </summary>
    static public System.Action _playerLeftArmWeapon_Consecutively;
    /// <summary> <para>�E�r�ɑ������ꂽ����̍U�������B�f���Q�[�g�ϐ��B</para>
    /// <para>"�E�N���b�N�������Ǝ��s����B"</para> </summary>
    static public System.Action _playerRightArmWeapon_Consecutively;

    /// <summary> FireOne���͔���p </summary>
    bool _isFireOneDown = false;
    /// <summary> FireTow���͔���p </summary>
    bool _isFireTowDown = false;
    /// <summary> FireOne���͔���p </summary>
    bool _isFireOneDownNow = false;
    /// <summary> FireTow���͔���p </summary>
    bool _isFireTowDownNow = false;

    [Header("���N���b�N�̃{�^���̖��O"), SerializeField] string FireOneButtonName = "";
    [Header("�E�N���b�N�̃{�^���̖��O"), SerializeField] string FireTowButtonName = "";

    //<===== Unity���b�Z�[�W =====>//
    void Start()
    {

    }
    void Update()
    {
        Input_Attack();
        Update_Attack();
    }
    //<===== private�����o�[�֐� =====>//
    /// <summary> ���͏��� </summary>
    void Input_Attack()
    {
        _isFireOneDown = Input.GetButtonDown(FireOneButtonName);
        _isFireTowDown = Input.GetButtonDown(FireTowButtonName);
        _isFireOneDownNow = Input.GetButton(FireOneButtonName);
        _isFireTowDownNow = Input.GetButton(FireTowButtonName);
    }
    /// <summary> �X�V���� </summary>
    void Update_Attack()
    {
        // ���N���b�N�������̏��� �f���Q�[�g�ϐ��ɓo�^���ꂽ���������s����B
        if (_isFireOneDown && _playerLeftArmWeapon_Moment != null)
        {
            _playerLeftArmWeapon_Moment();
        }
        // �E�N���b�N�������̏��� �f���Q�[�g�ϐ��ɓo�^���ꂽ���������s����B
        if (_isFireTowDown && _playerRightArmWeapon_Moment != null)
        {
            _playerRightArmWeapon_Moment();
        }
        // ���N���b�N�������̏��� �f���Q�[�g�ϐ��ɓo�^���ꂽ���������s����B
        if (_isFireOneDownNow && _playerLeftArmWeapon_Consecutively != null)
        {
            _playerLeftArmWeapon_Consecutively();
        }
        // �E�N���b�N�������̏��� �f���Q�[�g�ϐ��ɓo�^���ꂽ���������s����B
        if (_isFireTowDownNow && _playerRightArmWeapon_Consecutively != null)
        {
            _playerRightArmWeapon_Consecutively();
        }
    }
}
