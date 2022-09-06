using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��{�I�ȋߋ����U���N���X�B
/// </summary>
public class BasicShortRangeAttack : FireBehavior
{
    //<===== �����o�[�ϐ� =====>//
    [Header("�ߋ����U�� : �R���{�ꌂ�ڂ̃v���n�u"), SerializeField]
    GameObject _shortRangeAttack_FirstComboPrefab = default;
    [Header("�ߋ����U�� : �R���{�񌂖ڂ̃v���n�u"), SerializeField]
    GameObject _shortRangeAttack_SecondComboPrefab = default;
    [Header("�ߋ����U�� : �R���{�O���ڂ̃v���n�u"), SerializeField]
    GameObject _shortRangeAttack_ThirdComboPrefab = default;

    /// <summary> 2�R���{�ڂ����Ă邩�ǂ��� </summary>
    bool _isSecondCombo = false;
    /// <summary> 3�R���{�ڂ����Ă邩�ǂ��� </summary>
    bool _isThirdCombo = false;
    /// <summary> �R���{�Ԃ̃C���^�[�o�� </summary>
    [Header("�e�R���{�Ԃ̃C���^�[�o��"), SerializeField] float _intervalBetweenCombos = 0.8f;

    //<===== Unity���b�Z�[�W =====>//
    void Start()
    {
        Initialized(Constants.ON_FIRE_PRESS_TYPE_MOMENT);

        //***�e�X�g�p����***//
        //*�E�r�ɑ�������B*//
        SetEquip_RightArm();
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
        //3���ڂ�����
        if (_isThirdCombo)
        {
            Instantiate(_shortRangeAttack_ThirdComboPrefab);
            _isThirdCombo = false;
        }
        //2���ڂ�����
        else if (_isSecondCombo)
        {
            Instantiate(_shortRangeAttack_SecondComboPrefab);
            //3���ڏ�������
            StartCoroutine(ThirdShotReady());
            _isSecondCombo = false;
        }
        //1���ڂ�����
        else
        {
            Instantiate(_shortRangeAttack_FirstComboPrefab);
            //2���ڏ�������
            StartCoroutine(SecondShotReady());
        }
    }

    //<===== �R���[�`�� =====>//
    /// <summary> ��莞�ԁA2�R���{�ڂ����Ă�悤�ɂ���B </summary>
    IEnumerator SecondShotReady()
    {
        _isSecondCombo = true;
        yield return new WaitForSeconds(_intervalBetweenCombos);
        _isSecondCombo = false;
    }
    /// <summary> ��莞�ԁA3�R���{�ڂ����Ă�悤�ɂ���B </summary>
    IEnumerator ThirdShotReady()
    {
        _isThirdCombo = true;
        yield return new WaitForSeconds(_intervalBetweenCombos);
        _isThirdCombo = false;
    }
}
