using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//ƒvƒŒƒCƒ„[‚Ì‘Ì—Í‚È‚Ç‚ÌŠî‘bî•ñ‚ğ‚ÂƒNƒ‰ƒX
public class PlayerBasicInformation : MonoBehaviour
{
    public int _maxHitPoint = 3;

    public int _playerHitPoint = 3;
    ChangePlayerState _changePlayerState;
    PlayerController _playerController;

    float _godModeTime = 1.5f;

    bool _isGodMode = false;

    // Start is called before the first frame update
    void Start()
    {
        _changePlayerState = GetComponent<ChangePlayerState>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //player ‚Ì‘Ì—Í‚ª‚È‚­‚È‚Á‚½‚çÁ–Å‚·‚é
        if (_playerHitPoint < 1)
        {
            _playerController.enabled = false;
            _changePlayerState._isDead = true;
        }
    }

    //“G‚ÆÚG‚µ‚½‚Æ‚«A“G‚ÌUŒ‚—Í•ªdamage‚ğó‚¯A“G‚Ìforce•ªŒã•û‚Ö”ò‚Î‚³‚ê‚é
    private void OnCollisionStay2D(Collision2D collision)
    {
        //–³“Gó‘Ô‚Å‚ ‚ê‚ÎUŒ‚‚ğó‚¯‚È‚¢
        if (!_isGodMode)
        {
            //Enemy‚ÆÚG‚µ‚½‚çEnemy‚ÌHitPlayerŠÖ”‚ğÀs‚·‚é
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyBase>().HitPlayer();
                _changePlayerState._isHitEnemy = true;
                StartCoroutine(GodMode());
            }
        }
    }
    ////“G‚ÆÚG‚µ‚½‚Æ‚«A“G‚ÌUŒ‚—Í•ªdamage‚ğó‚¯A“G‚Ìforce•ªŒã•û‚Ö”ò‚Î‚³‚ê‚é
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!_isGodMode)
    //    {
    //        //Enemy‚ÆÚG‚µ‚½‚çEnemy‚ÌHitPlayerŠÖ”‚ğÀs‚·‚é
    //        if (collision.gameObject.tag == "Enemy")
    //        {
    //            collision.gameObject.GetComponent<EnemyBase>().HitPlayer();
    //            _changePlayerState.isHitEnemy = true;
    //            StartCoroutine(GodMode());
    //        }
    //    }
    //}
    IEnumerator GodMode()
    {
        _isGodMode = true;
        yield return new WaitForSeconds(_godModeTime);
        _isGodMode = false;
    }
}
