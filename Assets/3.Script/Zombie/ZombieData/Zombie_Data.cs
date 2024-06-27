using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptablieObject/Zombie_Data",
    fileName = "Zombie_Data")]

public class Zombie_Data : ScriptableObject
{
    /*
     * ü��
     * �̵��ӵ�
     * ���ݷ�
     * �Ǻλ�
     */

    public float Health = 100f;
    public float Damage = 20f;
    public float Speed = 2f;

    public Color skinColor = Color.white;
}
