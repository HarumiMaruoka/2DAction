using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ʏ�ˌ��U���N���X : ����x�[�X���p������B
/// </summary>
public class BasicShooting : FireBehavior
{
    //<===== �����o�[�ϐ� =====>//
    [Header("�e�̃v���n�u"), SerializeField] GameObject _bulletPrefab = default;
    /// <summary> ���ˊԊu </summary>
    [Header("���ˊԊu"), SerializeField] float _fireInterval = 1f;
    /// <summary> �e�����Ă邩�ǂ�����\���ϐ��B </summary>
    bool _isFire = true;

    //<===== Unity���b�Z�[�W =====>//
    void Start()
    {
        Initialized(Constants.ON_FIRE_PRESS_TYPE_CONSECUTIVELY);

        //***�e�X�g�p����***//
        //*���r�ɑ�������B*//
        SetEquip_LeftArm();
        //******************//
    }
    void Update()
    {

    }

    //<===== overrides =====>//
    protected override bool Initialized(bool pressType)
    {
        return base.Initialized(pressType);
    }
    protected override void OnFire_ThisWeapon()
    {
        // �U���\�ł���Ύ��s����
        if (_isFire)
        {
            // �e�𐶐�����B
            Instantiate(_bulletPrefab, transform);
            // �C���^�[�o����҂B
            StartCoroutine(WaitInterval());
        }
    }

    //<===== �R���[�`�� =====>//
    /// <summary> �ˌ��̃C���^�[�o����҂B </summary>
    IEnumerator WaitInterval()
    {
        _isFire = false;
        yield return new WaitForSeconds(_fireInterval);
        _isFire = true;
    }
}
