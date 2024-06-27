using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="ScriptableObject/Gundata", 
    fileName ="Gun_Data")]
public class Gun_Data : ScriptableObject
{
    /*
        공격력          -> float 
        연사력          -> float / 코루틴(시간)
        재장전시간       -> float
        탄창용량        -> int
        전체 탄창용량    -> int
        총소리          -> audio clip
        재장전소리       -> audio clip
     */


    public float Damage = 25f; //공격력

    public float Timebet_fire = 0.12f;//연사력
    public float Reload_Time = 1.8f;//재장전 시간

    public int MagCapacity = 30;//탄창용량
    public int StartAmmoRemain = 100;

    public AudioClip Shotclip;
    public AudioClip Reload_clip;

}
