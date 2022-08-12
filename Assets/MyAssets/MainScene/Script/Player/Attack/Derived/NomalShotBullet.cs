using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ��{�ˌ��U���R���|�[�l���g </summary>
public class NomalShotBullet : PlayerWeaponBase
{
    //<======= �����o�[�ϐ� =======>//
    [Header("�e�̑��x"), SerializeField] float _bulletSpeed = 10f;
    [Header("�ˌ��̃C���^�[�o��"), SerializeField] float _interval = 1f;
    [Header("���Ă邩�ǂ����̐^�U�l"), SerializeField] bool _isFire = true;
    [Header("�e�̃v���n�u"), SerializeField] GameObject _bulletPrefab;

    protected override void WeaponInit()
    {
        //�e(�v���C���[)�̃g�����X�t�H�[�����擾����B
        _playerPos = transform.parent;
        //����ID��ݒ肷��B
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_00_NomalShotAttack;
        //PressType��ݒ肷��B
        _isPressType = false;
    }

    private void Start()
    {
        WeaponInit();
    }

    /// <summary> Fire�{�^����������Ă���Ԃ̏��� : ���̊֐��́A�f���Q�[�g�ϐ��ɓo�^���Ăяo���B </summary>
    public override void Run_FireProcess()
    {
        //�ʒu���X�V����B
        UpdatePosition();
        //���ˏ������s���B
        if (_isFire && _bulletPrefab)
        {
            Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            StartCoroutine(WaitForInterval());
        }
    }

    /// <summary> �C���^�[�o����҂B </summary>
    IEnumerator WaitForInterval()
    {
        _isFire = false;
        yield return new WaitForSeconds(_interval);
        _isFire = true;
    }
}
