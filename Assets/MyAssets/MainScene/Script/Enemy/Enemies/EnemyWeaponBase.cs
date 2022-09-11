using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーが扱う武器の基底クラス
/// </summary>
public class EnemyWeaponBase : MonoBehaviour, AttackOnPlayer
{
    [Header("攻撃力"), SerializeField] float _offensivePower;
    [Header("攻撃力"), SerializeField] float _blowingPower;
    public virtual void HitPlayer()
    {
        Debug.LogError("間違った方が呼ばれています！修正してください！");
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.PLAYER_TAG_NAME)
        {
            PlayerStatusManager.Instance.PlayerHealthPoint -= _offensivePower;
        }
        else
        {
            Debug.LogError($"プレイヤーのタグは{Constants.PLAYER_TAG_NAME}");
        }
    }
}
