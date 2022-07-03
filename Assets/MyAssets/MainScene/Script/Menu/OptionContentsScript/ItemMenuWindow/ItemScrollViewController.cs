using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemScrollViewController : MonoBehaviour
{
    [SerializeField] float _lineHeight;

    bool _firstAdjustment = false;//最初は調整しない

    GameObject _currentButton;
    GameObject _beforeButton;

    void Start()
    {

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
            _currentButton = EventSystem.current.currentSelectedGameObject;
            //必要であれば、スクロールする。
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
            //上にはみ出していないか判定する
            if (GetComponent<RectTransform>().anchoredPosition.y >//枠の上辺
                -_currentButton.GetComponent<RectTransform>().anchoredPosition.y)//ボタンの上辺(枠が正の値に大きくなるのに対して、ボタンは負の値が大きくなるので、片方だけ正負反転する)
            {
                //はみ出していれば位置を調整する
                GetComponent<RectTransform>().anchoredPosition = Vector2.up * -_currentButton.GetComponent<RectTransform>().anchoredPosition;
            }

            //下にはみ出していないか判定する
            if (GetComponent<RectTransform>().anchoredPosition.y + 500 <//枠の下辺(500はScrollViewのHeight。シリアライズで取ってくるよう変更する)
                -(_currentButton.GetComponent<RectTransform>().anchoredPosition.y - _lineHeight))//ボタンの下辺
            {
                //はみ出していれば位置を調整する
                GetComponent<RectTransform>().anchoredPosition = Vector3.up * -(_currentButton.GetComponent<RectTransform>().anchoredPosition.y + 500f - _lineHeight);
            }
        }
    }
}
