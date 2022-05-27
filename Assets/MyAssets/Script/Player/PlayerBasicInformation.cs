using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーの体力などの基礎情報を持つクラス
public class PlayerBasicInformation : MonoBehaviour
{
    public int _maxHitPoint = 3;

    public int _playerHitPoint = 3;
    ChangePlayerState _changePlayerState;
    PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _changePlayerState = GetComponent<ChangePlayerState>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //player の体力がなくなったら消滅する
        if (_playerHitPoint < 1)
        {
            _playerController.enabled = false;
            _changePlayerState._isDead = true;
        }
    }

    //敵と接触したとき、敵の攻撃力分damageを受け、敵のforce分後方へ飛ばされる
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Enemyと接触したらEnemyのHitPlayer関数を実行する
        if (collision.gameObject.tag == "Enemy") 
        {
            collision.gameObject.GetComponent<EnemyBase>().HitPlayer();
            _changePlayerState.isHitEnemy = true;
        }
    }
}
