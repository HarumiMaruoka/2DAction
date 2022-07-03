using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemScrollViewController : MonoBehaviour
{
    [SerializeField] float _lineHeight;

    bool _firstAdjustment = false;//�ŏ��͒������Ȃ�

    GameObject _currentButton;
    GameObject _beforeButton;

    void Start()
    {

    }

    void Update()
    {
        //�Ȃ����ŏ��̈�T�ڂ͂ł���߂Ȑ����������Ă���̂Œ������Ȃ�
        if (!_firstAdjustment)
        {
            _firstAdjustment = true;
        }
        else
        {
            _currentButton = EventSystem.current.currentSelectedGameObject;
            //�K�v�ł���΁A�X�N���[������B
            if (_currentButton != _beforeButton)
            {
                Update_ScrollPos();
            }
            _beforeButton = _currentButton;
        }
    }


    void Update_ScrollPos()
    {
        if (_currentButton != null)
        {
            //��ɂ͂ݏo���Ă��Ȃ������肷��
            if (GetComponent<RectTransform>().anchoredPosition.y >//�g�̏��
                -_currentButton.GetComponent<RectTransform>().anchoredPosition.y)//�{�^���̏��(�g�����̒l�ɑ傫���Ȃ�̂ɑ΂��āA�{�^���͕��̒l���傫���Ȃ�̂ŁA�Е������������]����)
            {
                //�͂ݏo���Ă���Έʒu�𒲐�����
                GetComponent<RectTransform>().anchoredPosition = Vector2.up * -_currentButton.GetComponent<RectTransform>().anchoredPosition;
            }

            //���ɂ͂ݏo���Ă��Ȃ������肷��
            if (GetComponent<RectTransform>().anchoredPosition.y + 500 <//�g�̉���(500��ScrollView��Height�B�V���A���C�Y�Ŏ���Ă���悤�ύX����)
                -(_currentButton.GetComponent<RectTransform>().anchoredPosition.y - _lineHeight))//�{�^���̉���
            {
                //�͂ݏo���Ă���Έʒu�𒲐�����
                GetComponent<RectTransform>().anchoredPosition = Vector3.up * -(_currentButton.GetComponent<RectTransform>().anchoredPosition.y + 500f - _lineHeight);
            }
        }
    }
}
