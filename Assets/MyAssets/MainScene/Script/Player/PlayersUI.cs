using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersUI : MonoBehaviour
{
    //�X���C�_�[���擾
    [SerializeField] Slider _hitPointSlider;
    [SerializeField] Slider _hoverSlider;
    PlayerBasicInformation _playerBasicInformation;

    void Start()
    {
        //�R���|�[�l���g���擾
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();

        //HP�p�X���C�_�[
        _hitPointSlider.maxValue = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
        _hitPointSlider.minValue = 0;
        //�z�o�[�p�X���C�_�[�̏�����
        _hoverSlider.maxValue = _playerBasicInformation.MaxHealthForHover;
        _hoverSlider.minValue = 0;
    }

    private void FixedUpdate()
    {
        // �ő�̗͂��ύX���ꂽ��X���C�_�[�̍ő�l�̐ݒ肷��B
        if (Mathf.Abs(_hitPointSlider.maxValue - PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp) > 0.01f)
        {
            _hitPointSlider.maxValue = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
        }
        // �̗͂��ύX���ꂽ��X���C�_�[�̒l��ύX����B
        if (Mathf.Abs(_hitPointSlider.value - PlayerStatusManager.Instance.PlayerHealthPoint) > 0.01f)
        {
            _hitPointSlider.value = PlayerStatusManager.Instance.PlayerHealthPoint;
        }
        _hoverSlider.value = _playerBasicInformation._hoverValue;
    }
}
