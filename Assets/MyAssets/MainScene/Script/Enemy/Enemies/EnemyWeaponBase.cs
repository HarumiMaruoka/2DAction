using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy / Bossが扱う武器の基底クラス : <br/>
/// このクラスにはプレイヤーに対する攻撃に関する処理を記載してある。<br/>
/// 
/// 敵が放つ
/// </summary>
public class EnemyWeaponBase : MonoBehaviour, IAttackOnPlayer
{
    //===== メンバー変数 =====//
    Collider2D _collider2D;

    [Header("攻撃力"), SerializeField] float _offensivePower;
    [Header("ノックバック力"), SerializeField] float _blowingPower;

    //===== Unityメッセージ =====//
    /// <summary>
    /// 初期化処理。コライダーを取得する。
    /// </summary>
    protected virtual void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }
    /// <summary> プレイヤーに接触したらプレイヤーの体力を減らす。 </summary>
    /// <param name="collision"> 接触相手 </param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.PLAYER_TAG_NAME)
        {
            // プレイヤーの体力を減らす。
            if (collision.TryGetComponent(out Rigidbody2D playerRb2D))
                HitPlayer(playerRb2D);
        }
    }
    /// <summary> 
    /// このオブジェクトにアタッチされているコライダーをアクティブにする。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void ClliderOn()
    {
        _collider2D.enabled = true;
    }
    /// <summary>
    /// このオブジェクトにアタッチされているコライダーを非アクティブにする。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void ClliderOff()
    {
        _collider2D.enabled = false;
    }
    /// <summary>
    /// このゲームオブジェクトおよびアタッチされたコンポーネントを破棄する。<br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    public virtual void HitPlayer(Rigidbody2D playerRb2D)
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensivePower;
        playerRb2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG_NAME).transform.position.x >
            transform.position.x)
        {
            playerRb2D.AddForce((Vector2.right + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
        else
        {
            playerRb2D.AddForce((Vector2.left + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
    }
}
