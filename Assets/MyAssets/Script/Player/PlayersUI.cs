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

    // Start is called before the first frame update
    void Start()
    {
        //�R���|�[�l���g���擾
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();

        //HP�p�X���C�_�[
        _hitPointSlider.maxValue = _playerBasicInformation._maxHitPoint;
        _hitPointSlider.minValue = 0;
        //�z�o�[�p�X���C�_�[�̏�����
        _hoverSlider.maxValue = _playerBasicInformation._maxHealthForHover;
        _hoverSlider.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _hitPointSlider.value = _playerBasicInformation._playerHitPoint;
        _hoverSlider.value = _playerBasicInformation._hoverValue;
    }
}
