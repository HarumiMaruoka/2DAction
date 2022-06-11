using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastController : MonoBehaviour
{
    //ブリンジャーとプレイヤーのトランスフォーム
    Transform _bringerTransForm;
    Transform _playerTransForm;

    //自身のコンポーネント
    Animator _animator;
    BoxCollider2D _collider2D;

    void Start()
    {
        _bringerTransForm = transform.parent.gameObject.transform;
        _playerTransForm = GameObject.Find("ChibiRobo").transform;

        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        UpdateCast();
    }

    void UpdateCast()
    {
        //ここに、Bringerが遠距離攻撃するときの処理を書く
    }

    /// <summary> キャストの発動時の処理 </summary>
    void PlayCast()
    {
        //アニメーション再生
        _animator.SetTrigger("Cast");
    }
}
