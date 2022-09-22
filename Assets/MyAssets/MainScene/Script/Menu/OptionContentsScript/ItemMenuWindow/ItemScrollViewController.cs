using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムUIのスクロールを制御するコンポーネント
/// </summary>
public class ItemScrollViewController : UseEventSystemBehavior
{
    [SerializeField] float _lineHeight;

    bool _firstAdjustment = false;//最初は調整しない

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        //なぜか最初の一週目はでたらめな数字が入っているので調整しない
        if (!_firstAdjustment)
        {
            _firstAdjustment = true;
        }
        else
        {
            //必要であれば、スクロールする。
            if (_eventSystem.currentSelectedGameObject != _beforeSelectedGameObject)
            {
                Update_ScrollPos();
            }
            _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
        }
    }


    void Update_ScrollPos()
    {
        if (_eventSystem.currentSelectedGameObject != null)
        {
            //上にはみ出していないか判定する
            if (GetComponent<RectTransform>().anchoredPosition.y >//枠の上辺
                -_eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y)//ボタンの上辺(枠が正の値に大きくなるのに対して、ボタンは負の値が大きくなるので、片方だけ正負反転する)
            {
                //はみ出していれば位置を調整する
                GetComponent<RectTransform>().anchoredPosition = Vector2.up * -_eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;
            }

            //下にはみ出していないか判定する
            if (GetComponent<RectTransform>().anchoredPosition.y + 500 <//枠の下辺(500はScrollViewのHeight。シリアライズで取ってくるよう変更する)
                -(_eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y - _lineHeight))//ボタンの下辺
            {
                //はみ出していれば位置を調整する
                GetComponent<RectTransform>().anchoredPosition = Vector3.up * -(_eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition.y + 500f - _lineHeight);
            }
        }
    }
}
