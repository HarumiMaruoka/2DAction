using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScrollViewController : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        //�K�v�ł���΁A�X�N���[������B
        if (!SelectedObjectInRectTop())
        {
            ScrollViewTop();
        }
        if (!SelectedObjectInRectBottom())
        {
            ScrollViewBottom();
        }
    }

    /// <summary> �I�𒆂�Button���g����ɁA�͂ݏo���Ă��Ȃ������肷�� </summary>
    /// <returns> �͈͓��ł����true��Ԃ� </returns>
    bool SelectedObjectInRectTop()
    {
        //�͈͓��ɂȂ����false��Ԃ�
        return false;
    }

    /// <summary> �I�𒆂�Button���g��艺�ɁA�͂ݏo���Ă��Ȃ������肷�� </summary>
    bool SelectedObjectInRectBottom()
    {
        //�͈͓��ɂȂ����false��Ԃ�
        return false;
    }

    /// <summary> SelectedObjectInRect()��false��Ԃ�����X�N���[������ </summary>
    void ScrollViewTop()
    {
        //Content�̈ʒu���X�V����
    }

    void ScrollViewBottom()
    {

    }
}
