using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    /*
     Player input -> reload  / fire �޼ҵ� ȣ��

    Reload -> animation -> trigger 
     
     */
    public Gun gun;

    //�ѱ� ��ġ�� ���߱� ���� transform
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
        //input ���õ� �̺�Ʈ ȣ��
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
        //���� �������� ������ �Ȳ�ġ�� �̵�
        GunPivot.position =
            ani.GetIKHintPosition(AvatarIKHint.RightElbow);


        //ik�� �̿��ؿ� �޼��� ��ġ�� ȸ�� ���� �� ���� ������ �������� ����
        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

        ani.SetIKPosition(AvatarIKGoal.LeftHand, LeftHand_Mount.position);
        ani.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand_Mount.rotation);


        //ik�� �̿��ؿ� ������ ��ġ�� ȸ�� ���� �� ������ ������ �������� ����
        ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        ani.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

        ani.SetIKPosition(AvatarIKGoal.RightHand, RightHand_Mount.position);
        ani.SetIKRotation(AvatarIKGoal.RightHand, RightHand_Mount.rotation);
    }

}
