using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムID06用に作成したコンポーネント。<br/>
/// 敵の移動速度を一定時間減らす。<br/>
/// </summary>
public class OldUseEnemyMoveSpeedDownItem : MonoBehaviour
{
    //===== フィールド / プロパティ =====//
    [Header("このアイテムのステータス")]
    [Tooltip("消滅までの時間"), SerializeField]
    float _destroyTime = 10f;
    [Tooltip("減退倍率(%) : 100(%)で0,50%で半分になる"), SerializeField]
    float _moveSpeedDownValue = 10f;
    /// <summary> コルーチン </summary>
    IEnumerator _waitDestroyCoroutine = default;


    //===== Unityメッセージ =====//
    void Start()
    {
        _waitDestroyCoroutine = WaitDestroy();
        StartCoroutine(_waitDestroyCoroutine);
        OnPause();
    }
    void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }

    //===== privateメソッド =====//
    void OnPause()
    {
        StopCoroutine(_waitDestroyCoroutine);
    }
    void OnResume()
    {
        StartCoroutine(_waitDestroyCoroutine);
    }

    //===== コルーチン =====//
    IEnumerator WaitDestroy()
    {
        float timer = 0f;
        UseItemManager.Instance.EnemyMoveSpeedDownValueChange(_moveSpeedDownValue);
        while (timer < _destroyTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        UseItemManager.Instance.EnemyMoveSpeedDownValueChange(-_moveSpeedDownValue);
        Destroy(gameObject);
    }
}
