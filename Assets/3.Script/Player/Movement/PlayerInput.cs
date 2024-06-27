using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string MoveAxis_name = "Vertical";
    [SerializeField] private string RotateAxis_name = "Horizontal";
    [SerializeField] private string Fire_name = "Fire1";
    [SerializeField] private string Reload_name = "Reload";


    //getaxis -> float
    public float Move_Value { get; private set; }
    public float Rotate_Value { get; private set; }

    //getbutton -> bool
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }

    private void Update()
    {
        //미래에게...나중에 너가 게임오버를 만들면 못 움직도록 해줘...부탁할게...
        //todo 0624

        Move_Value = Input.GetAxis(MoveAxis_name);
        Rotate_Value = Input.GetAxis(RotateAxis_name);

        isFire = Input.GetButton(Fire_name);
        isReload = Input.GetButton(Reload_name);
    }

}
