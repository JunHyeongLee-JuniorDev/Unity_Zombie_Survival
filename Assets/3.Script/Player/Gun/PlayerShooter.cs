using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*
     Player input -> reload  / fire 메소드 호출

    Reload -> animation -> trigger 
     
     */
    public Gun gun;

    //총기 위치를 맟추기 위한 transform
    public Transform GunPivot;
    public Transform LeftHand_Mount;
    public Transform RightHand_Mount;

    [SerializeField] private Animator ani;
    [SerializeField] private PlayerInput input;

    private void Start()
    {
        ani = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        //input 관련된 이벤트 호출
        if(input.isFire)
        {
            gun.Fire();
        }
        else if(input.isReload)
        {
            if(gun.Reload())
            {
                ani.SetTrigger("Reload");
            }
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //총의 기준점을 오른쪽 팔꿈치로 이동
        GunPivot.position =
            ani.GetIKHintPosition(AvatarIKHint.RightElbow);


        //ik를 이용해여 왼손의 위치와 회전 값을 총 왼쪽 손잡이 기준으로 맟춤
        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        ani.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand_Mount.position);
        ani.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand_Mount.rotation);


        //ik를 이용해여 오른쪽 위치와 회전 값을 총 오른쪽 손잡이 기준으로 맟춤
        ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        ani.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        ani.SetIKPosition(AvatarIKGoal.RightHand, RightHand_Mount.position);
        ani.SetIKRotation(AvatarIKGoal.RightHand, RightHand_Mount.rotation);
    }

}
