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
        //必要であれば、スクロールする。
        if (!SelectedObjectInRectTop())
        {
            ScrollViewTop();
        }
        if (!SelectedObjectInRectBottom())
        {
            ScrollViewBottom();
        }
    }

    /// <summary> 選択中のButtonが枠より上に、はみ出していないか判定する </summary>
    /// <returns> 範囲内であればtrueを返す </returns>
    bool SelectedObjectInRectTop()
    {
        //範囲内になければfalseを返す
        return false;
    }

    /// <summary> 選択中のButtonが枠より下に、はみ出していないか判定する </summary>
    bool SelectedObjectInRectBottom()
    {
        //範囲内になければfalseを返す
        return false;
    }

    /// <summary> SelectedObjectInRect()がfalseを返したらスクロールする </summary>
    void ScrollViewTop()
    {
        //Contentの位置を更新する
    }

    void ScrollViewBottom()
    {

    }
}
