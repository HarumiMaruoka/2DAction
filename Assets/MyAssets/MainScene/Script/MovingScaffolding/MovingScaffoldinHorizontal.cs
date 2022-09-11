using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ��������𐧌䂷��R���|�[�l���g : ����������
/// </summary>
public class MovingScaffoldinHorizontal : MonoBehaviour
{
    [Header("���̈ړ���"), SerializeField] float _endPointX;
    [Header("�|���鎞��"), SerializeField] float _moveTimeX;


    void Start()
    {
        transform.
        DOMoveX(_endPointX, _moveTimeX). // X�����Ɉړ�����B_endPointX �Ɍ����� _moveTimeX �b�Ԋ|���āB
        SetEase(Ease.Linear).            // �ړ��̃j���A���X��t����B �ꗗ �� https://game-ui.net/?p=835 
        SetLoops(-1, LoopType.Yoyo);     // ����J��Ԃ����B-1�Ŗ�����BLoopType�͌J��Ԃ����@�B
    }
    void OnDestroy()
    {
        // DOTween ���~�߂�i�~�߂Ȃ��ƌx�����o��j
        DOTween.KillAll();
    }
}
