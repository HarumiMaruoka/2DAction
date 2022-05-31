using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Script : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    [SerializeField, Tooltip("Gizmo表示")] bool _isGizmo = true;

    //CheckGround
    //[SerializeField]
    //private CheckGround _checkGround;


    [SerializeField]
    private Vector2 _over_lap_box_center;
    [SerializeField]
    private Vector2 _over_lap_box_size;
    [SerializeField]
    LayerMask _layer_mask_Ground;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

   public void Jump(float jamp_power)
    {
        _over_lap_box_center = transform.position + new Vector3(0f, -0.725f, 0);

        //ジャンプ入力と接地判定があればジャンプする
        if (Input.GetButtonDown("Jump"))
        {
            //接地していればジャンプする
            if (GetIsGround() && !Input.GetButton("Fire2"))
            {
                rigidbody2d.velocity = new Vector3(0, 40, 0);
                //rigidbody2d.AddForce(Vector2.up * jamp_power, ForceMode2D.Impulse);
            }
        }
    }

    public bool GetIsGround()
    {
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _over_lap_box_center,
            _over_lap_box_size,
            0f,
            _layer_mask_Ground);

        if (collision.Length != 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_isGizmo)
        {
            Gizmos.DrawCube(_over_lap_box_center, _over_lap_box_size);
        }
    }
}
