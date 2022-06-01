using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Script : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    [SerializeField, Tooltip("Gizmo�\��")] bool _isGizmo = true;

    [SerializeField]
    private Vector2 _over_lap_box_center;
    [SerializeField]
    private Vector2 _over_lap_box_size;
    [SerializeField]
    LayerMask _layer_mask_Ground;

    float _groundCheckPositionY = -0.725f;
    float _jumpPower = 40;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    /// <summary> �ڒn���Ă���΃W�����v���� </summary>
    public void Jump(float jamp_power)
    {
        _over_lap_box_center = transform.position + new Vector3(0f, _groundCheckPositionY, 0);

        //�W�����v���͂Ɛڒn���肪����΃W�����v����
        if (Input.GetButtonDown("Jump"))
        {
            //�ڒn���Ă���΃W�����v����
            if (GetIsGround() && !Input.GetButton("Fire2"))
            {
                rigidbody2d.velocity = new Vector3(0, _jumpPower, 0);
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
