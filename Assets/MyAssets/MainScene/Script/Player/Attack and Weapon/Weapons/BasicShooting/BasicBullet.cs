using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BasicShootingが放つ弾が持つコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BasicBullet : LongRangeWeaponBase
{
    [Header("弾の速度"), SerializeField] float _moveSpeed = 5f;
    [Header("ダッシュ時の加速度"), SerializeField] float _dashAcceleration = 1.5f;
    [Header("消滅までの時間"), SerializeField] float _dethTime = 1.5f;

    [Header("発射高さオフセット"), SerializeField]
    float _shotPosOffsetY = -0.2f;

    IEnumerator _coroutine = default;
    Coroutine _coroutine2 = default;

    const float RIGHT = 1f;
    const float LEFT = -1f;

    Vector2 _velocity = default;
    Rigidbody2D _rb2D;
    float _angularVelocity;
    bool _isStop;

    void Start()
    {
        Initialized();
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
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.GROUND_TAG_NAME)
        {
            Destroy(gameObject);
        }
        base.OnTriggerEnter2D(collision);
    }

    protected override bool Initialized()
    {

        transform.position += Vector3.up * _shotPosOffsetY;
        _rb2D = GetComponent<Rigidbody2D>();
        //プレイヤーの向きに応じて飛んでいく方向を決める。
        _rb2D.velocity =
            Vector2.right * _moveSpeed * (PlayerStatusManager.Instance.IsRight ? LEFT : RIGHT) + Vector2.right * GameObject.Find("ChibiRobo").GetComponent<Rigidbody2D>().velocity.x;
        if (PlayerStatusManager.Instance.IsRight)
        {
            var l = transform.localScale;
            l.x *= -1;
            transform.localScale = l;
        }
        // 発射音を鳴らす
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);

        // デストロイするまで待つ
        _coroutine = WaitDestroy();
        StartCoroutine(_coroutine);

        // 現在コルーチンを使用して弾を消失させているが、
        // 弾を少しづつ小さくして消失させるという表現の方がよいと思うので、
        // それをDOTweenを使用して実装しよう。

        return true;
    }
    /// <summary> ポーズ開始 </summary>
    void OnPause()
    {
        StopCoroutine(_coroutine);

        _angularVelocity = _rb2D.angularVelocity;
        _velocity = _rb2D.velocity;
        _rb2D.Sleep();
        _rb2D.simulated = false;
    }
    /// <summary> ポーズ解除 </summary>
    void OnResume()
    {
        _rb2D.simulated = true;
        _rb2D.WakeUp();
        _rb2D.angularVelocity = _angularVelocity;
        _rb2D.velocity = _velocity;

        StartCoroutine(_coroutine);
    }


    //<===== コルーチン =====>//
    IEnumerator WaitDestroy()
    {
        float timer = 0f;

        // コルーチン進行中は数値を増加させる
        while (timer < _dethTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
