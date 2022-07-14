using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw_NowEquipped : MonoBehaviour
{
    //<==========インスペクタから設定すべき値==========>//
    [Header("体を表示する場所"), SerializeField] Image _bodyPartsImageArea;
    [Header("頭パーツの情報を表示する場所"), SerializeField] Image _headPartsImageArea;
    [Header("胴パーツの情報を表示する場所"), SerializeField] Image _torsoPartsImageArea;
    [Header("腕パーツの情報を表示する場所"), SerializeField] Image _armPartsImageArea;
    [Header("足パーツの情報を表示する場所"), SerializeField] Image _legPartsImageArea;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
