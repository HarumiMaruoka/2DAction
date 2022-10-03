using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "アイテムの使用"をゲームオブジェクトで <br/>
/// 表現する場合に継承すべきクラス。<br/>
/// </summary>
public class UseItemBehavior : MonoBehaviour
{
    //===== フィールド / プロパティ =====//
    /// <summary> このクラスで使用するコルーチンの保存先。</summary>
    IEnumerator _waitEffectTimeCoroutine = default;
    [Tooltip("消滅までの時間"), SerializeField]
    protected float _destroyTime = 10f;
    [Tooltip("効果量"), SerializeField]
    protected float _effectValue = 10f;

    //===== Unityメッセージ =====//
    protected virtual void Start()
    {
        // コルーチンを変数に登録する。
        _waitEffectTimeCoroutine = WaitEffectTime();
        StartCoroutine(_waitEffectTimeCoroutine);
        // アイテム使用時はポーズ中なのでポーズ処理も行う。
        OnPause();
    }
    protected virtual void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    protected virtual void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }

    //===== 仮想関数 =====//
    /// <summary>
    /// ポーズ処理
    /// </summary>
    protected virtual void OnPause()
    {
        StopCoroutine(_waitEffectTimeCoroutine);
    }
    /// <summary>
    /// ポーズ解除処理
    /// </summary>
    protected virtual void OnResume()
    {
        StartCoroutine(_waitEffectTimeCoroutine);
    }
    /// <summary>
    /// 効果の適用開始処理。<br/>
    /// 継承先で独自の処理を実装してください。
    /// </summary>
    protected virtual void OnEffect() { }
    /// <summary>
    /// 効果の適用解除処理。 <br/>
    /// 継承先で独自の処理を実装してください。
    /// </summary>
    protected virtual void OffEffect() { }

    //===== コルーチン =====//
    protected IEnumerator WaitEffectTime()
    {
        float timer = 0f;
        OnEffect();
        while (timer < _destroyTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        OffEffect();
        Destroy(gameObject);
    }
}
