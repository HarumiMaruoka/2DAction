using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム使用による、
/// 攻撃力上昇を表す為のコンポーネント
/// </summary>
public class UseOffensivePowerItem : MonoBehaviour
{
    //===== フィールド / プロパティ =====//
    [Header("このアイテムのステータス")]
    [Tooltip("消滅までの時間"), SerializeField]
    float _destroyTime = 10f;
    [Tooltip("強化倍率(%) : 100(%)で2倍"), SerializeField]
    int _powerUpValue = 100;
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
        UseItemManager.Instance.OffensivePowerUpValueChange(_powerUpValue);
        while (timer < _destroyTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        UseItemManager.Instance.OffensivePowerUpValueChange(-_powerUpValue);
        Destroy(gameObject);
    }
}
