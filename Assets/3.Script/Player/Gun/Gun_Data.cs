using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ScriptableObject/Gundata", 
    fileName ="Gun_Data")]
public class Gun_Data : ScriptableObject
{
    /*
        ���ݷ�          -> float 
        �����          -> float / �ڷ�ƾ(�ð�)
        �������ð�       -> float
        źâ�뷮        -> int
        ��ü źâ�뷮    -> int
        �ѼҸ�          -> audio clip
        �������Ҹ�       -> audio clip
     */


    public float Damage = 25f; //���ݷ�

    public float Timebet_fire = 0.12f;//�����
    public float Reload_Time = 1.8f;//������ �ð�

    public int MagCapacity = 30;//źâ�뷮
    public int StartAmmoRemain = 100;

    public AudioClip Shotclip;
    public AudioClip Reload_clip;

}
