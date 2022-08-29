using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary> 所持している装備のボタンを管理するクラス </summary>
public class ManagerOfPossessedEquipment : MonoBehaviour
{
    //<===== メンバー変数 =====>//
    [Header("ボタンの親となるべきコンテント"), SerializeField] Transform _content;
    [Header("装備ボタンのプレハブ"), SerializeField] GameObject _equipmentButtonPrefab;
    [Header("イベントシステム"), SerializeField] EventSystem _eventSystem;
    [Header("装備の情報を表示するテキストの親"), SerializeField] GameObject _equipmentInformationParents;
    [Header("選択中の装備の説明文を表示するエリアのゲームオブジェクト"), SerializeField] Text _ExplanatoryTextArea;

    GameObject[] _equipmentButtons;
    GameObject _beforeSelectedGameObject;
    EquipmentButton _beforeEquipmentButton;
    Text[] _riseValueTexts;

    //<===== unityメッセージ =====>//
    void Start()
    {
        Initialized_ThisClass();
    }
    void Update()
    {
        Update_DrawEquipmentInformation();
    }

    //<===== privateメンバー関数 =====>//
    /// <summary> このクラスの初期化関数 </summary>
    void Initialized_ThisClass()
    {
        //配列分のメモリを確保
        _equipmentButtons = new GameObject[EquipmentDataBase.Instance.MaxHaveValue];
        //所持できる数だけボタンを生成し、配列に保存しておく。
        for (int i = 0; i < EquipmentDataBase.Instance.MaxHaveValue; i++)
        {
            //生成処理。
            _equipmentButtons[i] = Instantiate(_equipmentButtonPrefab, Vector3.zero, Quaternion.identity, _content);
            //生成したボタンに値を設定する。
            Set_ValueToButton(i);
        }
        //装備の情報を表示するテキストオブジェクトを取得し、変数に保存しておく。
        _riseValueTexts = _equipmentInformationParents.transform.GetComponentsInChildren<Text>();
    }
    /// <summary> 全てのボタンに装備情報を設定する。 </summary>
    void Set_ValueToButtonALL()
    {
        for (int i = 0; i < EquipmentDataBase.Instance.MaxHaveValue; i++)
        {
            Set_ValueToButton(i);
        }
    }
    /// <summary> 特定のボタンに装備情報を設定する。 </summary>
    /// <param name="index"> 変更したいボタンのインデックス </param>
    void Set_ValueToButton(int index)
    {
        // 所持装備の情報を保管している場所から、装備のIDを取得する。
        int thisEquipmentID = EquipmentDataBase.Instance.HaveEquipmentID._equipmentsID[index];
        // -1なら所持していないのでnullを設定する。そうでなければ、ボタンに装備情報をセットする。
        if (thisEquipmentID != -1) _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(EquipmentDataBase.Instance.EquipmentData[thisEquipmentID]);
        else _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(null);
    }
    /// <summary>「装備する」ボタンをアクティブにする </summary>
    /// <param name="equipmentButton"> 対象の「装備」ボタン </param>
    void OnEnabled_EquipButton(EquipmentButton equipmentButton)
    {
        if (equipmentButton._myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
        {
            equipmentButton.OnEnabled_EquipButton_OtherArm();
        }
        else
        {
            equipmentButton.OnEnabled_EquipButton_LeftArm();
            equipmentButton.OnEnabled_EquipButton_RightArm();
        }
    }
    /// <summary>「装備する」ボタンを非アクティブにする </summary>
    /// <param name="equipmentButton"> 対象の「装備」ボタン </param>
    void OffEnabled_EquipButton(EquipmentButton equipmentButton)
    {
        if (equipmentButton._myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
        {
            equipmentButton.OffEnabled_EquipButton_OtherArm();
        }
        else
        {
            equipmentButton.OffEnabled_EquipButton_LeftArm();
            equipmentButton.OffEnabled_EquipButton_RightArm();
        }
    }
    //<===== publicメンバー関数 =====>//
    /// <summary> 装備情報の表示を切り替える。 </summary>
    public void Update_DrawEquipmentInformation()
    {
        // 前フレームと今フレームで選択しているオブジェクトが異なる場合に処理を実行する。
        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject)
        {
            //カレントオブジェクトの処理
            if (_eventSystem.currentSelectedGameObject != null)
            {
                //「装備」ボタンの場合
                if (_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton currentEquipmentButton))
                {
                    Update_RiseValueText(currentEquipmentButton._myEquipment);

                    //現在選択中のパーツの「装備する」ボタンをアクティブにする。
                    OnEnabled_EquipButton(currentEquipmentButton);
                }
            }
            //「装備する」ボタンの場合
            if (_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipButton currentEquipButton))
            {
                //ここに処理を記述する。
            }

            //前フレームで選択されていたボタンの処理
            if (_beforeSelectedGameObject != null)
            {
                //「装備」ボタンの場合
                if (_beforeSelectedGameObject.TryGetComponent(out EquipmentButton beforeEquipmentButton))
                {
                    if ((_beforeSelectedGameObject.transform.parent.gameObject != _eventSystem.currentSelectedGameObject) &&
                        (_eventSystem.currentSelectedGameObject.transform.parent.gameObject != _beforeSelectedGameObject))
                    {
                        //親子関係でなければ、「装備する」ボタンを非アクティブにする。
                        OffEnabled_EquipButton(beforeEquipmentButton);
                    }
                }
                //「装備する」ボタンの場合
                if (_beforeSelectedGameObject.TryGetComponent(out EquipButton beforeEquipButton))
                {
                    foreach (var button in beforeEquipButton.transform.parent.GetComponentsInChildren<EquipButton>())
                    {
                        if (button.gameObject.activeSelf) button.gameObject.SetActive(false);
                    }
                }
            }
        }

        //古いオブジェクトを保存しておく。
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
    /// <summary> 上昇値テキストを更新する </summary>
    /// <param name="equipment"> 上昇値を表示する装備 </param>
    public void Update_RiseValueText(Equipment equipment)
    {
        //装備の種類
        _riseValueTexts[0].text = "装備の種類 : " + equipment._myTypeName;

        var parameter = equipment.ThisEquipment_StatusRisingValue;
        //最大体力の増加量を設定
        _riseValueTexts[1].text = $"最大体力の上昇値 : {parameter._maxHp}";
        //最大スタミナの増加量を設定
        _riseValueTexts[2].text = $"最大スタミナの上昇値 : {parameter._maxStamina}";
        //近距離攻撃力の増加量を設定
        _riseValueTexts[3].text = $"近距離攻撃力の上昇値 : {parameter._shortRangeAttackPower}";
        //遠離攻撃力の増加量を設定
        _riseValueTexts[4].text = $"遠距離攻撃力の上昇値 : {parameter._longRangeAttackPower}";
        //防御力の増加量を設定
        _riseValueTexts[5].text = $"防御力の上昇値 : {parameter._defensePower}";
        //移動速度の増加量を設定
        _riseValueTexts[6].text = $"移動速度の上昇値 : {parameter._moveSpeed}";
        //吹っ飛びにくさの増加量を設定
        _riseValueTexts[7].text = $"吹っ飛びにくさの上昇値 : {parameter._difficultToBlowOff}";
        //説明文を設定
        _ExplanatoryTextArea.text = equipment._explanatoryText;
    }
}

