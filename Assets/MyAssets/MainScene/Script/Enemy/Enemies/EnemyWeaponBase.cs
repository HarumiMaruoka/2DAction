using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy / Bossが扱う武器の基底クラス : <br/>
/// プレイヤーに対する攻撃処理を記載。
/// </summary>
public class EnemyWeaponBase : MonoBehaviour, AttackOnPlayer
{
    //===== メンバー変数 =====//
    Collider2D _collider;

    [Header("攻撃力"), SerializeField] float _offensivePower;
    [Header("攻撃力"), SerializeField] float _blowingPower;

    //===== Unityメッセージ =====//
    /// <summary>
    /// 初期化処理。コライダーを取得する。
    /// </summary>
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    /// <summary> プレイヤーに接触したらプレイヤーの体力を減らす。 </summary>
    /// <param name="collision"> 接触相手 </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.PLAYER_TAG_NAME)
        {
            PlayerStatusManager.Instance.PlayerHealthPoint -= _offensivePower;
        }
        else
        {
            Debug.LogError($"エラー！\nプレイヤーのタグは、\"{Constants.PLAYER_TAG_NAME}\"ですか？");
        }
    }
    /// <summary> 
    /// このオブジェクトにアタッチされているコライダーをアクティブにする。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void ClliderOn()
    {
        _collider.enabled = true;
    }
    /// <summary>
    /// このオブジェクトにアタッチされているコライダーを非アクティブにする。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void ClliderOff()
    {
        _collider.enabled = false;
    }

    public virtual void HitPlayer(Rigidbody2D playerRb2D)
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensivePower;
        playerRb2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (transform.parent.GetComponent<EnemyBase>().IsRight)
        {
            playerRb2D.AddForce((Vector2.right + Vector2.up)* _blowingPower, ForceMode2D.Impulse);
        }
        else
        {
            playerRb2D.AddForce((Vector2.left + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
    }
}
