using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    [SerializeField] int _slashPower;
    Transform _playerPos;
    SpriteRenderer _playerSpriteRendere;

    SpriteRenderer _mySpriteRendere;

    bool _isRigth;

    // Start is called before the first frame update
    void Start()
    {
        _playerPos = GameObject.Find("ChibiRobo").GetComponent<Transform>();
        _playerSpriteRendere = GameObject.Find("ChibiRobo").GetComponent<SpriteRenderer>();

        _mySpriteRendere = GetComponent<SpriteRenderer>();

        _mySpriteRendere.flipX = _playerSpriteRendere.flipX;
        transform.position = _playerSpriteRendere.flipX ? _playerPos.position + Vector3.left : _playerPos.position + Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _playerPos.position;
        //プレイヤーが振り向いたら斬撃は消える
        if(_mySpriteRendere.flipX != _playerSpriteRendere.flipX)
        {
            Destroy(this.gameObject);
        }
    }

    //斬撃を消す処理、アニメーションイベントから呼び出す
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    //敵と接触したときに行う処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //ここに敵と接触したときの処理を書く
            collision.gameObject.GetComponent<EnemyBase>().HitPlayerAttadk(_slashPower);
        }
    }
}
