using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    //[SerializeField] private float RotateSpeed = 180f;

    [SerializeField] private PlayerInput playerinput;

    private Camera mainCam;

    private Rigidbody player_r;
    private Animator player_ani;

    private Vector3 MousePositionViewport = Vector3.zero;

    private void Start()
    {
        mainCam = Camera.main;
        TryGetComponent(out playerinput);
        TryGetComponent(out player_r);
        TryGetComponent(out player_ani);
    }

    private void FixedUpdate()
    {
        Move();
        Aim();
        player_ani.SetFloat("Move", playerinput.Move_Value);
    }

    private void Move()
    {
        Vector3 MoveDirection =
            playerinput.Move_Value * transform.forward * MoveSpeed * Time.deltaTime;
        player_r.MovePosition(player_r.position + MoveDirection);
    }

    [SerializeField] private LayerMask groundMask;

    private void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;

            direction.y = 0;

            transform.forward = direction;
        }
    }

    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            return (success: true, position: hitInfo.point);
        }
        else
        {
            return (success: false, position: Vector3.zero);
        }
    }
}
