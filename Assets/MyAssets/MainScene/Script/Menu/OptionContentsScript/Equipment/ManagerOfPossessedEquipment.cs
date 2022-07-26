using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary> 所持している装備のボタンを管理するクラス </summary>
public class ManagerOfPossessedEquipment : MonoBehaviour
{
    //<=========== 必要な値 ===========>//
    /// <summary> 装備ボタンの配列 </summary>
    GameObject[] _equipmentButtons;
    GameObject _beforeSelectedGameObject;
    Text[] _riseValueTexts;

    //<======== アサインすべき値 ========>//
    [Header("ボタンの親となるべきコンテント"), SerializeField] Transform _content;
    /// <summary> 装備ボタンのプレハブ </summary>
    [Header("装備ボタンのプレハブ"), SerializeField] GameObject _equipmentButtonPrefab;
    [Header("イベントシステム"), SerializeField] EventSystem _eventSystem;
    [Header("装備の情報を表示するテキストの親"), SerializeField] GameObject _equipmentInformationParents;
    [Header("選択中の装備の説明文を表示するエリアのゲームオブジェクト"), SerializeField] Text _ExplanatoryTextArea;

    //<===== インスペクタから設定すべき値 =====>//


    void Start()
    {
        _equipmentButtons = new GameObject[EquipmentManager.Instance.MaxHaveValue];
        //所持できる数だけボタンを生成しておく
        for (int i = 0; i < EquipmentManager.Instance.MaxHaveValue; i++)
        {
            //生成処理。
            _equipmentButtons[i] = Instantiate(_equipmentButtonPrefab, Vector3.zero, Quaternion.identity, _content);
            //生成したボタンに値を設定する。
            Set_ValueToButton(i);
        }

        _riseValueTexts = _equipmentInformationParents.transform.GetComponentsInChildren<Text>();
    }

    void Update()
    {

        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject && _eventSystem.currentSelectedGameObject?.GetComponent<EquipmentButton>())
        {
            //装備の種類
            _riseValueTexts[0].text = "装備の種類 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._myTypeName;
            //最大体力の増加量を設定
            _riseValueTexts[1].text = "最大体力の上昇値 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._maxHealthPoint_RiseValue.ToString();
            //最大スタミナの増加量を設定
            _riseValueTexts[2].text = "最大スタミナの上昇値 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._maxStamina_RiseValue.ToString();
            //近距離攻撃力の増加量を設定
            _riseValueTexts[3].text = "近距離攻撃力の上昇値 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._offensivePower_ShortDistance_RiseValue.ToString();
            //遠離攻撃力の増加量を設定
            _riseValueTexts[4].text = "遠距離攻撃力の上昇値 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._offensivePower_LongDistance_RiseValue.ToString();
            //防御力の増加量を設定
            _riseValueTexts[5].text = "防御力の上昇値 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._defensePower_RiseValue.ToString();
            //移動速度の増加量を設定
            _riseValueTexts[6].text = "移動速度の上昇値 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._moveSpeed_RiseValue.ToString();
            //吹っ飛びにくさの増加量を設定
            _riseValueTexts[7].text = "吹っ飛びにくさの上昇値 : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._defensePower_RiseValue.ToString();
            //説明文を設定
            _ExplanatoryTextArea.text = _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._explanatoryText;

        }

        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }

    /// <summary> 全てのボタンに装備情報を設定する。 </summary>
    void Set_ValueToButtonALL()
    {
        for (int i = 0; i < EquipmentManager.Instance.MaxHaveValue; i++)
        {
            Set_ValueToButton(i);
        }
    }

    /// <summary> 特定のボタンに装備情報を設定する。 </summary>
    /// <param name="index"> 変更したいボタンのインデックス </param>
    void Set_ValueToButton(int index)
    {
        // 所持装備の情報を保管している場所から、装備のIDを取得する。
        int thisEquipmentID = EquipmentManager.Instance.HaveEquipmentID._equipmentsID[index];
        // -1なら所持していないのでnullを設定する。そうでなければ、ボタンに装備情報をセットする。
        if (thisEquipmentID != -1) _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(EquipmentManager.Instance.EquipmentData[thisEquipmentID]);
        else _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(null);
    }
}
